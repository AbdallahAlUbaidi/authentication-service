namespace Infrastructure;

using Core.Auth;

public class FakePasswordHasher : IPasswordHasher
{
	public string HashPassword(string password)
	{
		return $"${password}$";
	}
}

