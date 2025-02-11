namespace CRM_ERP_UNID_TESTS;

public class UnitTest1
{
    private readonly HttpClient _client;
    
    public UnitTest1(CustomWebApiFactory factory)
    {
        _client = factory.CreateClientWithBearerToken();
    }
    
    [Fact]
    public void Test1()
    {
    }
}