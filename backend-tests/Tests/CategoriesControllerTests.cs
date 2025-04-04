using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;
using CRM_ERP_UNID_TESTS.TestsModels;
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
                        ValidValue = Models.Categories.Technology.CategoryId.ToString(),
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
                new CreateCategoryDto
                {
                    CategoryName = "Business",
                    CategoryDescription = "Business category"
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // CategoryName already exist
            {
                new CreateCategoryDto
                {
                    CategoryName = Models.Categories.Technology.CategoryName,
                    CategoryDescription = "Technology category"
                },
                HttpStatusCode.Conflict
            };
        }

        [Theory]
        [MemberData(nameof(CreateCategoryTestData))]
        public async Task CreateCategory_ReturnsExpectedResult(CreateCategoryDto createCategoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PostAsJsonAsync($"{Endpoint}/create", createCategoryDto);
            response.StatusCode.Should().Be(expectedStatusCode);
        }
    }

    public class UpdateCategoryTests : CategoriesControllerTests
    {
        public UpdateCategoryTests(CustomWebApiFactory factory) : base(factory)
        {
        }

        public static IEnumerable<Object[]> UpdateCategoryTestData()
        {
            yield return new Object[] // CategoryName already exist
            {
                new UpdateCategoryDto
                {
                    CategoryId = Models.Categories.Technology.CategoryId,
                    CategoryName = Models.Categories.Men.CategoryName,
                },
                HttpStatusCode.Conflict
            };
            
            yield return new Object[] // All OK
            {
                new UpdateCategoryDto
                {
                    CategoryId = Models.Categories.Technology.CategoryId,
                    CategoryName = "Tecnologia",
                },
                HttpStatusCode.OK
            };

            yield return new Object[] // Not found
            {
                new UpdateCategoryDto
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = Models.Categories.Technology.CategoryName,
                    CategoryDescription = "Technology category"
                },
                HttpStatusCode.NotFound
            };
        }

        [Theory]
        [MemberData(nameof(UpdateCategoryTestData))]
        public async Task UpdateCategory_ReturnsExpectedResult(UpdateCategoryDto updateCategoryDto,
            HttpStatusCode expectedStatusCode)
        {
            var response = await _client.PatchAsJsonAsync($"{Endpoint}/update", updateCategoryDto);
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
                Models.Categories.Technology.CategoryId,
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