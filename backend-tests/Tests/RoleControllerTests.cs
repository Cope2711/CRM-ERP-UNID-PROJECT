using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class RoleControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/roles";

    public RoleControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Roles.Admin.RoleName,
                        FieldName = "rolename"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Roles.Admin.RoleId.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class UpdateRoleTests : RoleControllerTests
    {
        public UpdateRoleTests(CustomWebApiFactory factory) : base(factory) { }

        public static IEnumerable<object[]> UpdateRoleTestData()
        {
            // Success
            yield return new object[]
            {
                Models.Roles.User.RoleId,
                new UpdateRoleDto
                {
                    RoleName = "Adminmsamdksad",
                    RoleDescription = "Admin role"
                },
                HttpStatusCode.OK
            };

            // Conflict - Same RoleName as Admin
            yield return new object[]
            {
                Models.Roles.User.RoleId,
                new UpdateRoleDto
                {
                    RoleName = Models.Roles.Admin.RoleName,
                    RoleDescription = "Admin role"
                },
                HttpStatusCode.Conflict
            };

            // Forbidden - Trying to update a protected role
            yield return new object[]
            {
                Models.Roles.Admin.RoleId,
                new UpdateRoleDto
                {
                    RoleName = "asdsadsadsa",
                    RoleDescription = "Admin role"
                },
                HttpStatusCode.Forbidden
            };

            // NotFound - Role doesn't exist
            yield return new object[]
            {
                Guid.NewGuid(),
                new UpdateRoleDto
                {
                    RoleName = "asdsadsadsa",
                    RoleDescription = "Admin role"
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateRoleTestData))]
        public async Task UpdateRole_ReturnsExpectedResult(Guid roleId, UpdateRoleDto updateDto, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{roleId}", updateDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    
    public class CreateRoleTests : RoleControllerTests
    {
        public CreateRoleTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> CreateRoleTestData()
        {
            yield return new object[]
            {
                // Success
                new CreateRoleDto
                {
                    RoleName = "Adminmsamdksad",
                    RoleDescription = "Admin role",
                    RolePriority = 1f
                },
                HttpStatusCode.Created
            };

            yield return new object[]
            {
                new CreateRoleDto
                {
                    RoleName = "Admin",
                    RoleDescription = "Admin role",
                    RolePriority = 1f
                },
                HttpStatusCode.Conflict
            };

            yield return new object[]
            {
                new CreateRoleDto
                {
                    RoleName = "Adminmsamdksad",
                    RoleDescription = "Admin role",
                    RolePriority = 100f
                },
                HttpStatusCode.Forbidden
            };
        }

        [Theory]
        [MemberData(nameof(CreateRoleTestData))]
        public async Task CreateRole_ReturnsExpectedResult(CreateRoleDto createRoleDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createRoleDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
    
    public class DeleteByIdTests : RoleControllerTests
    {
        public DeleteByIdTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<object[]> DeleteByIdTestData()
        {
            yield return new object[]
            {
                // Success
                Models.Roles.User.RoleId,
                HttpStatusCode.OK
            };
            
            yield return new object[]
            {
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };
            
            yield return new object[]
            {
                // Success
                Models.Roles.HighestPriority.RoleId,
                HttpStatusCode.Forbidden
            };
        }
        
        [Theory]
        [MemberData(nameof(DeleteByIdTestData))]
        public async Task DeleteById_ReturnsExpectedResult(Guid id, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.DeleteAsync($"{Endpoint}/delete-by-id?id={id}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}