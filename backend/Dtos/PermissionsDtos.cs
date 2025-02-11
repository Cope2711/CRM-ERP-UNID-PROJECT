namespace CRM_ERP_UNID.Dtos;

public class PermissionDto
{
    public Guid PermissionId { get; set; }
    public required string PermissionName { get; set; }
    public string? PermissionDescription { get; set; }
}