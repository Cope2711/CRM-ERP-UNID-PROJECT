namespace CRM_ERP_UNID.Helpers;

public static class StringsHelper
{
    public static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            return str;

        return char.ToLower(str[0]) + str.Substring(1);
    }
}