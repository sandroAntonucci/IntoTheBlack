using System;

[Serializable]
public class UserRequestDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    public UserRequestDTO(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }

    public UserRequestDTO(string username, string password)
    {
        Username = username;
        Password = password;
        Email = null;
    }
}

