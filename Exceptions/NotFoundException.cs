namespace CRM_ERP_UNID.Exceptions;

public class NotFoundException : Exception
{
    public string Field { get; }

    public NotFoundException(string message, string field) : base(message)
    {
        Field = field;
    }
}