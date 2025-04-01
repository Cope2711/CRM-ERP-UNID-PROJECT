using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;

[Collection("Tests")]
public class UsersControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/users";

    public UsersControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetUserByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetUserByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.UserUserName,
                        FieldName = "username"  
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.UserId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    
    public class ExistUserByTests : ExistByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public ExistUserByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/exist-user-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.UserUserName,
                        FieldName = "username"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.UserEmail,
                        FieldName = "email"
                    }
                }
            })
        {
        }
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
            
            // When user is not in the same branch
            yield return new object[]
            {
                new UpdateUserDto
                {
                    UserId = Models.Users.TestUser2.UserId,
                    UserUserName = "test-updated"
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(UpdateUserTestData))]
        public async Task UpdateUser_ReturnsExpectedResult(UpdateUserDto updateUserDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateUserDto);
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
            var response = await _client.PutAsJsonAsync($"{Endpoint}/change-password", changePasswordDto);
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
                Ids = new List<Guid>
                {
                    Models.Users.InactiveTestUser.UserId, // Success
                    Models.Users.TestUser.UserId,
                    Guid.NewGuid(),
                    Models.Users.DeactivateHighestPriorityUser.UserId,
                    Models.Users.TestUser2.UserId
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/activate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserResponseStatusDto>>();
            
            // Assert
            activateUsersResponseDto.Should().NotBeNull();
            activateUsersResponseDto.Success.Count.Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
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
                Ids = new List<Guid>
                {
                    Models.Users.InactiveTestUser.UserId, 
                    Models.Users.TestUser.UserId, // Success
                    Guid.NewGuid(),
                    Models.Users.HighestPriorityUser.UserId,
                    Models.Users.TestUser2.UserId
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/deactivate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserResponseStatusDto>>();
            
            // Assert
            activateUsersResponseDto.Should().NotBeNull();
            activateUsersResponseDto.Success.Count.Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
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
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}