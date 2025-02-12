namespace CRM_ERP_UNID.Exceptions;

public class ForbiddenException : Exception
{
    public string Permission { get; }
    public string? Resource { get; }

    public ForbiddenException(string message, string permission, string? resource = null) : base(message)
    {
        Permission = permission;
        Resource = resource;
    }
}
