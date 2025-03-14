using System.Net;
using System.Net.Http.Json;
using CRM_ERP_UNID_TESTS.Dtos;
using FluentAssertions;

namespace CRM_ERP_UNID_TESTS.TestsBase;

[Collection("Tests")]
public abstract class ExistByTestsBase
{
    private readonly HttpClient _client;
    private readonly string _endpoint;
    private readonly DoubleBasicStructuresDto _existByStructuresDto;
    
    public ExistByTestsBase(HttpClient client, string endpoint, DoubleBasicStructuresDto existByStructuresDto)
    {
        _client = client;
        _existByStructuresDto = existByStructuresDto;
        _endpoint = endpoint;
    }
    
    [Fact]
    public async Task ExistBy_ReturnsExpectedResult()
    {
        foreach (DoubleBasicStructureDto existByStructureDto in _existByStructuresDto.DoubleBasicStructureDtos)
        {
            List<DoubleDataTestDto> doubleDataTestDtos = new List<DoubleDataTestDto>
            {
                new DoubleDataTestDto
                {
                    Data = existByStructureDto.ValidValue,
                    ExpectedStatusCode = HttpStatusCode.OK
                },
                new DoubleDataTestDto
                {
                    Data = Guid.NewGuid().ToString(),
                    ExpectedStatusCode = HttpStatusCode.NotFound
                }
            };
            
            foreach (DoubleDataTestDto doubleDataTestDto in doubleDataTestDtos)
            {
                var response = await _client.GetAsync($"{_endpoint}-{existByStructureDto.FieldName}?{existByStructureDto.FieldName}={doubleDataTestDto.Data}");
                bool result = await response.Content.ReadFromJsonAsync<bool>();
                result.Should().Be(doubleDataTestDto.ExpectedStatusCode == HttpStatusCode.OK);
            }
        }
    }
}