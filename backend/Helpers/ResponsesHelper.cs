using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Helpers;

public static class ResponsesHelper
{
    public static void AddFailedResponseDto(ResponsesDto<IdResponseStatusDto> responseDto,
        Guid id, string status, string field, string message)
    {
        responseDto.Failed.Add(new IdResponseStatusDto
        {
            Id = id,
            Status = status,
            Field = field,
            Message = message
        });
    }
}