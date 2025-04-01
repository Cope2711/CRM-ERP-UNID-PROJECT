using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
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
                    ProductName = "iPhone 133333",
                    ProductPrice = 999.99m,
                    ProductDescription = "Latest iPhone model",
                    IsActive = true,
                    BrandId = Models.Brands.Apple.BrandId
                },
                HttpStatusCode.OK
            };
            
            yield return new Object[] // ProductName already exist
            {
                new CreateProductDto
                {
                    ProductName = Models.Products.iPadPro.ProductName,
                    ProductPrice = 999.99m,
                    ProductDescription = "Latest iPhone model",
                    IsActive = true,
                    BrandId = Models.Brands.Apple.BrandId
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

        public static IEnumerable<Object[]> UpdateProductTestData()
        {
            yield return new Object[] // All OK
            {
                new UpdateProductDto
                {
                    ProductId = Models.Products.iPhone13.ProductId,
                    ProductName = "Appless"
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // ProductName already exist
            {
                new UpdateProductDto
                {
                    ProductId = Models.Products.iPhone13.ProductId,
                    ProductName = Models.Products.iPadPro.ProductName,
                },
                HttpStatusCode.Conflict
            };
            
            yield return new Object[] // Not found
            {
                new UpdateProductDto
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = Models.Products.iPadPro.ProductName,
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateProductTestData))]
        public async Task UpdateProduct_ReturnsExpectedResult(UpdateProductDto updateProductDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateProductDto);
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
}