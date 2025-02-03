using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

public class UsersControllerShouldTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;

    public UsersControllerShouldTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        CreateUserDto createUserDto = new CreateUserDto
        {
            UserUserName = "test-user-1",
            UserFirstName = "Test",
            UserLastName = "User",
            UserEmail = "test-user-1@test.com",
            UserPassword = "123456",
            IsActive = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/create", createUserDto);

        // Assert
        response.Should().NotBeNull();

        // Obtener el UserDto de la respuesta
        UserDto? userDto = await response.Content.ReadFromJsonAsync<UserDto>();

        // Validaciones adicionales
        userDto.Should().NotBeNull();
        userDto.UserUserName.Should().Be(createUserDto.UserUserName);
    }

    [Fact]
    public async Task CreateUser_WhenUserNameAlreadyExists_ReturnsConflict()
    {
        // Arrange
        CreateUserDto createUserDto = new CreateUserDto
        {
            UserUserName = "admin",
            UserFirstName = "Admin",
            UserLastName = "User",
            UserEmail = "admin@admin.com",
            UserPassword = "123456",
            IsActive = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/create", createUserDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateUser_WhenEmailAlreadyExists_ReturnsConflict()
    {
        // Arrange
        CreateUserDto createUserDto = new CreateUserDto
        {
            UserUserName = "test-user-11",
            UserFirstName = "Test",
            UserLastName = "User",
            UserEmail = "admin@admin.com",
            UserPassword = "123456",
            IsActive = true
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/create", createUserDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetUserById_ReturnsExpectedUser()
    {
        // Arrange
        UserDto expectedUser = new UserDto
        {
            UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
            UserUserName = "admin",
            UserFirstName = "Admin",
            UserLastName = "User",
            UserEmail = "admin@admin.com",
            IsActive = true
        };

        // Act
        var response = await _client.GetFromJsonAsync<UserDto>($"/api/users/get-by-id?id={expectedUser.UserId}");

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        Guid nonExistentUserId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/users/get-by-id?id={nonExistentUserId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserByUsername_ReturnsExpectedUser()
    {
        UserDto expectedUser = new UserDto
        {
            UserId = Guid.Parse("d7f9ed1e-417e-46c0-98f3-df8d63e1e8b6"),
            UserUserName = "admin",
            UserFirstName = "Admin",
            UserLastName = "User",
            UserEmail = "admin@admin.com",
            IsActive = true
        };

        // Act
        var response = await _client.GetFromJsonAsync<UserDto>($"/api/users/get-by-username?username={expectedUser.UserUserName}");

        // Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetUserByUsername_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        string nonExistentUserName = "non-existent-user";

        // Act
        var response = await _client.GetAsync($"/api/users/get-by-username?username={nonExistentUserName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ExistUserByEmail_ReturnsTrue()
    {
        // Arrange
        string email = "admin@admin.com";

        // Act
        var response = await _client.GetAsync($"/api/users/exist-user-by-email?email={email}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.ReadAsStringAsync().Result.Should().Be("true");
    }

    [Fact]
    public async Task ExistUserByEmail_ReturnsFalse()
    {
        // Arrange
        string email = "non-existent-email@email.com";

        // Act
        var response = await _client.GetAsync($"/api/users/exist-user-by-email?email={email}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.ReadAsStringAsync().Result.Should().Be("false");
    }

    [Fact]
    public async Task ExistUserByUsername_ReturnsTrue()
    {
        // Arrange
        string username = "admin";

        // Act
        var response = await _client.GetAsync($"/api/users/exist-user-by-username?username={username}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.ReadAsStringAsync().Result.Should().Be("true");
    }

    [Fact]
    public async Task ExistUserByUsername_ReturnsFalse()
    {
        // Arrange
        string username = "non-existent-username";

        // Act
        var response = await _client.GetAsync($"/api/users/exist-user-by-username?username={username}");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.ReadAsStringAsync().Result.Should().Be("false");
    }
}