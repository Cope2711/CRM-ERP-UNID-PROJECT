using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Constants;
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
            // OK
            yield return new object[]
            {
                Models.Suppliers.Apple.SupplierId,
                new UpdateSupplierDto
                {
                    SupplierName = "Appless",
                    SupplierContact = "Apples company",
                    SupplierEmail = "apples@gmail.com",
                },
                HttpStatusCode.OK
            };

            // 404 NOT FOUND
            yield return new object[]
            {
                Guid.NewGuid(),
                new UpdateSupplierDto
                {
                    SupplierName = "Appless",
                    SupplierContact = "Apples company",
                    SupplierEmail = "apples@gmail.com",
                },
                HttpStatusCode.NotFound
            };

            // 409 CONFLICT - NAME
            yield return new object[]
            {
                Models.Suppliers.Apple.SupplierId,
                new UpdateSupplierDto
                {
                    SupplierName = Models.Suppliers.Xataka.SupplierName
                },
                HttpStatusCode.Conflict
            };

            // 409 CONFLICT - EMAIL
            yield return new object[]
            {
                Models.Suppliers.Apple.SupplierId,
                new UpdateSupplierDto
                {
                    SupplierEmail = Models.Suppliers.Xataka.SupplierEmail
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public async Task UpdateSupplier_ReturnsExpectedResult(Guid supplierId, UpdateSupplierDto updateDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{supplierId}", updateDto);
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

    public class ActivateUserTests : SuppliersControllerTests
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
                    Models.Suppliers.CentelInactive.SupplierId, // Success
                    Guid.NewGuid(), // Not found
                    Models.Suppliers.Apple.SupplierId // Already proccessed
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

    public class DeactivateUserTests : SuppliersControllerTests
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
                    Models.Suppliers.Apple.SupplierId, // Success
                    Models.Suppliers.CentelInactive.SupplierId, // AlreadyProcessed
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