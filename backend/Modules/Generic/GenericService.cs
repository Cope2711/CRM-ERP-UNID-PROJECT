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
    public async Task<T> Create(T entity)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        await ValidateFields(entity);

        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}", authenticatedUserId, typeof(T).Name);

        _genericRepository.Add(entity);
        await _genericRepository.SaveChanges();

        _logger.LogInformation("User with id: {authenticatedUserId} Creating entity of type {EntityType}", authenticatedUserId, typeof(T).Name);

        return entity;
    }

    private async Task ValidateFields(T entity)
    {
        var propertiesWithUniqueAttr = typeof(T)
            .GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

        foreach (PropertyInfo prop in propertiesWithUniqueAttr)
        {
            var value = prop.GetValue(entity);
            if (value == null) continue;

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