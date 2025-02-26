public static class Endpoints
{
    // API
    public const string ApiUrlLocal = "https://localhost:8080/api";
    public const string ApiUrlCloud = "https://intotheblack-api.onrender.com/api";

    // AUTH
    public const string Login = "/auth/login";
    public const string Register = "/auth/register";

    // USERS
    public const string GetUserPlayers = "/users/{0}/players";

    // PLAYERS
    public const string SaveTimePlayer = "/players/{0}/time";
    public const string ResetPlayerData = "/players/{0}/reset-data";
    public const string SaveFragmentOnPlayer = "/players/{0}/fragment/add/{1}";
    public const string CreatePlayer = "/players";
    public const string GetPlayers = "/players/{0}";
    public const string DeletePlayer = "/players/{0}";

    // FRAGMENTS


}
