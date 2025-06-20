using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class GenericService<T>(
    IGenericRepository<T> _genericRepository,
    ILogger<GenericService<T>> _logger,
    IHttpContextAccessor _httpContextAccessor
) : IGenericService<T> where T : class
{
    public async Task<(T, bool)> Update(T entity, JsonElement data)
    {
        await ValidateUpdateFields(entity, data);

        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        _logger.LogInformation("User with id: {authenticatedUserId} Updating entity of type {EntityType}",
            authenticatedUserId, typeof(T).Name);

        Type entityType = typeof(T);
        bool hasChanges = false;

        foreach (var jsonProp in data.EnumerateObject())
        {
            // Busca la propiedad correspondiente en el modelo
            var prop = entityType.GetProperty(jsonProp.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null || !prop.CanWrite) continue;

            // Deserializa el valor del JSON al tipo correcto
            object? newValue = JsonSerializer.Deserialize(jsonProp.Value.GetRawText(), prop.PropertyType);
            object? currentValue = prop.GetValue(entity);

            if (!Equals(newValue, currentValue))
            {
                prop.SetValue(entity, newValue);
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _genericRepository.SaveChanges();
            _logger.LogInformation("User with id: {authenticatedUserId} Updated entity of type {EntityType}",
                authenticatedUserId, typeof(T).Name);
        }
        else
        {
            _logger.LogInformation("User with id: {authenticatedUserId} Not Updated entity of type {EntityType}",
                authenticatedUserId, typeof(T).Name);
        }

        return (entity, hasChanges);
    }

    private async Task ValidateUpdateFields(T entity, JsonElement data)
    {
        var modelType = typeof(T);

        foreach (var jsonProp in data.EnumerateObject())
        {
            var propInfo = modelType.GetProperty(jsonProp.Name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propInfo == null) continue;

            object? newValue = JsonSerializer.Deserialize(
                jsonProp.Value.GetRawText(),
                propInfo.PropertyType);

            var validationAttributes = propInfo
                .GetCustomAttributes<ValidationAttribute>(inherit: true)
                .Where(attr => attr.GetType() != typeof(UniqueAttribute));

            var context = new ValidationContext(entity)
            {
                MemberName = propInfo.Name
            };

            foreach (var attr in validationAttributes)
            {
                var result = attr.GetValidationResult(newValue, context);
                if (result != ValidationResult.Success)
                {
                    throw new ValidationException(result!.ErrorMessage ?? $"Invalid value for {propInfo.Name}");
                }
            }

            if (Attribute.IsDefined(propInfo, typeof(UniqueAttribute)))
            {
                var currentValue = propInfo.GetValue(entity);
                if (newValue != null && !Equals(currentValue, newValue))
                {
                    var parameter = Expression.Parameter(modelType, "x");
                    var member = Expression.Property(parameter, propInfo.Name);
                    var constant = Expression.Constant(newValue);
                    var body = Expression.Equal(
                        Expression.Convert(member, typeof(object)),
                        Expression.Convert(constant, typeof(object))
                    );
                    var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

                    bool exists = await ExistsAsync(lambda);
                    if (exists)
                    {
                        throw new UniqueConstraintViolationException(
                            $"{modelType.Name} with {propInfo.Name} '{newValue}' already exists",
                            propInfo.Name
                        );
                    }
                }
            }
        }
    }

    public async Task<T> Create(T entity)
    {
        await ValidateCreateFields(entity);

        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}",
            authenticatedUserId, typeof(T).Name);

        _genericRepository.Add(entity);
        await _genericRepository.SaveChanges();

        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}",
            authenticatedUserId, typeof(T).Name);

        return entity;
    }

    private async Task ValidateCreateFields(T entity)
    {
        var propertiesWithUniqueAttr = typeof(T)
            .GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute)))
            .Where(p => p.GetValue(entity) != null);

        foreach (PropertyInfo prop in propertiesWithUniqueAttr)
        {
            var value = prop.GetValue(entity);

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.Property(parameter, prop.Name);
            var constant = Expression.Constant(value);

            Expression body = Expression.Equal(
                Expression.Convert(member, typeof(object)),
                Expression.Convert(constant, typeof(object))
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            bool exists = await ExistsAsync(lambda);
            if (exists)
            {
                throw new UniqueConstraintViolationException(
                    $"{typeof(T).Name} with {prop.Name} '{value}' already exists",
                    prop.Name
                );
            }
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _genericRepository.ExistsAsync(predicate);
    }

    public async Task<T?> GetById(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        return await _genericRepository.GetByIdAsync(id, include);
    }

    public async Task<T> GetFirstThrowsNotFoundAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        T? entity = await _genericRepository.GetFirstAsync(fieldSelector, value, include);

        if (entity == null)
        {
            string fieldName =
                ((MemberExpression)(fieldSelector.Body is UnaryExpression unary ? unary.Operand : fieldSelector.Body))
                .Member.Name;

            throw new NotFoundException(
                message: $"{typeof(T).Name} with {fieldName}: {value} not found!",
                field: fieldName
            );
        }

        return entity;
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        return await _genericRepository.GetFirstAsync(fieldSelector, value, include);
    }

    public async Task<T> GetByIdThrowsNotFound(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        T? entity = await _genericRepository.GetByIdAsync(id, include);

        if (entity == null)
        {
            throw new NotFoundException(
                message: $"{typeof(T).Name} with id: {id} not found!",
                field: _genericRepository.GetKeyProperty()?.Name ?? "Id"
            );
        }

        return entity;
    }

    public async Task<GetAllResponseDto<T>> GetAllAsync(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null)
    {
        GetAllResponseDto<T> response = new GetAllResponseDto<T>
        {
            TotalItems = await _genericRepository.GetTotalItemsAsync(getAllDto),
            PageNumber = getAllDto.PageNumber,
            PageSize = getAllDto.PageSize
        };

        response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / getAllDto.PageSize);
        response.Data = await _genericRepository.GetAllAsync(getAllDto);
        return response;
    }
}