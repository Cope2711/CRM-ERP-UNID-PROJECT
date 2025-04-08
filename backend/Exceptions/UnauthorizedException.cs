namespace CRM_ERP_UNID.Exceptions;

public class UnauthorizedException : Exception
{
    public string Reason { get; }
    public string? Field { get; }
    
    public UnauthorizedException(string message, string reason, string? field = null)
        : base(message)
    {
        Reason = reason;
        Field = field;
    }
}