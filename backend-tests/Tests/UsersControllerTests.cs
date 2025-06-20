using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;

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
                        ValidValue = Models.Users.Admin.userName,
                        FieldName = "username"  
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.id.ToString(),
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
                        ValidValue = Models.Users.Admin.userName,
                        FieldName = "username"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Users.Admin.email,
                        FieldName = "email"
                    }
                }
            })
        {
        }
    }
    
    public class UpdateUserTests : UsersControllerTests
    {
        public UpdateUserTests(CustomWebApiFactory factory) : base(factory) { }

        public static IEnumerable<object[]> UpdateUserTestData()
        {
            yield return new object[]
            {
                Models.Users.TestUser.id,
                new 
                {
                    userName = "test-updated"
                },
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                new 
                {
                    userName = "TestUserr"
                },
                HttpStatusCode.NotFound
            };

            yield return new object[]
            {
                Models.Users.TestUser.id,
                new 
                {
                    userName = "admin"
                },
                HttpStatusCode.Conflict
            };

            yield return new object[]
            {
                Models.Users.TestUser.id,
                new 
                {
                    email = "admin@admin.com"
                },
                HttpStatusCode.Conflict
            };

            yield return new object[]
            {
                Models.Users.HighestPriorityUser.id,
                new 
                {
                    email = "asdsa@gmail.com"
                },
                HttpStatusCode.Forbidden
            };

            yield return new object[]
            {
                Models.Users.TestUser2.id,
                new 
                {
                    userName = "test-updated"
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(UpdateUserTestData))]
        public async Task UpdateUser_ReturnsExpectedResult(Guid userId, object data, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{userId}", data);
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
            IdsDto usersIdsDto = new IdsDto
            {
                Ids = new List<Guid>
                {
                    Models.Users.InactiveTestUser.id, // Success
                    Models.Users.TestUser.id,
                    Guid.NewGuid(),
                    Models.Users.DeactivateHighestPriorityUser.id,
                    Models.Users.TestUser2.id
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/activate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<IdResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();
            
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
            IdsDto usersIdsDto = new IdsDto
            {
                Ids = new List<Guid>
                {
                    Models.Users.InactiveTestUser.id, 
                    Models.Users.TestUser.id, // Success
                    Guid.NewGuid(),
                    Models.Users.HighestPriorityUser.id,
                    Models.Users.TestUser2.id
                }
            };
            
            // Act
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/deactivate", usersIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<IdResponseStatusDto>? activateUsersResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();
            
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
                new User
                {
                    userName = "test-user-1",
                    firstName = "Test",
                    lastName = "User",
                    email = "test-user-1@test.com",
                    password = "123456",
                    isActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the UserName
            yield return new object[]
            {
                new User
                {
                    userName = Models.Users.Admin.userName,
                    firstName = "Admin",
                    lastName = "User",
                    email = "admin@admin.com",
                    password = "123456",
                    isActive = true
                },
                HttpStatusCode.Conflict
            };

            // Returns Conflict for the Email
            yield return new object[]
            {
                new User
                {
                    userName = "test-user-11",
                    firstName = "Test",
                    lastName = "User",
                    email = Models.Users.Admin.email,
                    password = "123456",
                    isActive = true
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateUserTestData))]
        public async Task CreateUser_ReturnsExpectedResult(User createUserDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createUserDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}