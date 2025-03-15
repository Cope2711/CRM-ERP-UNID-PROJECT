
using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class BrandsControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/brands";

    public BrandsControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetBrandByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetBrandByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Brands.Apple.BrandName,
                        FieldName = "name"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Brands.Apple.BrandId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class CreateBrandTests : BrandsControllerTests
    {
        public CreateBrandTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> CreateBrandTestData()
        {
            yield return new Object[] // All ok
            {
                new CreateBrandDto
                {
                    BrandName = "Apples",
                    BrandDescription = "Apples company",
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BrandName already exist
            {
                new CreateBrandDto
                {
                    BrandName = Models.Brands.Apple.BrandName,
                    BrandDescription = "Apples company",
                    IsActive = false
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateBrandTestData))]
        public async Task CreateBrand_ReturnsExpectedResult(CreateBrandDto createBrandDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createBrandDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateBrandTests : BrandsControllerTests
    {
        public UpdateBrandTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> UpdateBrandTestData()
        {
            yield return new Object[] // All OK
            {
                new UpdateBrandDto
                {
                    BrandId = Models.Brands.Nike.BrandId,
                    BrandName = "Appless",
                    BrandDescription = "Apples company",
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BrandName already exist
            {
                new UpdateBrandDto
                {
                    BrandId = Models.Brands.Nike.BrandId,
                    BrandName = Models.Brands.Apple.BrandName,
                    BrandDescription = "Apples company",
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(UpdateBrandTestData))]
        public async Task UpdateBrand_ReturnsExpectedResult(UpdateBrandDto updateBrandDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateBrandDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}