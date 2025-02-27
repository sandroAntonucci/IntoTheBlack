using System;

[Serializable]
public class RegisterRequest
{
    public string username;
    public string password;
    public string email;

    public RegisterRequest(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
}

