namespace Infrastructure;

using System.Threading.Tasks;
using Core.Auth;

public class FakeUserRepository : IUserRepository
{
	public List<User> Users = [];

	public bool IsEmpty()
	{
		return Users.Count == 0;
	}

	public User? GetLast()
	{
		if (IsEmpty()) return null;

		return Users.FindLast((u) => true);

	}

	public Task Save(User user)
	{
		Users.Add(user);

		return Task.CompletedTask;
	}

	public Task<User?> GetUserByName(string username)
	{
		User? user = Users.Find(u => u.Username == username);

		return Task.FromResult(user);
	}
}

