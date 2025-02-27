using System;

[Serializable]
public class User
{
    public string token;
    public UserData user;

    public User(UserData user, string token)
    {
        this.user = user;
        this.token = token;
    }
}