using CRM_ERP_UNID_TESTS.TestsModels;
using CRM_ERP_UNID_TESTS.Dtos;
using CRM_ERP_UNID_TESTS.TestsBase;

[Collection("Tests")]
public class ResourceControllerTests : IClassFixture<CustomWebApiFactory>
{
    private readonly HttpClient _client;
    private static readonly string Endpoint = "/api/resources";

    public ResourceControllerTests(CustomWebApiFactory factory)
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
                        ValidValue = Models.Resources.Users.name,
                        FieldName = "resourcename"
                    },
                    new DoubleBasicStructureDto
                    {
                        ValidValue = Models.Resources.Users.id.ToString(),
                        FieldName = "id"
                    }
                }
            })
        {
        }
    }
}