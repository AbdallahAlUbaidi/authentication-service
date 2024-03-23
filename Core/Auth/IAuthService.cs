namespace Core.Auth;

public interface IAuthService
{
	public Task<User> Register(RegisterInput input);
	public Task<User> Login(LoginInput input);
}
