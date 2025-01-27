namespace CRM_ERP_UNID.Exceptions;

public class UniqueConstraintViolationException : Exception
{
    public string Field { get; } // Nombre del campo afectado

    public UniqueConstraintViolationException(string message, string field) : base(message)
    {
        Field = field;
    }
}