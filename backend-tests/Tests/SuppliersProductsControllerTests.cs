using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using Newtonsoft.Json;

[Collection("Tests")]
public class SuppliersProductsControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/suppliers-products";

    public SuppliersProductsControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetByIdTests : SuppliersProductsControllerTests
    {
        public GetByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetTestData()
        {
            // All OK
            yield return new object[]
            {
                Models.SuppliersProducts.AppleIphone13.SupplierProductId,
                HttpStatusCode.OK
            };

            // 404 NOT FOUND
            yield return new object[]
            {
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task GetById_ReturnsExpectedResult(Guid id,
            HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Endpoint}/get-by-id?id={id}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class AssignProductsTests : SuppliersProductsControllerTests
    {
        public AssignProductsTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AssignProducts_ReturnsExpectedResult()
        {
            SuppliersAndProductsIdsDto suppliersAndProductsIdsDto = new SuppliersAndProductsIdsDto
            {
                SuppliersAndProductsIds = new List<SupplierAndProductIdDto>
                {
                    new SupplierAndProductIdDto // ALL OK
                    {
                        SupplierId = Models.Suppliers.Apple.SupplierId,
                        ProductId = Models.Products.iPadPro.ProductId
                    },
                    
                    new SupplierAndProductIdDto // ALREADY PROCESSED
                    {
                        SupplierId = Models.Suppliers.Apple.SupplierId,
                        ProductId = Models.Products.iPhone13.ProductId
                    },
                    
                    new SupplierAndProductIdDto // NOT FOUND FOR SUPPLIER
                    {
                        SupplierId = Guid.NewGuid(),
                        ProductId = Models.Products.iPhone13.ProductId
                    },
                    
                    new SupplierAndProductIdDto // NOT FOUND FOR PRODUCT
                    {
                        SupplierId = Models.Suppliers.Apple.SupplierId,
                        ProductId = Guid.NewGuid()
                    }
                }
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync($"{Endpoint}/assign-products", suppliersAndProductsIdsDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ResponsesDto<SupplierAndProductResponseStatusDto> responseDto = await response.Content.ReadFromJsonAsync<ResponsesDto<SupplierAndProductResponseStatusDto>>();
            responseDto.Should().NotBeNull();
            responseDto.Success.Count.Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(2);
        }
    }
    
    public class RevokeProductsTests : SuppliersProductsControllerTests
    {
        public RevokeProductsTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task RevokeProducts_ReturnsExpectedResult()
        {
            IdsDto suppliersProductsIdsDto = new IdsDto{
                Ids = new List<Guid>
                {
                    Models.SuppliersProducts.AppleIphone13.SupplierProductId, // ALL OK
                    Guid.NewGuid() // NOT FOUND
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke-products")
            {
                Content = new StringContent(JsonConvert.SerializeObject(suppliersProductsIdsDto), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            ResponsesDto<IdResponseStatusDto>? responseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();
            responseDto.Should().NotBeNull();
            responseDto.Success.Count.Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(1);
        }
    }
    
    public class UpdateSupplierProductTests : SuppliersProductsControllerTests
    {
        public UpdateSupplierProductTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetTestData()
        {
            // All OK
            yield return new object[]
            {
                new UpdateSupplierProductDto
                {
                    SupplierProductId = Models.SuppliersProducts.AppleIphone13.SupplierProductId,
                    SupplyPrice = 999.99m,
                    SupplyLeadTime = 10
                },
                HttpStatusCode.OK
            };
            
            // 404 NOT FOUND
            yield return new object[]
            {
                new UpdateSupplierProductDto
                {
                    SupplierProductId = Guid.NewGuid(),
                    SupplyPrice = 999.99m,
                    SupplyLeadTime = 10
                },
                HttpStatusCode.NotFound
            };
        }
        
        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task UpdateSupplierProduct_ReturnsExpectedResult(UpdateSupplierProductDto updateSupplierProductDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateSupplierProductDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}