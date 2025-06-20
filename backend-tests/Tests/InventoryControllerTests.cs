using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID.Data.Models;
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

    public class GetByIdTests : InventoryControllerTests
    {
        public GetByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetByIdTestData()
        {
            yield return new object[]
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.id,
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(GetByIdTestData))]
        public async Task GetById_ReturnsExpectedResult(Guid id,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.GetAsync($"{Endpoint}/get-by-id?id={id}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class GetByProductIdInBranchIdTests : InventoryControllerTests
    {
        public GetByProductIdInBranchIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetByProductIdInBranchIdTestData()
        {
            yield return new object[]
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.productId,
                Models.InventoryModels.iPhone13InventoryHermosillo.branchId,
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.productId,
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                Models.InventoryModels.iPhone13InventoryHermosillo.branchId,
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(GetByProductIdInBranchIdTestData))]
        public async Task GetByProductIdInBranchId_ReturnsExpectedResult(Guid productId, Guid branchId,
            HttpStatusCode expectedStatusCode)
        {
            var response =
                await _client.GetAsync($"{Endpoint}/get-by-productId?productId={productId}&branchId={branchId}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateInventoryTests : InventoryControllerTests
    {
        public UpdateInventoryTests(CustomWebApiFactory factory) : base(factory) { }

        public static IEnumerable<object[]> UpdateInventoryTestData()
        {
            yield return new object[] // All ok
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.id,
                new
                {
                    quantity = 20,
                },
                HttpStatusCode.OK
            };

            yield return new object[] // Conflict for ProductId
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.id,
                new
                {
                    productId = Models.InventoryModels.GalaxyS21InventoryHermosillo.productId,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Forbidden for BranchId
            {
                Models.InventoryModels.iPadProInventoryHermosillo.id,
                new
                {
                    branchId = Models.InventoryModels.iPadProInventoryCampoReal.branchId,
                },
                HttpStatusCode.Forbidden
            };

            yield return new object[] // NotFound for InventoryId
            {
                Guid.NewGuid(),
                new
                {
                    quantity = 20,
                    isActive = true
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateInventoryTestData))]
        public async Task UpdateInventory_ReturnsExpectedResult(Guid inventoryId, object data, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{inventoryId}", data);
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
                new Inventory
                {
                    productId = Models.Products.NikeDriFitTShirt.id,
                    branchId = Models.Branches.HermosilloMiguelHidalgo.id,
                    quantity = 1,
                    isActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the ProductId
            yield return new object[]
            {
                new Inventory
                {
                    productId = Models.InventoryModels.iPhone13InventoryHermosillo.productId,
                    branchId = Models.InventoryModels.iPhone13InventoryHermosillo.branchId,
                    quantity = 5,
                    isActive = false
                },
                HttpStatusCode.Conflict
            };

            // Returns Conflict for the BranchID
            yield return new object[]
            {
                new Inventory
                {
                    productId = Models.Products.iPadPro.id,
                    branchId = Models.InventoryModels.iPadProInventoryHermosillo.branchId,
                    quantity = 10,
                    isActive = true
                },
                HttpStatusCode.Conflict
            };
            
            // Returns Forbidden for the user branch
            yield return new object[]
            {
                new Inventory
                {
                    productId = Models.Products.iPadPro.id,
                    branchId = Models.InventoryModels.iPadProInventoryCampoReal.branchId,
                    quantity = 10,
                    isActive = true
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(CreateInventoryTestData))]
        public async Task CreateInventory_ReturnsExpectedResult(Inventory createInventoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createInventoryDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}