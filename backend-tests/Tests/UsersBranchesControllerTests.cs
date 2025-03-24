
using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
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

    public class GetUserBranchByIdTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetUserBranchByIdTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.UsersBranches.AdminUserBranchHermosillo.UserBranchId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class AssignBranchTests : UsersBranchesControllerTests
    {
        public AssignBranchTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AssignBranch_ReturnsExpectedResult()
        {
            UsersAndBranchesDtos usersAndBranchesDtos = new UsersAndBranchesDtos
            {
                UserAndBranchIdDtos = new List<UserAndBranchIdDto>
                {
                    // Success - Assigned
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser2.UserId,
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },

                    // Already assigned
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser2.UserId,
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not found
                    new UserAndBranchIdDto
                    {
                        UserId = Guid.NewGuid(),
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not found branch
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser2.UserId,
                        BranchId = Guid.NewGuid()
                    },
                    
                    // Not enough priority
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.HighestPriorityUser2.UserId,
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not in the same branch
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser2.UserId,
                        BranchId = Models.Branches.CampoReal.BranchId
                    }
                }
            };
            
            var response = await _client.PostAsJsonAsync($"{Endpoint}/assign-branch", usersAndBranchesDtos);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserBranchResponseStatusDto>? assignBranchesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserBranchResponseStatusDto>>();

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
            UsersAndBranchesDtos usersAndBranchesDtos = new UsersAndBranchesDtos
            {
                UserAndBranchIdDtos = new List<UserAndBranchIdDto>
                {
                    // Success - Revoked
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser.UserId,
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not found
                    new UserAndBranchIdDto
                    {
                        UserId = Guid.NewGuid(),
                        BranchId = Models.Branches.CampoReal.BranchId
                    },
                    
                    // Not enough priority
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.HighestPriorityUser.UserId,
                        BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    // Not in the same branch
                    new UserAndBranchIdDto
                    {
                        UserId = Models.Users.TestUser2.UserId,
                        BranchId = Models.Branches.PuertoRico.BranchId
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke-branch")
            {
                Content = new StringContent(JsonConvert.SerializeObject(usersAndBranchesDtos), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);            
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponsesDto<UserBranchResponseStatusDto>? revokeBranchesResponseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<UserBranchResponseStatusDto>>();

            revokeBranchesResponseDto.Should().NotBeNull();
            revokeBranchesResponseDto.Success.Count.Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotFound).Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.NotEnoughPriority).Should().Be(1);
            revokeBranchesResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
        }
    }
}