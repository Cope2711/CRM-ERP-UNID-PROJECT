using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class InventoryControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/inventory";

    public InventoryControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }
    
    public class GetInventoryByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetInventoryByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.InventoryModels.iPhone13Inventory.ProductId.ToString(),
                        FieldName = "productId"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.InventoryModels.iPhone13Inventory.InventoryId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }
    
    public class UpdateInventoryTests : InventoryControllerTests
    {
        public UpdateInventoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> UpdateInventoryTestData()
        {
            yield return new object[] // All ok
            {
                new UpdateInventoryDto
                {
                    InventoryId = Models.InventoryModels.iPhone13Inventory.InventoryId,
                    ProductId = Models.InventoryModels.iPhone13Inventory.ProductId,
                    Quantity = 20,
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the ProductId
            yield return new object[]
            {
                new UpdateInventoryDto
                {
                    InventoryId = Models.InventoryModels.GalaxyS21Inventory.InventoryId,
                    ProductId = Models.Products.iPhone13.ProductId,
                    Quantity = 20,
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(UpdateInventoryTestData))]
        public async Task UpdateInventory_ReturnsExpectedResult(UpdateInventoryDto updateInventoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateInventoryDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

        public class CreateInventoryTests : InventoryControllerTests
    {
        public CreateInventoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> CreateInventoryTestData()
        {
            yield return new object[]
            {
                new CreateInventoryDto
                {
                    ProductId = Models.Products.NikeDriFitTShirt.ProductId,
                    Quantity = 10,
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the ProductId
            yield return new object[]
            {
                new CreateInventoryDto
                {
                    ProductId = Models.InventoryModels.iPhone13Inventory.ProductId,
                    Quantity = 10,
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateInventoryTestData))]
        public async Task CreateInventory_ReturnsExpectedResult(CreateInventoryDto createInventoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createInventoryDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}