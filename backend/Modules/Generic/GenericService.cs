using System.Linq.Expressions;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public class GenericService<T>(
    IGenericRepository<T> _genericRepository) : IGenericService<T> where T : class
{
    private IGenericService<T> _genericServiceImplementation;
    private IGenericService<T> _genericServiceImplementation1;

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
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null,
        List<string>? camposPermitidos = null)
    {
        GetAllResponseDto<T> response = new GetAllResponseDto<T>     
        {
            TotalItems = await _genericRepository.GetTotalItemsAsync(getAllDto),
            PageNumber = getAllDto.PageNumber,
            PageSize = getAllDto.PageSize
        };
        
        response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / getAllDto.PageSize);
        var data = await _genericRepository.GetAllAsync(getAllDto);

        // Campos restringidos insertale mas no se cuales xde
        var camposRestringidos = new HashSet<string> { "UserPassword" };

        // Si se proporcionan campos permitidos, filtrar los campos restringidos
        if (camposPermitidos != null)
        {
            camposPermitidos = camposPermitidos.Except(camposRestringidos).ToList();
        }

        var filteredData = new List<Dictionary<string, object>>();
        foreach (var item in data)
        {
            var newItem = new Dictionary<string, object>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if ((camposPermitidos == null || camposPermitidos.Contains(prop.Name)) && 
                    !camposRestringidos.Contains(prop.Name) && 
                    !string.IsNullOrEmpty(prop.Name) &&
                    item is IDictionary<string, object> expandoDict && expandoDict.ContainsKey(prop.Name))
                {
                    newItem[prop.Name] = expandoDict[prop.Name];
                }
            }
            filteredData.Add(newItem);
        }
        //datos filtrados a la respuesta
        response.Data = filteredData;
        return response;
    }
}