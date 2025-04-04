using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class ProductsCategoriesControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/products-categories";

    public ProductsCategoriesControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class AssignProductsCategoriesTests : ProductsCategoriesControllerTests
    {
        public AssignProductsCategoriesTests(CustomWebApiFactory factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task AssignProductsCategories_ReturnsExpectedResult()
        {
            ProductsAndCategoriesDto productsAndCategoriesDto = new()
            {
                ProductAndCategoryIdDto = new List<ProductAndCategoryIdDto>
                {
                    new ProductAndCategoryIdDto // OK
                    {
                        ProductId = Models.Products.GalaxyS21.ProductId,
                        CategoryId = Models.Categories.Technology.CategoryId
                    },
                    
                    new ProductAndCategoryIdDto // Not found
                    {
                        ProductId = Guid.NewGuid(),
                        CategoryId = Models.Categories.Technology.CategoryId
                    },
                    
                    new ProductAndCategoryIdDto // Not found
                    {
                        ProductId = Models.Products.GalaxyS21.ProductId,
                        CategoryId = Guid.NewGuid()
                    },
                    
                    new ProductAndCategoryIdDto // Already assigned
                    {
                        ProductId = Models.Products.iPhone13.ProductId,
                        CategoryId = Models.Categories.Technology.CategoryId
                    }
                }
            };
            
            var response = await _client.PostAsJsonAsync($"{Endpoint}/assign", productsAndCategoriesDto);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ResponsesDto<ProductAndCategoryResponseStatusDto> responsesDto = await response.Content.ReadFromJsonAsync<ResponsesDto<ProductAndCategoryResponseStatusDto>>();
            responsesDto.Should().NotBeNull();
            responsesDto.Success.Should().HaveCount(1);
            responsesDto.Failed.Count(x => x.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            responsesDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(2);
        }
    }
    
    public class RevokeProductsCategoriesTests : ProductsCategoriesControllerTests
    {
        public RevokeProductsCategoriesTests(CustomWebApiFactory factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task RevokeProductsCategories_ReturnsExpectedResult()
        {
            IdsDto idsDto = new()
            {
                Ids = new List<Guid>
                {
                    Models.ProductsCategories.iPhone13Tecnology.ProductCategoryId,
                    Guid.NewGuid()
                }
            };
            
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke")
            {
                Content = new StringContent(JsonSerializer.Serialize(idsDto), Encoding.UTF8, "application/json")
            };
            
            var response = await _client.SendAsync(request);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ResponsesDto<IdResponseStatusDto> responsesDto = await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();
            responsesDto.Should().NotBeNull();
            responsesDto.Success.Should().HaveCount(1);
            responsesDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(1);
        }
    }
}