using System.Linq.Expressions;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public interface IGenericServie<T> where T : class
{
    Task<T?> GetById(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<T> GetFirstThrowsNotFoundAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);

    Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);

    Task<bool> ExistsAsync(Expression<Func<T, object>> fieldSelector, object value);

    Task<T> GetByIdThrowsNotFoundAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<GetAllResponseDto<T>> GetAllAsync(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null);
}

public class GenericService<T>(
    IGenericRepository<T> _genericRepository
    ) : IGenericServie<T> where T : class
{
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