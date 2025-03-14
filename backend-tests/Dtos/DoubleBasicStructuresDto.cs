using System.Net;

namespace CRM_ERP_UNID_TESTS.Dtos;

public class DoubleBasicStructuresDto
{
    public required List<DoubleBasicStructureDto> DoubleBasicStructureDtos { get; set; }
}

public class DoubleBasicStructureDto
{
    public required string ValidValue { get; set; }
    public required string FieldName { get; set; }
}

public class DoubleDataTestDto
{
    public required string Data { get; set; }
    public required HttpStatusCode ExpectedStatusCode { get; set; }
}
