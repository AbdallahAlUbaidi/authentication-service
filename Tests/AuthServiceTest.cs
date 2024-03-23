namespace Tests;

using System.Diagnostics;
using System.Threading.Tasks;
using Core.Auth;
using Infrastructure;

public class AuthServiceRegisterTest
{

    private readonly AuthService Service;
    private readonly FakeUserRepository FakeUserRepository;

    private readonly FakePasswordHasher FakePasswordHasher;

    public AuthServiceRegisterTest()
    {
        FakeUserRepository = new();
        FakePasswordHasher = new();
        Service = new AuthService(FakeUserRepository, FakePasswordHasher);
    }

    [Fact]
    public async void WhenRegisteringWithValidInput_ItShouldReturnTheNewUser()
    {
        //Arrange
        string username = "Abdullah";
        string password = "123123Aa";

        //Act
        User newUser = await Service.Register(new(username, password));

        //Assert
        newUser.Username.Should().BeEquivalentTo(username);
    }

    [Fact]
    public async void WhenRegisteringWithValidInput_ItShouldSaveTheUser()
    {
        //Arrange
        string username = "Abdullah";
        string password = "123123Aa";

        //Act
        User user = await Service.Register(new(username, password));

        //Assert
        FakeUserRepository.IsEmpty().Should().NotBe(true);
        FakeUserRepository.GetLast().Should().Be(user);
    }

    [Fact]
    public async void GivenThatTheUsernameIsUsed_ItShould_ThrowUserAlreadyExistsException()
    {
        //Arrange
        User existingUser = new("Abdullah", "123123");
        await FakeUserRepository.Save(existingUser);

        //Act
        Func<Task<User>> act = async () => await Service.Register(new("Abdullah", "123141"));

        //Assert
        await act.Should().ThrowAsync<AuthService.UserAlreadyExistsException>();
    }

    [Fact]
    public async void WhenRegisteringWithValidInput_ItShouldHashThePassword()
    {
        //Act
        User user = await Service.Register(new("Abdullah", "123123"));

        //Assert
        user.Password.Should().Be("$123123$");
    }
}

public class AuthServiceLoginTest
{
    private readonly AuthService Service;
    private readonly FakeUserRepository FakeUserRepository;

    private readonly FakePasswordHasher FakePasswordHasher;

    public AuthServiceLoginTest()
    {
        FakeUserRepository = new();
        FakePasswordHasher = new();
        Service = new AuthService(FakeUserRepository, FakePasswordHasher);
    }

    [Fact]
    public async void GivenThatUserDoesNotExists_ItShouldThrowInvalidCredentialsException()
    {
        //Act
        Func<Task<User>> act = async () => await Service.Login(new("Abdullah", "1234Aa"));

        //Assert
        await act.Should().ThrowAsync<InvalidCredentialException>();
    }

    [Fact]
    public async void GivenThatTheUserExists_ItShouldReturnTheUser()
    {
        //Arrange
        string username = "Abdullah";
        string password = "123123Aa";
        User existingUser = new(username, $"${password}$");
        await FakeUserRepository.Save(existingUser);

        //Act
        User user = await Service.Login(new(username, password));

        //Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public async void GivenThatTheUserExistsButPasswordIsInvalid_ItShouldThrowInvalidCredentialsError()
    {
        //Arrange
        string username = "Abdullah";
        string password = "123123Aa";
        string invalidPassword = "123123Bb";
        User existingUser = new(username, $"${password}$");
        await FakeUserRepository.Save(existingUser);

        //Act   
        Func<Task<User>> act = async () => await Service.Login(new(username, invalidPassword));

        //Assert
        await act.Should().ThrowAsync<InvalidCredentialException>();

    }

}
