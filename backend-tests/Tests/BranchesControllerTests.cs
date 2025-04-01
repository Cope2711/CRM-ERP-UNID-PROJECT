
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
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
                    BranchPhone = "666666666",
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
                    BranchPhone = "666666666",
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

        public static IEnumerable<Object[]> UpdateBranchTestData()
        {
            yield return new Object[] // All OK
            {
                new UpdateBranchDto
                {
                    BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId,
                    BranchName = "Olivares de la Frontera",
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "666666666",
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BranchName already exist
            {
                new UpdateBranchDto
                {
                    BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId,
                    BranchName = Models.Branches.CampoReal.BranchName,
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "666666666",
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
            
            yield return new Object[] // Not found
            {
                new UpdateBranchDto
                {
                    BranchId = Guid.NewGuid(),
                    BranchName = Models.Branches.CampoReal.BranchName,
                    BranchAddress = "Calle 123 Nº 1, Hermosillo, Sonora, Mexico",
                    BranchPhone = "666666666",
                    IsActive = true
                },
                HttpStatusCode.NotFound
            };
            
            yield return new object[] // Forbidden for the user branch
            {
                new UpdateBranchDto
                {
                    BranchId = Models.Branches.CampoReal.BranchId,
                    BranchName = "Hermosillo Miguel Hidalgo",
                    IsActive = true
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(UpdateBranchTestData))]
        public async Task UpdateBranch_ReturnsExpectedResult(UpdateBranchDto updateBranchDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateBranchDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}