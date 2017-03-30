using System.Net;
using RestSharp;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Game starting");

    // parse query parameter
    string player1URL = GetPlayer1URL(req);
    string player2URL = GetPlayer2URL(req);

    log.Info("Player 1 is at URL " + player1URL);
    log.Info("Player 2 is at URL " + player2URL);

    string gameStatus = "Draw";

    do
    {
        string player1Move = GetMoveFromPlayer(player1URL);
        log.Info("Player1 chooses " + player1Move);

        string player2Move = GetMoveFromPlayer(player2URL);
        log.Info("Player2 chooses " + player2Move);

        gameStatus = DetermineWinner(player1Move, player2Move);
        log.Info(gameStatus);
    }
    while(gameStatus == "Draw");

    return req.CreateResponse(HttpStatusCode.OK, gameStatus);
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

    return winningPlayer + "Wins";
}

private static string GetMoveFromPlayer(string playerUrl)
{
    var client = new RestClient(playerUrl);

    var request = new RestRequest(Method.GET);

    request.AddHeader("Content-type", "application/json");

    var response = client.Execute(request);

    return response.Content;
}

private static string GetPlayer1URL(HttpRequestMessage req)
{
    return req.GetQueryNameValuePairs()
    .FirstOrDefault(q => string.Compare(q.Key, "Player1URL", true) == 0)
    .Value;
}

private static string GetPlayer2URL(HttpRequestMessage req)
{
    return req.GetQueryNameValuePairs()
    .FirstOrDefault(q => string.Compare(q.Key, "Player2URL", true) == 0)
    .Value;
}