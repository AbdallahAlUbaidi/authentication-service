namespace Core.Auth;

public interface IPasswordHasher
{
	string HashPassword(string password);

	bool Compare(string password, string hash);

}

