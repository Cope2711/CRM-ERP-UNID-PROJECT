namespace CRM_ERP_UNID.Exceptions;

public class ForbiddenException : Exception
{
    public string? Permission { get; }
    public string? Resource { get; }
    public string? Field { get; }

    public ForbiddenException(string message, string? permission = null, string? resource = null, string? field = null)
        : base(message)
    {
        Permission = permission;
        Resource = resource;
        Field = field;
    }
}
