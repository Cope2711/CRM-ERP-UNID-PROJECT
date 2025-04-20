using CRM_ERP_UNID.Attributes;

namespace CRM_ERP_UNID.Dtos;

public class UserBranchDto
{
    public Guid UserBranchId { get; set; }
    public Guid UserId { get; set; }
    public Guid BranchId { get; set; }
}