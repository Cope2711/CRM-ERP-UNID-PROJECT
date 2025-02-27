using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;

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

        public static IEnumerable<object[]> UpdateUserTestData()
        {
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Models.Users.TestUser.UserId,
                    UserUserName = "test-updated"
                },
                HttpStatusCode.OK
            };

            // When User does not exist
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Guid.NewGuid(),
                    UserUserName = "TestUserr",
                },
                HttpStatusCode.NotFound
            };

            // When UserName already exist
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Models.Users.TestUser.UserId,
                    UserUserName = "admin"
                },
                HttpStatusCode.Conflict
            };

            // When Email already exist
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Models.Users.TestUser.UserId,
                    UserEmail = "admin@admin.com"
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(UpdateUserTestData))]
        public async Task UpdateUser_ReturnsExpectedResult(UpdateUserDto updateUserDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"/api/users/update", updateUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ChangePasswordTests : UsersControllerTests
    {
        public ChangePasswordTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> ChangePasswordTestData()
        {
            yield return new object[]
            {
                new ChangePasswordDto
                {
                    ActualPassword = "123456", NewPassword = "12345678"
                },
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                new ChangePasswordDto
                {
                    ActualPassword = "1234566", NewPassword = "12345678"
                },
                HttpStatusCode.Unauthorized
            };
        }

        [Theory]
        [MemberData(nameof(ChangePasswordTestData))]
        public async Task ChangePassword_ReturnsExpectedResult(ChangePasswordDto changePasswordDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PutAsJsonAsync($"/api/users/change-password", changePasswordDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class DeactivateUserTests : UsersControllerTests
    {
        public DeactivateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> DeactiveUserTestData()
        {
            yield return new object[]
            {
                new DeactivateUserDto
                {
                    UserId = Models.Users.InactiveTestUser.UserId
                },
                HttpStatusCode.BadRequest
            };

            yield return new object[]
            {
                new DeactivateUserDto
                {
                    UserId = Guid.NewGuid()
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(DeactiveUserTestData))]
        public async Task DeactiveUser_ReturnsExpectedResult(DeactivateUserDto deactivateUserDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PutAsJsonAsync("/api/users/deactivate", deactivateUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class CreateUserTests : UsersControllerTests
    {
        public CreateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> CreateUserTestData()
        {
            // Returns Ok
            yield return new object[]
            {
                new CreateUserDto
                {
                    UserUserName = "test-user-1",
                    UserFirstName = "Test",
                    UserLastName = "User",
                    UserEmail = "test-user-1@test.com",
                    UserPassword = "123456",
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the UserName
            yield return new object[]
            {
                new CreateUserDto
                {
                    UserUserName = Models.Users.Admin.UserUserName,
                    UserFirstName = "Admin",
                    UserLastName = "User",
                    UserEmail = "admin@admin.com",
                    UserPassword = "123456",
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };

            // Returns Conflict for the Email
            yield return new object[]
            {
                new CreateUserDto
                {
                    UserUserName = "test-user-11",
                    UserFirstName = "Test",
                    UserLastName = "User",
                    UserEmail = Models.Users.Admin.UserEmail,
                    UserPassword = "123456",
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateUserTestData))]
        public async Task CreateUser_ReturnsExpectedResult(CreateUserDto createUserDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync("/api/users/create", createUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class GetUserByIdTests : UsersControllerTests
    {
        public GetUserByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetUserTestData()
        {
            yield return new object[] { Guid.NewGuid(), HttpStatusCode.NotFound };
            yield return new object[] { Models.Users.Admin.UserId, HttpStatusCode.OK };
        }

        [Theory]
        [MemberData(nameof(GetUserTestData))]
        public async Task GetUserById_ReturnsExpectedResult(Guid userId, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.GetAsync($"/api/users/get-by-id?id={userId}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class GetUserByUsernameTests : UsersControllerTests
    {
        public GetUserByUsernameTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetUserByUsernameTestData()
        {
            yield return new object[] { Models.Users.Admin.UserUserName, HttpStatusCode.OK };
            yield return new object[] { "non-existent-username", HttpStatusCode.NotFound };
        }

        [Theory]
        [MemberData(nameof(GetUserByUsernameTestData))]
        public async Task GetUserByUsername_ReturnsExpectedResult(string username, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.GetAsync($"/api/users/get-by-username?username={username}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ExistUserBy : UsersControllerTests
    {
        public ExistUserBy(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> ExistUserByEmailTestData()
        {
            yield return new object[] { Models.Users.Admin.UserEmail, HttpStatusCode.OK, true };
            yield return new object[] { "non-existent-email@email.com", HttpStatusCode.OK, false };
        }

        [Theory]
        [MemberData(nameof(ExistUserByEmailTestData))]
        public async Task ExistUserByEmail_ReturnsExpectedResult(string email, HttpStatusCode expectedStatusCode,
            bool expectedResult)
        {
            var response = await _client.GetAsync($"/api/users/exist-user-by-email?email={email}");
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Content.ReadAsStringAsync()?.Result.ToLower().Should().Be(expectedResult.ToString().ToLower());
        }

        public static IEnumerable<object[]> ExistUserByUsernameTestData()
        {
            yield return new object[] { Models.Users.Admin.UserUserName, HttpStatusCode.OK, true };
            yield return new object[] { "non-existent-username", HttpStatusCode.OK, false };
        }

        [Theory]
        [MemberData(nameof(ExistUserByUsernameTestData))]
        public async Task ExistUserByUsername_ReturnsExpectedResult(string username, HttpStatusCode expectedStatusCode,
            bool expectedResult)
        {
            var response = await _client.GetAsync($"/api/users/exist-user-by-username?username={username}");
            response.StatusCode.Should().Be(expectedStatusCode);
            response.Content.ReadAsStringAsync()?.Result.ToLower().Should().Be(expectedResult.ToString().ToLower());
        }
    }
}