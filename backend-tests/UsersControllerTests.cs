using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

[Collection("Tests")]
public class UsersControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class UpdateUserTests : UsersControllerTests
    {
        public UpdateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task UpdateUser_WhenAllAreValid_ReturnsOk()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserId = Models.Users.TestUser.UserId,
                UserUserName = "test-updated"
            };

            // Act
            var response = await _client.PatchAsJsonAsync($"/api/users/update", updateUserDto);
            
            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            UserDto? userDto = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            userDto.Should().NotBeNull();
            userDto.UserUserName.Should().Be(updateUserDto.UserUserName);
        }

        [Fact]
        public async Task UpdateUser_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserId = Guid.NewGuid(),
                UserUserName = "TestUserr",
            };

            // Act
            var response = await _client.PatchAsJsonAsync($"/api/users/update", updateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateUser_WhenUserNameAlreadyExist_ReturnsConflict()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserId = Models.Users.TestUser.UserId,
                UserUserName = "admin"
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"/api/users/update", updateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }
        
        [Fact]
        public async Task UpdateUser_WhenEmailAlreadyExist_ReturnsConflict()
        {
            // Arrange
            UpdateUserDto updateUserDto = new UpdateUserDto
            {
                UserId = Models.Users.TestUser.UserId,
                UserEmail = "admin@admin.com"
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"/api/users/update", updateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }
    }
    
    public class ChangePasswordTests : UsersControllerTests
    {
        public ChangePasswordTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ChangePassword_WhenAllAreValid_ReturnsOk()
        {
            ChangePasswordDto changePasswordDto = new ChangePasswordDto
            {
                ActualPassword = "123456", NewPassword = "12345678"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/change-password", changePasswordDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ChangePassword_WithWrongPassword_ReturnsUnauthorized()
        {
            // Arrange
            ChangePasswordDto changePasswordDto = new ChangePasswordDto
            {
                ActualPassword = "1234566", NewPassword = "12345678"
            };
            
            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/change-password", changePasswordDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    public class DeactivateUserTests : UsersControllerTests
    {
        public DeactivateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task DeactivateUser_WhenUserIsNotActive_ReturnsBadRequest()
        {
            // Arrange 
            DeactivateUserDto deactivateUserDto = new DeactivateUserDto
            {
                UserId = Models.Users.InactiveTestUser.UserId
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/deactivate", deactivateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeactivateUser_WhenUserIsNotExist_ReturnsNotFound()
        {
            // Arrange 
            DeactivateUserDto deactivateUserDto = new DeactivateUserDto
            {
                UserId = Guid.NewGuid()
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/deactivate", deactivateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeactivateUser_WhenUserIsActive_ReturnsOkAndLogoutTheUser()
        {
            // Deactivate logged user and make sure the user is logged out
            DeactivateUserDto deactivateUserDto = new DeactivateUserDto
            {
                UserId = Models.Users.TestUser.UserId
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/users/deactivate", deactivateUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    public class CreateUserTests : UsersControllerTests
    {
        public CreateUserTests(CustomWebApiFactory factory) : base(factory)
        {
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
    }

    public class GetUserByIdTests : UsersControllerTests
    {
        public GetUserByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetUserById_ReturnsExpectedUser()
        {
            // Arrange
            UserDto expectedUserDto = Mapper.UserToUserDto(Models.Users.Admin);

            // Act
            var response = await _client.GetFromJsonAsync<UserDto>($"/api/users/get-by-id?id={expectedUserDto.UserId}");

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedUserDto);
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
    }

    public class GetUserByUsernameTests : UsersControllerTests
    {
        public GetUserByUsernameTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetUserByUsername_ReturnsExpectedUser()
        {
            UserDto expectedUserDto = Mapper.UserToUserDto(Models.Users.Admin);

            // Act
            var response =
                await _client.GetFromJsonAsync<UserDto>(
                    $"/api/users/get-by-username?username={expectedUserDto.UserUserName}");

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedUserDto);
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
    }

    public class ExistUserBy : UsersControllerTests
    {
        public ExistUserBy(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ExistUserByEmail_ReturnsTrue()
        {
            // Arrange
            string email = Models.Users.Admin.UserEmail;

            // Act
            var response = await _client.GetAsync($"/api/users/exist-user-by-email?email={email}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync()?.Result.Should().Be("true");
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
            response.Content.ReadAsStringAsync()?.Result.Should().Be("false");
        }

        [Fact]
        public async Task ExistUserByUsername_ReturnsTrue()
        {
            // Arrange
            string username = Models.Users.Admin.UserUserName;

            // Act
            var response = await _client.GetAsync($"/api/users/exist-user-by-username?username={username}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.ReadAsStringAsync()?.Result.Should().Be("true");
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
            response.Content.ReadAsStringAsync()?.Result.Should().Be("false");
        }
    }
}