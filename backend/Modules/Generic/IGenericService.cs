using System.Linq.Expressions;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IGenericService<T> where T : class
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