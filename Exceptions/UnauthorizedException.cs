namespace CRM_ERP_UNID.Exceptions;

public class UnauthorizedException : Exception
{
    public string Reason { get; }

    public UnauthorizedException(string message, string reason) 
        : base(message)
    {
        Reason = reason;
    }
}