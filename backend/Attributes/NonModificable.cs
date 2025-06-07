using System.ComponentModel.DataAnnotations;

namespace CRM_ERP_UNID.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NonModificable : ValidationAttribute
{
    public NonModificable()
    {
        
    }
}