namespace CRM_ERP_UNID.Exceptions;

public class BadRequestException : Exception
{
    public string? Field { get; }
    public string? Reason { get; }
     public BadRequestException(string message, string? field = null, string? reason= null) : base(message)
    {
        Field = field;
        Reason = reason;
    }
}