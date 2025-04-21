using System.Net;
using System.Net.Http.Json;
using System.Text;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;
using Newtonsoft.Json;

[Collection("Tests")]
public class SuppliersBranchesControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/suppliers-branches";

    public SuppliersBranchesControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }
    
    public class UpdateSupplierBranchTests : SuppliersBranchesControllerTests
    {
        public UpdateSupplierBranchTests(CustomWebApiFactory factory) : base(factory)
        {
        }
        
        public static IEnumerable<object[]> GetTestData()
        {
            // All OK
            yield return new object[]
            {
                new UpdateSupplierBranchDto
                {
                    SupplierBranchId = Models.SuppliersBranches.AppleHermosilloMiguelHidalgo.SupplierBranchId,
                    IsPreferredSupplier = true
                },
                HttpStatusCode.OK
            };
            
            // 404 NOT FOUND
            yield return new object[]
            {
                new UpdateSupplierBranchDto
                {
                    SupplierBranchId = Guid.NewGuid(),
                    IsPreferredSupplier = true
                },
                HttpStatusCode.NotFound
            };
        }
        
        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task UpdateSupplierBranch_ReturnsExpectedResult(UpdateSupplierBranchDto updateSupplierBranchDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateSupplierBranchDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class RevokeBranchesTests : SuppliersBranchesControllerTests
    {
        public RevokeBranchesTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task RevokeBranches_ReturnsExpectedResult()
        {
            IdsDto idsDto = new IdsDto
            {
                Ids = new List<Guid>
                {
                    Models.SuppliersBranches.AppleHermosilloMiguelHidalgo.SupplierBranchId, // ALL OK
                    Guid.NewGuid(), // NOT FOUND
                    Models.SuppliersBranches.ApplePuertoRico.SupplierBranchId // Not branch assigned to user
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/revoke")
            {
                Content = new StringContent(JsonConvert.SerializeObject(idsDto), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            ResponsesDto<IdResponseStatusDto>? responseDto =
                await response.Content.ReadFromJsonAsync<ResponsesDto<IdResponseStatusDto>>();
            
            responseDto.Should().NotBeNull();
            responseDto.Success.Count.Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
        }
    }
    
    public class AssignBranchesTests : SuppliersBranchesControllerTests
    {
        public AssignBranchesTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AssignBranches_ReturnsExpectedResult()
        {
            ModelsAndAssignsDtos modelsAndAssignsDtos = new ModelsAndAssignsDtos
            {
                ModelAssignIds = new List<ModelAssignIdsDto>
                {
                    new ModelAssignIdsDto // Success
                    {
                        ModelId = Models.Suppliers.Xataka.SupplierId,
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    new ModelAssignIdsDto // Already assigned
                    {
                        ModelId = Models.SuppliersBranches.AppleHermosilloMiguelHidalgo.SupplierId,
                        AssignId = Models.SuppliersBranches.AppleHermosilloMiguelHidalgo.BranchId
                    },
                    
                    new ModelAssignIdsDto // Branch not assigned to user
                    {
                        ModelId = Models.Suppliers.Apple.SupplierId,
                        AssignId = Models.Branches.CampoReal.BranchId
                    },
                    
                    new ModelAssignIdsDto // Supplier not exist
                    {
                        ModelId = Guid.NewGuid(),
                        AssignId = Models.Branches.HermosilloMiguelHidalgo.BranchId
                    },
                    
                    new ModelAssignIdsDto // Branch not exist
                    {
                        ModelId = Models.Suppliers.Apple.SupplierId,
                        AssignId = Guid.NewGuid()
                    }
                }
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync($"{Endpoint}/assign", modelsAndAssignsDtos);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ResponsesDto<ModelAndAssignResponseStatusDto>? responseDto = await response.Content.ReadFromJsonAsync<ResponsesDto<ModelAndAssignResponseStatusDto>>();
            responseDto.Should().NotBeNull();
            responseDto.Success.Count.Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.AlreadyProcessed).Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.BranchNotMatched).Should().Be(1);
            responseDto.Failed.Count(x => x.Status == ResponseStatus.NotFound).Should().Be(2);
        }
    }
    
    public class GetByIdTests : SuppliersBranchesControllerTests
    {
        public GetByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetTestData()
        {
            // All OK
            yield return new object[]
            {
                Models.SuppliersBranches.AppleHermosilloMiguelHidalgo.SupplierBranchId,
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
}