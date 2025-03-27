using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class SuppliersControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/suppliers";

    public SuppliersControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetSupplierByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetSupplierByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Suppliers.Apple.SupplierId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class UpdateSupplierTests : SuppliersControllerTests
    {
        public UpdateSupplierTests(CustomWebApiFactory factory) : base(factory)
        {
        }
        public static IEnumerable<object[]> GetTestData()
        {                
            // All OK
            yield return new object[]
            {
                new UpdateSupplierDto
                {
                    SupplierId = Models.Suppliers.Apple.SupplierId,
                    SupplierName = "Appless",
                    SupplierContact = "Apples company",
                    SupplierEmail = "apples@gmail.com",
                    IsActive = true
                },
                HttpStatusCode.OK
            };
            
            // 404 NOT FOUND
            yield return new object[]
            {
                new UpdateSupplierDto
                {
                    SupplierId = Guid.NewGuid(),
                    SupplierName = "Appless",
                    SupplierContact = "Apples company",
                    SupplierEmail = "apples@gmail.com",
                    IsActive = true
                },
                HttpStatusCode.NotFound
            };
            
            // 409 FOR THE NAME
            yield return new object[]
            {
                new UpdateSupplierDto
                {
                    SupplierId = Models.Suppliers.Apple.SupplierId,
                    SupplierName = Models.Suppliers.Xataka.SupplierName,
                },
                HttpStatusCode.Conflict
            };
            
            // 409 FOR THE EMAIL
            yield return new object[]
            {
                new UpdateSupplierDto
                {
                    SupplierId = Models.Suppliers.Apple.SupplierId,
                    SupplierEmail = Models.Suppliers.Xataka.SupplierEmail,
                },
                HttpStatusCode.Conflict
            };
        }
        
        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task UpdateSupplier_ReturnsExpectedResult(UpdateSupplierDto updateSupplierDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateSupplierDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
    
    public class CreateSupplierTests : SuppliersControllerTests
    {
        public CreateSupplierTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> GetTestData()
        {
            // All Ok
            yield return new object[]
            {
                new CreateSupplierDto
                {
                    SupplierName = "Pepito",
                    SupplierContact = "PepitoContact",
                    SupplierEmail = "pepito314134@gmail.com",
                    IsActive = true
                },
                HttpStatusCode.OK
            };
            
            // 409 FOR THE NAME
            yield return new object[]
            {
                new CreateSupplierDto
                {
                    SupplierName = Models.Suppliers.Apple.SupplierName,
                    SupplierContact = "PepitoContact",
                    SupplierEmail = "pepito@gmail.com",
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
            
            // 409 FOR THE EMAIL
            yield return new object[]
            {
                new CreateSupplierDto
                {
                    SupplierName = "Pepito",
                    SupplierContact = "PepitoContact",
                    SupplierEmail = Models.Suppliers.Apple.SupplierEmail,
                    IsActive = true
                },
                HttpStatusCode.Conflict
            };
        }
        
        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task CreateSupplier_ReturnsExpectedResult(CreateSupplierDto createSupplierDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createSupplierDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}