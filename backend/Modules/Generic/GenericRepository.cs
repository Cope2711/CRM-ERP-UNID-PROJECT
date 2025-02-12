using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using CRM_ERP_UNID.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include = null);

    Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);

    Task<List<T>> GetAllAsync<GetAllDto>(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null);

    Task<int> GetTotalItemsAsync<GetAllDto>(GetAllDto getAllDto);
    PropertyInfo? GetKeyProperty();
}

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        if (include != null)
        {
            queryable = include(queryable);
        }

        var keyProperty = GetKeyProperty();

        if (keyProperty == null)
        {
            throw new InvalidOperationException(
                $"La entidad {typeof(T).Name} no tiene una propiedad clave primaria definida.");
        }

        return await queryable
            .Where(BuildKeyPredicate(id, keyProperty))
            .FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstAsync(
        Expression<Func<T, object>> fieldSelector,
        object value,
        Func<IQueryable<T>, IQueryable<T>> include = null)
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        // Aplicar Includes si hay relaciones
        if (include != null)
        {
            queryable = include(queryable);
        }

        // Obtener el nombre del campo de la expresión
        string fieldName =
            ((MemberExpression)(fieldSelector.Body is UnaryExpression unary ? unary.Operand : fieldSelector.Body))
            .Member.Name;

        return await queryable.FirstOrDefaultAsync(e => EF.Property<object>(e, fieldName).Equals(value));
    }

    public async Task<List<T>> GetAllAsync<GetAllDto>(
        GetAllDto getAllDto,
        Func<IQueryable<T>, IQueryable<T>> queryModifier = null)
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        if (queryModifier != null)
        {
            queryable = queryModifier(queryable);
        }

        var searchTerm = GetPropertyValue<string>(getAllDto, "SearchTerm");
        var searchColumn = GetPropertyValue<string>(getAllDto, "SearchColumn");
        var orderBy = GetPropertyValue<string>(getAllDto, "OrderBy");
        var descending = GetPropertyValue<bool>(getAllDto, "Descending");
        var pageNumber = GetPropertyValue<int>(getAllDto, "PageNumber", 1);
        var pageSize = GetPropertyValue<int>(getAllDto, "PageSize", 10);

        // Aplica el filtro de búsqueda
        queryable = ApplySearchFilter(queryable, searchTerm, searchColumn);
        // Aplica el orden
        queryable = ApplyOrdering(queryable, orderBy, descending);

        if (queryModifier != null)
        {
            queryable = queryModifier(queryable);
        }

        // Aplica la paginación
        return await ApplyPagination(queryable, pageNumber, pageSize).ToListAsync();
    }

    public async Task<int> GetTotalItemsAsync<GetAllDto>(GetAllDto getAllDto)
    {
        IQueryable<T> query = _dbSet.AsQueryable();

        var searchTerm = GetPropertyValue<string>(getAllDto, "SearchTerm");
        var searchColumn = GetPropertyValue<string>(getAllDto, "SearchColumn");

        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchColumn))
        {
            query = ApplySearchFilter(query, searchTerm, searchColumn);
        }
        else if (!string.IsNullOrEmpty(searchTerm))
        {
            query = ApplyStringSearch(query, searchTerm);
        }

        return await query.CountAsync();
    }

    public PropertyInfo? GetKeyProperty()
    {
        return typeof(T).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());
    }

    private Expression<Func<T, bool>> BuildKeyPredicate(Guid id, PropertyInfo keyProperty)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, keyProperty);
        var constant = Expression.Constant(id);
        var equals = Expression.Equal(propertyAccess, constant);

        return Expression.Lambda<Func<T, bool>>(equals, parameter);
    }

    private static TProperty GetPropertyValue<TProperty>(object obj, string propertyName,
        TProperty defaultValue = default)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property != null ? (TProperty)property.GetValue(obj) : defaultValue;
    }

    private IQueryable<T> ApplySearchFilter(IQueryable<T> queryable, string searchTerm, string searchColumn)
    {
        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchColumn))
        {
            var property = typeof(T).GetProperty(searchColumn,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property != null)
            {
                // Is a boolean?
                if (property.PropertyType == typeof(bool) && bool.TryParse(searchTerm, out bool boolValue))
                {
                    queryable = queryable.Where(e => EF.Property<bool>(e, searchColumn) == boolValue);
                }
                // Is string?
                else if (property.PropertyType == typeof(string))
                {
                    queryable = queryable.Where(e =>
                        EF.Functions.Like(EF.Property<string>(e, searchColumn), $"%{searchTerm}%"));
                }
                // Is Guid?
                else if (property.PropertyType == typeof(Guid) && Guid.TryParse(searchTerm, out Guid guidValue))
                {
                    queryable = queryable.Where(e => EF.Property<Guid>(e, searchColumn) == guidValue);
                }
            }
        }

        return queryable;
    }

    private IQueryable<T> ApplyOrdering(IQueryable<T> queryable, string orderBy, bool descending)
    {
        if (!string.IsNullOrEmpty(orderBy))
        {
            var orderProperty = typeof(T).GetProperty(orderBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (orderProperty != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var propertyAccess = Expression.Property(parameter, orderProperty);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                var methodName = descending ? "OrderByDescending" : "OrderBy";
                var orderByCall = Expression.Call(typeof(Queryable), methodName,
                    new Type[] { typeof(T), orderProperty.PropertyType }, queryable.Expression,
                    Expression.Quote(orderExpression));

                queryable = queryable.Provider.CreateQuery<T>(orderByCall);
            }
        }

        return queryable;
    }

    private IQueryable<T> ApplyPagination(IQueryable<T> queryable, int pageNumber, int pageSize)
    {
        return queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    private IQueryable<T> ApplyStringSearch(IQueryable<T> query, string searchTerm)
    {
        var stringProperties = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();
        if (stringProperties.Any())
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var searchExpressions = stringProperties.Select(p =>
                (Expression)Expression.Call(Expression.Property(parameter, p),
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!, Expression.Constant(searchTerm))
            ).ToList();

            Expression combinedExpression = searchExpressions.First();
            foreach (var expr in searchExpressions.Skip(1))
            {
                combinedExpression = Expression.OrElse(combinedExpression, expr);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}