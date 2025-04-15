using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
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
                Models.InventoryModels.iPhone13InventoryHermosillo.InventoryId,
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
                Models.InventoryModels.iPhone13InventoryHermosillo.ProductId,
                Models.InventoryModels.iPhone13InventoryHermosillo.BranchId,
                HttpStatusCode.OK
            };

            yield return new object[]
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.ProductId,
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };

            yield return new object[]
            {
                Guid.NewGuid(),
                Models.InventoryModels.iPhone13InventoryHermosillo.BranchId,
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
                Models.InventoryModels.iPhone13InventoryHermosillo.InventoryId,
                new UpdateInventoryDto
                {
                    Quantity = 20,
                },
                HttpStatusCode.OK
            };

            yield return new object[] // Conflict for ProductId
            {
                Models.InventoryModels.iPhone13InventoryHermosillo.InventoryId,
                new UpdateInventoryDto
                {
                    ProductId = Models.InventoryModels.GalaxyS21InventoryHermosillo.ProductId,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // Forbidden for BranchId
            {
                Models.InventoryModels.iPadProInventoryHermosillo.InventoryId,
                new UpdateInventoryDto
                {
                    BranchId = Models.InventoryModels.iPadProInventoryCampoReal.BranchId,
                },
                HttpStatusCode.Forbidden
            };

            yield return new object[] // NotFound for InventoryId
            {
                Guid.NewGuid(),
                new UpdateInventoryDto
                {
                    Quantity = 20,
                    IsActive = true
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateInventoryTestData))]
        public async Task UpdateInventory_ReturnsExpectedResult(Guid inventoryId, UpdateInventoryDto dto, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{inventoryId}", dto);
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
                    BranchId = Models.Branches.HermosilloMiguelHidalgo.BranchId,
                    Quantity = 1,
                    IsActive = true
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the ProductId
            yield return new object[]
            {
                new CreateInventoryDto
                {
                    ProductId = Models.InventoryModels.iPhone13InventoryHermosillo.ProductId,
                    BranchId = Models.InventoryModels.iPhone13InventoryHermosillo.BranchId,
                    Quantity = 5,
                    IsActive = false
                },
                HttpStatusCode.Conflict
            };

            // Returns Conflict for the BranchID
            yield return new object[]
            {
                new CreateInventoryDto
                {
                    ProductId = Models.Products.iPadPro.ProductId,
                    BranchId = Models.InventoryModels.iPadProInventoryHermosillo.BranchId,
                    Quantity = 10,
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
            
            // Returns Forbidden for the user branch
            yield return new object[]
            {
                new CreateInventoryDto
                {
                    ProductId = Models.Products.iPadPro.ProductId,
                    BranchId = Models.InventoryModels.iPadProInventoryCampoReal.BranchId,
                    Quantity = 10,
                    IsActive = true
                },
                HttpStatusCode.Forbidden
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