using System;

[Serializable]
public class User
{
    public string Username { get; set; }
    public string Token { get; set; }

    public User(string username, string token)
    {
        this.Username = username;
        this.Token = token;
    }
}
