namespace Core.Auth;

public interface IUserRepository
{
	Task Save(User user);

	Task<User?> GetUserByName(string username);
}