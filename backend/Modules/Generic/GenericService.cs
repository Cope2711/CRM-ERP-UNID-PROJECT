using System.Linq.Expressions;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public interface IGenericServie<T> where T : class
{
    Task<T?> GetById(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<T> GetFirstThrowsNotFoundAsync(Expression<Func<T, object>> fieldSelector, object value,
        params Expression<Func<T, object>>[] includes);

    Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        params Expression<Func<T, object>>[] includes);

    Task<T> GetByIdThrowsNotFoundAsync(Guid id, Func<IQueryable<T>, IQueryable<T>>? include = null);

    Task<GetAllResponseDto<T>> GetAllAsync(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null);
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
        params Expression<Func<T, object>>[] includes)
    {
        T? entity = await _genericRepository.GetFirstAsync(fieldSelector, value, includes);

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
        params Expression<Func<T, object>>[] includes)
    {
        return await _genericRepository.GetFirstAsync(fieldSelector, value, includes);
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
        GetAllResponseDto<T> getAllResponseDto = new GetAllResponseDto<T>();
        getAllResponseDto.TotalItems = await this._genericRepository.GetTotalItemsAsync(getAllDto);
        getAllResponseDto.TotalPages = (int)Math.Ceiling((double)getAllResponseDto.TotalItems / getAllDto.PageSize);
        getAllResponseDto.PageNumber = getAllDto.PageNumber;
        getAllResponseDto.PageSize = getAllDto.PageSize;
        getAllResponseDto.Data = await this._genericRepository.GetAllAsync(getAllDto, queryModifier);
        return getAllResponseDto;
    }
}