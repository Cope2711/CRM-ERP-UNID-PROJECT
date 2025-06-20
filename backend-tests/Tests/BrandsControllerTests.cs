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
                        ValidValue = Models.Brands.Apple.name,
                        FieldName = "name"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Brands.Apple.id.ToString(),
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
                new Brand
                {
                    name = "Apples",
                    description = "Apples company",
                    isActive = true
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // BrandName already exist
            {
                new Brand
                {
                    name = Models.Brands.Apple.name,
                    description = "Apples company",
                    isActive = false
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateBrandTestData))]
        public async Task CreateBrand_ReturnsExpectedResult(Brand createBrandDto,
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

        public static IEnumerable<object[]> UpdateBrandTestData()
        {
            yield return new object[] // All OK
            {
                Models.Brands.Nike.id,
                new
                {
                    name = "Appless",
                    description = "Apples company",
                },
                HttpStatusCode.OK
            };

            yield return new object[] // BrandName already exist
            {
                Models.Brands.Nike.id,
                new
                {
                    name = Models.Brands.Apple.name,
                    description = "Apples company",
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Not found
            {
                Guid.NewGuid(),
                new
                {
                    name = Models.Brands.Apple.name,
                    description = "Apples company",
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateBrandTestData))]
        public async Task UpdateBrand_ReturnsExpectedResult(Guid brandId, object data,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{brandId}", data);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ActivateUserTests : BrandsControllerTests
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
                    Models.Brands.Nike.id, // Success
                    Guid.NewGuid(), // Not found
                    Models.Brands.Apple.id // Already proccessed
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
        }
    }

    public class DeactivateUserTests : BrandsControllerTests
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
                    Models.Brands.Samsung.id, // Success
                    Models.Brands.Nike.id, // AlreadyProcessed
                    Guid.NewGuid() // Not found
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