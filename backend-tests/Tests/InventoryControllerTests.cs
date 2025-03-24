using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
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
        public UpdateInventoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> UpdateInventoryTestData()
        {
            yield return new object[] // All ok
            {
                new UpdateInventoryDto
                {
                    InventoryId = Models.InventoryModels.iPhone13InventoryHermosillo.InventoryId,
                    Quantity = 20,
                },
                HttpStatusCode.OK
            };

            // Returns Conflict for the ProductId
            yield return new object[]
            {
                new UpdateInventoryDto
                {
                    InventoryId = Models.InventoryModels.iPhone13InventoryHermosillo.InventoryId,
                    ProductId = Models.InventoryModels.GalaxyS21InventoryHermosillo.ProductId,
                },
                HttpStatusCode.Conflict
            };

            // Returns Forbidden for the user branch
            yield return new object[]
            {
                new UpdateInventoryDto
                {
                    InventoryId = Models.InventoryModels.iPadProInventoryHermosillo.InventoryId,
                    BranchId = Models.InventoryModels.iPadProInventoryCampoReal.BranchId,
                },
                HttpStatusCode.Forbidden
            };

            // Returns NotFound for the InventoryId
            yield return new object[]
            {
                new UpdateInventoryDto
                {
                    InventoryId = Guid.NewGuid(),
                    Quantity = 20,
                    IsActive = true
                },
                HttpStatusCode.NotFound
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