using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using FluentAssertions;

[Collection("Tests")]
public class CategoriesControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/categories";

    public CategoriesControllerTests(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }

    public class GetCategoryByTests : GetByTestsBase, IClassFixture<CustomWebApiFactory>
    {
        public GetCategoryByTests(CustomWebApiFactory factory) :
            base(factory.CreateClientWithBearerToken(), $"{Endpoint}/get-by", new DoubleBasicStructuresDto
            {
                DoubleBasicStructureDtos = new List<DoubleBasicStructureDto>
                {
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Categories.Technology.id.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }

    public class CreateCategoryTests : CategoriesControllerTests
    {
        public CreateCategoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> CreateCategoryTestData()
        {
            yield return new Object[] // All ok
            {
                new Category
                {
                    name = "Business",
                    description = "Business category"
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // CategoryName already exist
            {
                new Category
                {
                    name = Models.Categories.Technology.name,
                    description = "Technology category"
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateCategoryTestData))]
        public async Task CreateCategory_ReturnsExpectedResult(Category createCategoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createCategoryDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateCategoryTests : CategoriesControllerTests
    {
        public UpdateCategoryTests(CustomWebApiFactory factory) : base(factory) { }

        public static IEnumerable<object[]> UpdateCategoryTestData()
        {
            yield return new object[] // CategoryName already exist
            {
                Models.Categories.Technology.id,
                new 
                {
                    name = Models.Categories.Men.name,
                },
                HttpStatusCode.Conflict
            };

            yield return new object[] // All OK
            {
                Models.Categories.Technology.id,
                new 
                {
                    name = "Tecnologia",
                },
                HttpStatusCode.OK
            };

            yield return new object[] // Not found
            {
                Guid.NewGuid(),
                new 
                {
                    name = Models.Categories.Technology.name,
                    description = "Technology category"
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateCategoryTestData))]
        public async Task UpdateCategory_ReturnsExpectedResult(Guid categoryId, object data, HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update/{categoryId}", data);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
    
    public class DeleteCategoryTests : CategoriesControllerTests
    {
        public DeleteCategoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> DeleteCategoryTestData()
        {
            yield return new Object[] // All OK
            {
                Models.Categories.Technology.id,
                HttpStatusCode.OK
            };

            yield return new Object[] // Not found
            {
                Guid.NewGuid(),
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(DeleteCategoryTestData))]
        public async Task DeleteCategory_ReturnsExpectedResult(Guid categoryId,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.DeleteAsync($"{Endpoint}/delete?id={categoryId}");
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}