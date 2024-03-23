namespace Tests;

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

