using System.Linq.Expressions;
using System.Reflection;
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
    public async Task<(T, bool)> Update(T entity, object updateDto)
    {
        await ValidateUpdateFields(entity, updateDto);
        
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        _logger.LogInformation("User with id: {authenticatedUserId} Updating entity of type {EntityType}", authenticatedUserId, typeof(T).Name);

        
        var dtoType = updateDto.GetType();
        var entityType = typeof(T);

        bool hasChanges = false;

        var nonNullDtoProps = dtoType.GetProperties()
            .Where(p => p.GetValue(updateDto) != null);
        
        foreach (var dtoProp in nonNullDtoProps)
        {
            var entityProp = entityType.GetProperty(dtoProp.Name);
            if (entityProp == null || !entityProp.CanWrite) continue;

            var newValue = dtoProp.GetValue(updateDto);
            var currentValue = entityProp.GetValue(entity);

            if (!Equals(newValue, currentValue))
            {
                entityProp.SetValue(entity, newValue);
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _genericRepository.SaveChanges();
            _logger.LogInformation("User with id: {authenticatedUserId} Updated entity of type {EntityType}", authenticatedUserId, typeof(T).Name);
        }
        else
        {
            _logger.LogInformation("User with id: {authenticatedUserId} Not Updated entity of type {EntityType}", authenticatedUserId, typeof(T).Name);
        }
        
        return (entity, hasChanges);
    }
    
    public async Task<T> Create(T entity)
    {
        await ValidateCreateFields(entity);
        
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}", authenticatedUserId, typeof(T).Name);

        _genericRepository.Add(entity);
        await _genericRepository.SaveChanges();

        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}", authenticatedUserId, typeof(T).Name);

        return entity;
    }

    private async Task ValidateUpdateFields(T entity, object updateDto)
    {
        var modelType = typeof(T);
        var dtoType = updateDto.GetType();

        var propertiesWithUniqueAttr = modelType
            .GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute)))
            .Where(p =>
            {
                var dtoProp = dtoType.GetProperty(p.Name);
                if (dtoProp == null) return false;
                var newValue = dtoProp.GetValue(updateDto);
                var currentValue = p.GetValue(entity);
                // Validar solo si hay nuevo valor y cambió respecto al actual
                return newValue != null && !Equals(currentValue, newValue);
            });

        foreach (PropertyInfo modelProp in propertiesWithUniqueAttr)
        {
            var dtoProp = dtoType.GetProperty(modelProp.Name);
            var newValue = dtoProp.GetValue(updateDto);

            // Armar expresión lambda: x => x.Prop == newValue
            var parameter = Expression.Parameter(modelType, "x");
            var member = Expression.Property(parameter, modelProp.Name);
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
                    $"{modelType.Name} with {modelProp.Name} '{newValue}' already exists",
                    modelProp.Name
                );
            }
        }
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