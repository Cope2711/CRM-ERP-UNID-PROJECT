namespace CRM_ERP_UNID.Exceptions;

public class BadRequestException : Exception
{
    public string? Field { get; }
     public BadRequestException(string message, string? field = null) : base(message)
    {
        Field = field;
    }
}