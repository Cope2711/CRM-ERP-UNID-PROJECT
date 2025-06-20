using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class BranchesTests
    : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/branches";

    public BranchesTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetBranchByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetBranchByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Branches.HermosilloMiguelHidalgo.name,
                        FieldName = "name"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Branches.HermosilloMiguelHidalgo.id.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class CreateBranchesTests : BranchesTests
    {
        public CreateBranchesTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> CreateBranchTestData()
        {
            yield return new Object[] // All ok
            {
                new Branch
                {
                    name = "Olivares de la Frontera",
                    address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    phone = "+526623296985",
                    isActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BranchName already exist
            {
                new Branch
                {
                    name = Models.Branches.HermosilloMiguelHidalgo.name,
                    address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    phone = "+526623296985",
                    isActive = false
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateBranchTestData))]
        public async Task CreateBranch_ReturnsExpectedResult(Branch createBrandDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createBrandDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateBranchTests : BranchesTests
    {
        public UpdateBranchTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> UpdateBranchTestData()
        {
            yield return new object[] // All OK
            {
                Models.Branches.HermosilloMiguelHidalgo.id,
                new
                {
                    name = "Olivares de la Frontera",
                    address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    phone = "+526623296985",
                },
                HttpStatusCode.OK
            };

            yield return new object[] // BranchName already exists
            {
                Models.Branches.HermosilloMiguelHidalgo.id,
                new 
                {
                    name = Models.Branches.CampoReal.name,
                    address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    phone = "+526623296985",
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Not found
            {
                Guid.NewGuid(),
                new 
                {
                    name = Models.Branches.CampoReal.name,
                    address = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    phone = "+526623296985",
                },
                HttpStatusCode.NotFound
            };

            yield return new object[] // Forbidden for the user branch
            {
                Models.Branches.CampoReal.id,
                new 
                {
                    name = "Hermosillo Miguel Hidalgo",
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(UpdateBranchTestData))]
        public async Task UpdateBranch_ReturnsExpectedResult(Guid branchId, Object data,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{branchId}", data);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ActivateUserTests : BranchesTests
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
                    Models.Branches.Obregon.id, // Success
                    Guid.NewGuid(), // Not found
                    Models.Branches.HermosilloMiguelHidalgo.id, // Already proccessed
                    Models.Branches.PuertoRico.id // Not access
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
            activateUsersResponseDto.Failed.Count(aur => aur.Status == ResponseStatus.BranchNotMatched).Should().Be(1);

        }
    }

    public class DeactivateUserTests : BranchesTests
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
                    Models.Branches.HermosilloMiguelHidalgo.id, // Success
                    Models.Branches.Obregon.id, // AlreadyProcessed
                    Guid.NewGuid(), // Not found
                    Models.Branches.PuertoRico.id
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
        }
    }
}