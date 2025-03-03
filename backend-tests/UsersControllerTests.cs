﻿using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Constants;

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
            
            // When user has highest priority
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Models.Users.HighestPriorityUser.UserId,
                    UserEmail = "asdsa@gmail.com"
                },
                HttpStatusCode.Forbidden
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

    public class ActivateUserTests : UsersControllerTests
    {
        public ActivateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Activate_Test()
        {
            // Arrange
            UsersIdsDto usersIdsDto = new UsersIdsDto
            {
                UsersIds = new List<Guid>
                {
                    Models.Users.InactiveTestUser.UserId, // Success
                    Models.Users.TestUser.UserId,
                    Guid.NewGuid(),
                    Models.Users.DeactivateHighestPriorityUser.UserId
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync("/api/users/activate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserResponseStatusDto>>();
            
            // Assert
            activateUsersResponseDto.Should().NotBeNull();
            activateUsersResponseDto.Success.Count.Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
        }
    }
    
    public class DeactivateUserTests : UsersControllerTests
    {
        public DeactivateUserTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Deactivate_Test()
        {
            // Arrange
            UsersIdsDto usersIdsDto = new UsersIdsDto
            {
                UsersIds = new List<Guid>
                {
                    Models.Users.InactiveTestUser.UserId, 
                    Models.Users.TestUser.UserId, // Success
                    Guid.NewGuid(),
                    Models.Users.HighestPriorityUser.UserId
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync("/api/users/deactivate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserResponseStatusDto>>();
            
            // Assert
            activateUsersResponseDto.Should().NotBeNull();
            activateUsersResponseDto.Success.Count.Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
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