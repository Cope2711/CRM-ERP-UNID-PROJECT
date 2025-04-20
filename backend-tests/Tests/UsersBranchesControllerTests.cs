using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using Newtonsoft.Json;

[Collection("Tests")]
public class UsersBranchesControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/users-branches";

    public UsersBranchesControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class AssignBranchTests : UsersBranchesControllerTests
    {
        public AssignBranchTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AssignBranch_ReturnsExpectedResult()
        {
            ModelsAndAssignsDtos usersAndBranchesDtos = new ModelsAndAssignsDtos
            {
                ModelAssignIds = new List<ModelAssignIdsDto>
                {
                    // Success - Assigned
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser2.UserId,
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },

                    // Already assigned
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser2.UserId,
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not found
                    new ModelAssignIdsDto
                    {
                        ModelId = Guid.NewGuid(),
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not found branch
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser2.UserId,
                        AssignId = Guid.NewGuid()
                    },
                    
                    // Not enough priority
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.HighestPriorityUser2.UserId,
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not in the same branch
                    new ModelAssignIdsDto
                    {
                        ModelId = Models.Users.TestUser2.UserId,
                        AssignId = Models.Branches.CampoReal.BranchId
                    }
                }
            };
            
            var response = await _client.PostAsJsonAsync($"{Endpoint}/assign", usersAndBranchesDtos);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<ModelAndAssignResponseStatusDto>? assignBranchesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<ModelAndAssignResponseStatusDto>>();

            assignBranchesResponseDto.Should().NotBeNull();
            assignBranchesResponseDto.Success.Count.Should().Be(1);
            assignBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            assignBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(2);
            assignBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            assignBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
        }
    }
    
    public class RevokeBranchTests : UsersBranchesControllerTests
    {
        public RevokeBranchTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task RevokeBranch_ReturnsExpectedResult()
        {
            IdsDto usersBranchesIdsDto = new IdsDto{
                Ids = new List<Guid>
                {
                    Models.UsersBranches.TestUserBranchHermosillo.UserBranchId, // Success - Revoked
                    Guid.NewGuid(), // Not found
                    Models.UsersBranches.HighestPriorityUserBranchHermosillo.UserBranchId, // Not enough priority
                    Models.UsersBranches.TestUser2BranchPuertoRico.UserBranchId // Not in the same branch
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke")
            {
                Content = new StringContent(JsonConvert.SerializeObject(usersBranchesIdsDto), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);            
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<IdResponseStatusDto>? revokeBranchesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();

            revokeBranchesResponseDto.Should().NotBeNull();
            revokeBranchesResponseDto.Success.Count.Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
        }
    }
}