#r "Newtonsoft.Json"
using System.Net;
using Newtonsoft.Json;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    var method = req.Method;

    if(method == HttpMethod.Get)
        return GetResponseToMoveRequest(req, log);

    ReceiveGameEvent(req, log);

    return req.CreateResponse(HttpStatusCode.OK);
}

private static async void ReceiveGameEvent(HttpRequestMessage req, TraceWriter log)
{
    string jsonContent = await req.Content.ReadAsStringAsync();
    dynamic eventFromGameEngine = JsonConvert.DeserializeObject(jsonContent);

    string eventMessage = eventFromGameEngine.message;
    log.Info("Game engine has posted event with message: " + eventMessage);
}

private static HttpResponseMessage GetResponseToMoveRequest(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Game engine has requested an action from this player.");

    string move = GetMove();

    log.Info("Player has chosen " + move);

    return req.CreateResponse(HttpStatusCode.OK, move);    
}

private static string GetMove()
{
    string move = "Player Forfiets";
    int randomNumber = new Random().Next(0, 3);
    
    if(randomNumber == 0)
        move = "Rock";

    if(randomNumber == 1)
        move = "Paper";

    if(randomNumber == 2)
        move = "Scissors";

    return move;
}