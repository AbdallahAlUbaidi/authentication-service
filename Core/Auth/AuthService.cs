namespace Core.Auth;

public class AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAuthService
{
	private readonly IUserRepository _UserRepository = userRepository;
	private readonly IPasswordHasher _PasswordHasher = passwordHasher;

	public async Task<User> Login(LoginInput input)
	{
		User? user = await _UserRepository.GetUserByName(input.Username)
		?? throw new InvalidCredentialException();

		bool isPasswordValid = _PasswordHasher.Compare(input.Password, user.Password);

		if (!isPasswordValid)
			throw new InvalidCredentialException();

		return user;
	}

	public async Task<User> Register(RegisterInput input)
	{
		if (await IsUsernameUsed(input.Username))
			throw new UserAlreadyExistsException();

		string hashedPassword = _PasswordHasher.HashPassword(input.Password);

		User user = new(input.Username, hashedPassword);

		await _UserRepository.Save(user);

		return user;
	}
	public class UserAlreadyExistsException : Exception
	{
	}

	private async Task<bool> IsUsernameUsed(string username)
	{
		User? user = await _UserRepository.GetUserByName(username);

		return user is not null;
	}

}
