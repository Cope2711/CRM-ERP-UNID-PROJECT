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
            IsActive = true,
            Roles = new List<RoleDto>
            {
                new RoleDto
                {
                    RoleId = Guid.Parse("735250a8-d410-4f77-870a-4422ab28a1a1"),
                    RoleName = "Admin"
                }
            }
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
            IsActive = true,
        };

        // Act
        var response =
            await _client.GetFromJsonAsync<UserDto>($"/api/users/get-by-username?username={expectedUser.UserUserName}");

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

    [Fact]
    public async Task GetAllUsers_WhenGetAllDtoIsValid_ReturnsExpectedUsers()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 1,
            OrderBy = "UserUserName",
            Descending = false,
            SearchTerm = null,
            SearchColumn = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        GetAllResponseDto<UserDto> getAllResponseDto =
            await response.Content.ReadFromJsonAsync<GetAllResponseDto<UserDto>>();

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getAllResponseDto.Should().NotBeNull();
        getAllResponseDto.Data.Should().NotBeNull();
        getAllResponseDto.Data.Should().HaveCount(1);
        getAllResponseDto.TotalItems.Should().Be(3);
        getAllResponseDto.PageNumber.Should().Be(1);
        getAllResponseDto.PageSize.Should().Be(1);
        getAllResponseDto.TotalPages.Should().Be(3);
    }

    [Fact]
    public async Task GetAllUsers_WhenOrderByIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 1,
            OrderBy = "invalid-column",
            Descending = false,
            SearchTerm = null,
            SearchColumn = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllUsers_WhenSearchTermIsValid_ReturnsExpectedOrder()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "UserUserName",
            Descending = false,
            SearchTerm = "a",
            SearchColumn = "UserUserName"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        GetAllResponseDto<UserDto> getAllResponseDto =
            await response.Content.ReadFromJsonAsync<GetAllResponseDto<UserDto>>();

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getAllResponseDto.Data[0].UserUserName.Should().Be("admin");
        getAllResponseDto.Data[1].UserUserName.Should().Be("janedoe");
        
    }
    
    [Fact]
    public async Task GetAllUsers_WhenSearchColumnIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 1,
            OrderBy = "UserUserName",
            Descending = false,
            SearchTerm = null,
            SearchColumn = "invalid-column"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetAllUsers_WhenSearchTermIsInvalid_ReturnsEmptyData()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "UserUserName",
            Descending = false,
            SearchTerm = "invalid-term",
            SearchColumn = "UserUserName"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        GetAllResponseDto<UserDto> getAllResponseDto =
            await response.Content.ReadFromJsonAsync<GetAllResponseDto<UserDto>>();

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getAllResponseDto.Data.Should().BeEmpty();
        getAllResponseDto.TotalItems.Should().Be(0);
    }
    
    [Fact]
    public async Task GetAllUsers_WhenDescendingIsTrue_ReturnsExpectedOrder()
    {
        // Arrange
        GetAllDto getAllDto = new GetAllDto
        {
            PageNumber = 1,
            PageSize = 10,
            OrderBy = "UserUserName",
            Descending = true,
            SearchTerm = "a",
            SearchColumn = "UserUserName"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/get-all", getAllDto);

        GetAllResponseDto<UserDto> getAllResponseDto =
            await response.Content.ReadFromJsonAsync<GetAllResponseDto<UserDto>>();

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        getAllResponseDto.Data[0].UserUserName.Should().Be("janedoe");
        getAllResponseDto.Data[1].UserUserName.Should().Be("admin");
    }
}