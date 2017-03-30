using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Game engine has requested an action from this player.");

    string move = GetMove();

    log.Info("Player has chosen " + move);

    return req.CreateResponse(HttpStatusCode.OK, move);
}

public static string GetMove()
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