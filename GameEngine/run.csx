#r "Newtonsoft.Json"
using System.Net;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Game starting");

    string jsonContent = req.Content.ReadAsStringAsync().Result;
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    string player1Url = data.Player1URL;
    string player2Url = data.Player2URL;

    var gameEvents = PlayGame(player1Url, player2Url, log);

    return req.CreateResponse(HttpStatusCode.OK, gameEvents);
}

private static List<string> PlayGame(string player1Url, string player2Url, TraceWriter log)
{
    string gameStatus = "Draw";
    var gameEvents = new List<string>();

    do
    {
        string player1Move = GetMoveFromPlayer(player1Url);
        string player1MoveEventMessage = "Player 1 chooses " + player1Move;
        ReportEventToPlayer(player2Url, player1MoveEventMessage);
        gameEvents.Add(player1MoveEventMessage);
        log.Info(player1MoveEventMessage);

        string player2Move = GetMoveFromPlayer(player2Url);
        string player2MoveEventMessage = "Player 2 chooses " + player2Move;
        ReportEventToPlayer(player1Url, player2MoveEventMessage);
        gameEvents.Add(player2MoveEventMessage);
        log.Info(player2MoveEventMessage);

        gameStatus = DetermineGameStatus(player1Move, player2Move);

        gameEvents.Add(gameStatus);
        log.Info(gameStatus);
    }
    while (gameStatus == "Draw");

    return gameEvents;
}


private static void ReportEventToPlayer(string playerUrl, string eventMessage)
{
    var client = new RestClient(playerUrl);

    var request = new RestRequest(Method.POST);

    request.AddHeader("Content-type", "application/json");
    request.AddJsonBody( new { message = eventMessage } );

    var response = client.Execute(request);
}

private static string DetermineGameStatus(string player1Move, string player2Move)
{
    string winningPlayer = "Player 2";

    if(player1Move == player2Move)
        return "Draw";

    if(player1Move == "Rock" && player2Move == "Scissors")
        winningPlayer = "Player 1";

    if(player1Move == "Paper" && player2Move == "Rock")
        winningPlayer = "Player 1";

    if(player1Move == "Scissors" && player2Move == "Paper")
       winningPlayer = "Player 1";

    return winningPlayer + " Wins";
}

private static string GetMoveFromPlayer(string playerUrl)
{
    var client = new RestClient(playerUrl);

    var request = new RestRequest(Method.GET);

    request.AddHeader("Content-type", "application/json");

    var response = client.Execute(request);

    string moveWithQuotesStripped = response.Content.Replace("\"", "");

    return moveWithQuotesStripped;
}

private static string GetPlayer1Url(HttpRequestMessage req)
{
    return req.GetQueryNameValuePairs()
    .FirstOrDefault(q => string.Compare(q.Key, "Player1URL", true) == 0)
    .Value;
}

private static string GetPlayer2Url(HttpRequestMessage req)
{
    return req.GetQueryNameValuePairs()
    .FirstOrDefault(q => string.Compare(q.Key, "Player2URL", true) == 0)
    .Value;
}