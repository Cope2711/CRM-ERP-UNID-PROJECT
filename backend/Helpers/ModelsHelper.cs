using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Helpers;

public static class ModelsHelper
{
    public static bool UpdateModel<TModel, TDto>(TModel model, TDto dto, Func<string, object, Task<bool>>? uniqueValidation = null)
        where TModel : class
        where TDto : class
    {
        bool hasChanges = false;

        foreach (var dtoProperty in typeof(TDto).GetProperties())
        {
            var newValue = dtoProperty.GetValue(dto);
            if (newValue == null) continue;

            if (dtoProperty.PropertyType == typeof(Guid) && (Guid)newValue == Guid.Empty)
                continue;

            var modelProperty = typeof(TModel).GetProperty(dtoProperty.Name);
            if (modelProperty == null) continue;

            var currentValue = modelProperty.GetValue(model);

            if (!object.Equals(currentValue, newValue))
            {
                if (uniqueValidation != null && uniqueValidation(dtoProperty.Name, newValue).Result)
                {
                    throw new UniqueConstraintViolationException(
                        $"A record with {dtoProperty.Name} '{newValue}' already exists.",
                        field: dtoProperty.Name);
                }

                modelProperty.SetValue(model, newValue);
                hasChanges = true;
            }
        }

        return hasChanges;
    }
}
