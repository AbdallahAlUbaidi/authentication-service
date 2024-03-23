namespace Infrastructure;

using Core.Auth;

public class FakePasswordHasher : IPasswordHasher
{
	public bool Compare(string password, string hash)
	{
		return $"${password}$" == hash;
	}

	public string HashPassword(string password)
	{
		return $"${password}$";
	}
}

