public static class Endpoints
{
    // API
    public const string ApiUrlLocal = "https://localhost:8080/api";
    public const string ApiUrlCloud = "https://intotheblack-api.onrender.com/api";

    // AUTH
    public const string Auth = "/auth";
    public const string Login = Auth + "/login";
    public const string Register = Auth + "/ register";

    // USERS
    public const string GetUserPlayers = "/users/{0}/players";

    // PLAYERS
    public const string Players = "/players";
    public const string PlayerTime = Players + "/{0}/time";
    public const string ResetPlayerData = Players + "/{0}/reset-data";
    public const string SaveFragmentOnPlayer = Players + "{0}/fragment/add/{1}";
    public const string PlayersWithId = Players + "/{0}";

    // FRAGMENTS
}
