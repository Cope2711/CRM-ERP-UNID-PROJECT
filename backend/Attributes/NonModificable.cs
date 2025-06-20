namespace CRM_ERP_UNID.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NonModificable : Attribute
{
    public NonModificable()
    {
        
    }
}