using System.Net;
using CRM_ERP_UNID_TESTS.Dtos;
using FluentAssertions;

namespace CRM_ERP_UNID_TESTS.TestsBase;

[Collection("Tests")]
public abstract class GetByTestsBase 
{
    private readonly HttpClient _client;
    private readonly string _endpoint;
    private readonly DoubleBasicStructuresDto _getByStructuresDto;
    
    public GetByTestsBase(HttpClient client, string endpoint, DoubleBasicStructuresDto getByStructuresDto)
    {
        _client = client;
        _getByStructuresDto = getByStructuresDto;
        _endpoint = endpoint;
    }

    [Fact]
    public async Task GetBy_ReturnsExpectedResult()
    {
        foreach (DoubleBasicStructureDto getByStructureDto in _getByStructuresDto.DoubleBasicStructureDtos)
        {
            List<DoubleDataTestDto> doubleDataTestDtos = new List<DoubleDataTestDto>
            {
                new DoubleDataTestDto
                {
                    Data = getByStructureDto.ValidValue,
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
                var response = await _client.GetAsync($"{_endpoint}-{getByStructureDto.FieldName}?{getByStructureDto.FieldName}={doubleDataTestDto.Data}");
                response.StatusCode.Should().Be(doubleDataTestDto.ExpectedStatusCode);
            }
        }
    }
}