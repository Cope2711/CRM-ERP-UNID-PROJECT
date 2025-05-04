using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;
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
                        ValidValue = Models.Branches.HermosilloMiguelHidalgo.BranchName,
                        FieldName = "name"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Branches.HermosilloMiguelHidalgo.BranchId.ToString(),
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
                new CreateBranchDto
                {
                    BranchName = "Olivares de la Frontera",
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "+526623296985",
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BranchName already exist
            {
                new CreateBranchDto
                {
                    BranchName = Models.Branches.HermosilloMiguelHidalgo.BranchName,
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "+526623296985",
                    IsActive = false
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateBranchTestData))]
        public async Task CreateBranch_ReturnsExpectedResult(CreateBranchDto createBrandDto,
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
                Models.Branches.HermosilloMiguelHidalgo.BranchId,
                new UpdateBranchDto
                {
                    BranchName = "Olivares de la Frontera",
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "+526623296985",
                },
                HttpStatusCode.OK
            };

            yield return new object[] // BranchName already exists
            {
                Models.Branches.HermosilloMiguelHidalgo.BranchId,
                new UpdateBranchDto
                {
                    BranchName = Models.Branches.CampoReal.BranchName,
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "+526623296985",
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Not found
            {
                Guid.NewGuid(),
                new UpdateBranchDto
                {
                    BranchName = Models.Branches.CampoReal.BranchName,
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "+526623296985",
                },
                HttpStatusCode.NotFound
            };

            yield return new object[] // Forbidden for the user branch
            {
                Models.Branches.CampoReal.BranchId,
                new UpdateBranchDto
                {
                    BranchName = "Hermosillo Miguel Hidalgo",
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(UpdateBranchTestData))]
        public async Task UpdateBranch_ReturnsExpectedResult(Guid branchId, UpdateBranchDto updateBranchDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{branchId}", updateBranchDto);
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
                    Models.Branches.Obregon.BranchId, // Success
                    Guid.NewGuid(), // Not found
                    Models.Branches.HermosilloMiguelHidalgo.BranchId, // Already proccessed
                    Models.Branches.PuertoRico.BranchId // Not access
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
                    Models.Branches.HermosilloMiguelHidalgo.BranchId, // Success
                    Models.Branches.Obregon.BranchId, // AlreadyProcessed
                    Guid.NewGuid(), // Not found
                    Models.Branches.PuertoRico.BranchId
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