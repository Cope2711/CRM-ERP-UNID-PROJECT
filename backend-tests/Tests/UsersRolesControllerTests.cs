using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS.TestsModels;
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
            ModelsAndAssignsDtos modelsAndAssignsDtos = new ModelsAndAssignsDtos
            {
                ModelAssignIds = new List<ModelAssignIdsDto>
                {
                    // Success - Assigned
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser.id,
                        AssignId = Models.Roles.Guest.id
                    },
                    
                    // Already assigned
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser.id,
                        AssignId = Models.Roles.User.id
                    },
                    
                    // Not found
                    new ModelAssignIdsDto
                    {
                        ModelId = Guid.NewGuid(),
                        AssignId = Models.Roles.User.id
                    },
                    
                    // Not enough priority
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.HighestPriorityUser.id,
                        AssignId = Models.Roles.User.id
                    },
                    
                    // Not enough priority
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.InactiveTestUser.id,
                        AssignId = Models.Roles.HighestPriority.id
                    },
                    
                    // Not the same branch
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser2.id,
                        AssignId = Models.Roles.Guest.id
                    },
                }
            };

            var response = await _client.PostAsJsonAsync($"{Endpoint}/assign", modelsAndAssignsDtos);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<ModelAndAssignResponseStatusDto>? assignRolesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<ModelAndAssignResponseStatusDto>>();

            assignRolesResponseDto.Should().NotBeNull();
            assignRolesResponseDto.Success.Count.Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(2);
            assignRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
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
            IdsDto idsDto = new IdsDto
            {
                Ids = new List<Guid>
                {
                    Models.UsersRoles.TestUserRoleUser.id, // Success - Revoked
                    Guid.NewGuid(), // Not found
                    Models.UsersRoles.HighestPriorityUserRoleHighestPriority.id, // Not enough priority
                    Models.UsersRoles.TestUser2RoleUser.id, // Not the same branch
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke")
            {
                Content = new StringContent(JsonConvert.SerializeObject(idsDto), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);            
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<IdResponseStatusDto>? revokeRolesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();

            revokeRolesResponseDto.Should().NotBeNull();
            revokeRolesResponseDto.Success.Count.Should().Be(1);
            revokeRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            revokeRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            revokeRolesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
        }
    }
}