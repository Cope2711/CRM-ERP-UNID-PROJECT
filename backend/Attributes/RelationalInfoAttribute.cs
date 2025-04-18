namespace CRM_ERP_UNID.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RelationInfoAttribute : Attribute
{
    public string RelationModel { get; }
    public string? Controller { get; }
    public string[] Selects { get; }

    public RelationInfoAttribute(string relationModel, string? controller = null, string[]? selects = null)
    {
        RelationModel = relationModel;
        Controller = controller;
        Selects = selects ?? Array.Empty<string>();
    }
}