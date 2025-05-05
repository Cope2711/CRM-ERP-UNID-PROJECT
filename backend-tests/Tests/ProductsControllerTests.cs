using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class ProductsControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/products";

    public ProductsControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetProductByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetProductByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Products.iPhone13.ProductName,
                        FieldName = "name"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Products.iPhone13.ProductId.ToString(),
                        FieldName = "id"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Products.iPhone13.ProductBarcode.ToString(),
                        FieldName = "barcode"
                    }
                }
            })
        {
        }
    }

    public class CreateProductTests : ProductsControllerTests
    {
        public CreateProductTests(CustomWebApiFactory factory) : base(factory)
        {
        }


        public static IEnumerable<Object[]> CreateProductTestData()
        {
            yield return new Object[] // All ok
            {
                new CreateProductDto
                {
                    ProductName = "iPhonasdsadsa",
                    ProductPrice = 999.99m,
                    ProductBarcode = "1111111111",
                    ProductDescription = "Latest iPhone model",
                    IsActive = true,
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // ProductName already exist
            {
                new CreateProductDto
                {
                    ProductName = Models.Products.iPadPro.ProductName,
                    ProductPrice = 999.99m,
                    ProductBarcode = "111111111121312321",
                    ProductDescription = "Latest iPhone model",
                    IsActive = true,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // ProductBarcode already exist
            {
                new CreateProductDto
                {
                    ProductName = "ProductNameelpepe",
                    ProductPrice = 999.99m,
                    ProductBarcode = Models.Products.iPhone13.ProductBarcode,
                    ProductDescription = "Latest iPhone model",
                    IsActive = true,
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateProductTestData))]
        public async Task CreateProduct_ReturnsExpectedResult(CreateProductDto createProductDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createProductDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateProductTests : ProductsControllerTests
    {
        public UpdateProductTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> UpdateProductTestData()
        {
            yield return new object[] // All OK
            {
                Models.Products.iPhone13.ProductId,
                new UpdateProductDto
                {
                    ProductName = "Appless"
                },
                HttpStatusCode.OK
            };

            yield return new object[] // ProductName already exists
            {
                Models.Products.iPhone13.ProductId,
                new UpdateProductDto
                {
                    ProductName = Models.Products.iPadPro.ProductName,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // ProductBarcode already exists
            {
                Models.Products.iPhone13.ProductId,
                new UpdateProductDto
                {
                    ProductBarcode = Models.Products.iPadPro.ProductBarcode,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Not Found
            {
                Guid.NewGuid(),
                new UpdateProductDto
                {
                    ProductName = Models.Products.iPadPro.ProductName,
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateProductTestData))]
        public async Task UpdateProduct_ReturnsExpectedResult(Guid productId, UpdateProductDto updateDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{productId}", updateDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }


    public class ChangeBrandProductTests : ProductsControllerTests
    {
        public ChangeBrandProductTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> ChangeBrandProductTestData()
        {
            yield return new Object[] // All OK
            {
                new ChangeBrandProductDto
                {
                    ProductId = Models.Products.iPhone13.ProductId,
                    BrandId = Models.Brands.Samsung.BrandId
                },
                HttpStatusCode.OK
            };

            yield return new Object[]
            {
                new ChangeBrandProductDto
                {
                    ProductId = Models.Products.iPhone13.ProductId,
                    BrandId = Guid.NewGuid()
                },
                HttpStatusCode.NotFound
            };

            yield return new Object[]
            {
                new ChangeBrandProductDto
                {
                    ProductId = Guid.NewGuid(),
                    BrandId = Models.Brands.Samsung.BrandId
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(ChangeBrandProductTestData))]
        public async Task ChangeBrandProduct_ReturnsExpectedResult(ChangeBrandProductDto changeBrandProductDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/change-brand", changeBrandProductDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class ActivateUserTests : ProductsControllerTests
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
                    Models.Products.NikeDriFitTShirt.ProductId, // Success
                    Guid.NewGuid(), // Not found
                    Models.Products.iPadPro.ProductId // Already proccessed
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

    public class DeactivateUserTests : ProductsControllerTests
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
                    Models.Products.GalaxyTabS7.ProductId, // Success
                    Models.Products.NikeDriFitTShirt.ProductId, // AlreadyProcessed
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