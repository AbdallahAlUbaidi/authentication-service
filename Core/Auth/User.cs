namespace Core.Auth;

public class User(string username, string password)
{
	public string Username { get; set; } = username;
	public string Password { get; set; } = password;

}