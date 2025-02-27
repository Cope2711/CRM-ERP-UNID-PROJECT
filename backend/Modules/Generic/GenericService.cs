using System.Linq.Expressions;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public interface IGenericServie<T> where T : class
{
    Task<T?> GetById(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    
    /// <summary>
    /// Retrieves an entity by its unique identifier, optionally including related data.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="include">An optional function to include related data in the query.</param>
    /// <returns>The entity with the specified identifier, or null if not found.</returns>

    Task<T> GetFirstThrowsNotFoundAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);
    
    /// <summary>
    /// Retrieves the first entity that matches the specified field value and throws an exception if no matching entity is found.
    /// </summary>
    /// <param name="fieldSelector">An expression that selects the field to compare.</param>
    /// <param name="value">The value to compare against.</param>
    /// <param name="include">An optional function to include related data in the query.</param>
    /// <returns>The first matching entity.</returns>
    /// <exception cref="NotFoundException">Thrown when no matching entity is found.</exception>

    Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);
    
    /// <summary>
    /// Retrieves the first entity that matches the specified field value, or null if no matching entity is found.
    /// </summary>
    /// <param name="fieldSelector">An expression that selects the field to compare.</param>
    /// <param name="value">The value to compare against.</param>
    /// <param name="include">An optional function to include related data in the query.</param>
    /// <returns>The first matching entity if found; otherwise, null.</returns>

    Task<bool> ExistsAsync(Expression<Func<T, object>> fieldSelector, object value);
    
    /// <summary>
    /// Checks if any entity exists that matches the specified field value.
    /// </summary>
    /// <param name="fieldSelector">An expression that selects the field to compare.</param>
    /// <param name="value">The value to compare against.</param>
    /// <returns>True if a matching entity exists; otherwise, false.</returns>

    Task<T> GetByIdThrowsNotFoundAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    
    /// <summary>
    /// Retrieves an entity by its unique identifier and throws an exception if the entity is not found, optionally including related data.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="include">An optional function to include related data in the query.</param>
    /// <returns>The entity with the specified identifier.</returns>
    /// <exception cref="NotFoundException">Thrown when no entity with the given ID is found.</exception>
    
    Task<GetAllResponseDto<T>> GetAllAsync(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null);
    
    /// <summary>
    /// Retrieves all entities based on the specified filtering and pagination parameters, with an optional query modifier.
    /// </summary>
    /// <param name="getAllDto">A DTO containing filtering and pagination parameters.</param>
    /// <param name="queryModifier">An optional function to modify the query for additional filtering or ordering.</param>
    /// <returns>A response DTO containing the list of retrieved entities along with associated metadata.</returns>
}



public class GenericService<T> : IGenericServie<T> where T : class
{
    private readonly IGenericRepository<T> _genericRepository;

    public GenericService(IGenericRepository<T> genericRepository)
    {
        _genericRepository = genericRepository;
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

    public async Task<bool> ExistsAsync(Expression<Func<T, object>> fieldSelector, object value)
    {
        return await _genericRepository.ExistsAsync(fieldSelector, value);
    }

    public async Task<T> GetByIdThrowsNotFoundAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null)
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