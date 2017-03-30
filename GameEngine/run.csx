using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Game starting");

    // parse query parameter
    string player1URL = GetPlayer1URL(req);
    string player2URL = GetPlayer2URL(req);

    log.Info("Player 1 is at URL " + player1URL);
    log.Info("Player 2 is at URL " + player2URL);

    return req.CreateResponse(HttpStatusCode.OK, "Game Complete");
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