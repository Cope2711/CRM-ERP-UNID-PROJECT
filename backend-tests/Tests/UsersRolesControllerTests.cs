using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using Newtonsoft.Json;

[Collection("Tests")]
public class UsersRolesControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/users-roles";

    public UsersRolesControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class AssignRolesTests : UsersRolesControllerTests
    {
        public AssignRolesTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AssignRoles_ReturnsExpectedResult()
        {
            UsersAndRolesDtos usersAndRolesDtos = new UsersAndRolesDtos
            {
                UserAndRoleId = new List<UserAndRoleIdDto>
                {
                    // Success - Assigned
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.TestUser.UserId,
                        RoleId = Models.Roles.Guest.RoleId
                    },
                    
                    // Already assigned
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.TestUser.UserId,
                        RoleId = Models.Roles.User.RoleId
                    },
                    
                    // Not found
                    new UserAndRoleIdDto
                    {
                        UserId = Guid.NewGuid(),
                        RoleId = Models.Roles.User.RoleId
                    },
                    
                    // Not enough priority
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.HighestPriorityUser.UserId,
                        RoleId = Models.Roles.User.RoleId
                    },
                    
                    // Not enough priority
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.InactiveTestUser.UserId,
                        RoleId = Models.Roles.HighestPriority.RoleId
                    },
                }
            };

            var response = await _client.PostAsJsonAsync($"{Endpoint}/assign-roles", usersAndRolesDtos);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserAndRoleResponseStatusDto>? assignRolesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserAndRoleResponseStatusDto>>();

            assignRolesResponseDto.Should().NotBeNull();
            assignRolesResponseDto.Success.Count.Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(2);
        }
    }
    
    public class RevokeRolesTests : UsersRolesControllerTests
    {
        public RevokeRolesTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task RevokeRoles_ReturnsExpectedResult()
        {
            UsersAndRolesDtos usersAndRolesDtos = new UsersAndRolesDtos
            {
                UserAndRoleId = new List<UserAndRoleIdDto>
                {
                    // Success - Revoked
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.TestUser.UserId,
                        RoleId = Models.Roles.User.RoleId
                    },
                    
                    // Not found
                    new UserAndRoleIdDto
                    {
                        UserId = Guid.NewGuid(),
                        RoleId = Models.Roles.User.RoleId
                    },
                    
                    // Not enough priority
                    new UserAndRoleIdDto
                    {
                        UserId = Models.Users.HighestPriorityUser.UserId,
                        RoleId = Models.Roles.HighestPriority.RoleId
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke-roles")
            {
                Content = new StringContent(JsonConvert.SerializeObject(usersAndRolesDtos), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);            
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserAndRoleResponseStatusDto>? revokeRolesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserAndRoleResponseStatusDto>>();

            revokeRolesResponseDto.Should().NotBeNull();
            revokeRolesResponseDto.Success.Count.Should().Be(1);
            revokeRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            revokeRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
        }
    }
}