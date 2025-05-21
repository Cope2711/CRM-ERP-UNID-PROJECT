namespace CRM_ERP_UNID.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ReferenceInfoAttribute : Attribute
{
    public string Controller { get; }
    public string Select { get; } 

    public ReferenceInfoAttribute(string controller, string select)
    {
        Controller = controller;
        Select = select;
    }
}