namespace CRM_ERP_UNID.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RelationInfoAttribute : Attribute
{
    public string RelationModel { get; }
    public string? Controller { get; }
    public string[] Selects { get; }

    public RelationInfoAttribute(
        string relationModel,
        string? controller = null,
        string[]? selects = null,
        string? relationalIdName = null
    )
    {
        RelationModel = relationModel;
        Controller = controller;

        if (!string.IsNullOrEmpty(relationalIdName) && selects != null)
        {
            // Combina relacionalIdName con selects[] (al frente)
            Selects = new[] { relationalIdName }.Concat(selects).ToArray();
        }
        else if (selects != null)
        {
            Selects = selects;
        }
        else
        {
            Selects = Array.Empty<string>();
        }
    }
}