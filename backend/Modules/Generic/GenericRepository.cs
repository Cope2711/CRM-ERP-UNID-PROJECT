using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include = null);

    Task<T?> GetFirstAsync(Expression<Func<T, object>> fieldSelector, object value,
        Func<IQueryable<T>, IQueryable<T>> include = null);
    
    Task<bool> ExistsAsync(Expression<Func<T, object>> fieldSelector, object value);

    Task<List<T>> GetAllAsync(GetAllDto getAllDto, Func<IQueryable<T>, IQueryable<T>> queryModifier = null);

    Task<int> GetTotalItemsAsync(GetAllDto getAllDto);

    PropertyInfo? GetKeyProperty();
}


public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, object>> fieldSelector, object value)
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        string fieldName =
            ((MemberExpression)(fieldSelector.Body is UnaryExpression unary ? unary.Operand : fieldSelector.Body))
            .Member.Name;

        return await queryable.AnyAsync(e => EF.Property<object>(e, fieldName).Equals(value));
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

    private Expression<Func<T, bool>> BuildKeyPredicate(Guid id, PropertyInfo keyProperty)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, keyProperty);
        var constant = Expression.Constant(id);
        var equals = Expression.Equal(propertyAccess, constant);

        return Expression.Lambda<Func<T, bool>>(equals, parameter);
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

    public async Task<List<T>> GetAllAsync(GetAllDto getAllDto, Func<IQueryable<T>, IQueryable<T>> queryModifier = null)
    {
        IQueryable<T> queryable = _dbSet.AsQueryable();

        if (queryModifier != null)
        {
            queryable = queryModifier(queryable);
        }

        // Aplica filtros dinámicos
        queryable = ApplyFilters(queryable, getAllDto.Filters);
        
        // Aplica el ordenamiento
        queryable = ApplyOrdering(queryable, getAllDto.OrderBy, getAllDto.Descending);

        // Aplica la paginación
        return await ApplyPagination(queryable, getAllDto.PageNumber, getAllDto.PageSize).ToListAsync();
    }

    public async Task<int> GetTotalItemsAsync(GetAllDto getAllDto)
    {
        IQueryable<T> query = _dbSet.AsQueryable();
        query = ApplyFilters(query, getAllDto.Filters);
        return await query.CountAsync();
    }

    public PropertyInfo? GetKeyProperty()
    {
        return typeof(T).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());
    }

    private static IQueryable<T> ApplyFilters(IQueryable<T> query, List<FilterDto>? filters)
    {
        if (filters == null || !filters.Any()) return query;

        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        Expression? finalExpression = null;

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(filter.Column,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null) continue;

            var left = Expression.Property(parameter, property);
            var right = Expression.Constant(Convert.ChangeType(filter.Value, property.PropertyType));

            Expression comparison;
            switch (filter.Operator.ToLower())
            {
                case "==":
                    comparison = Expression.Equal(left, right);
                    break;
                case "!=":
                    comparison = Expression.NotEqual(left, right);
                    break;
                case ">":
                    comparison = Expression.GreaterThan(left, right);
                    break;
                case "<":
                    comparison = Expression.LessThan(left, right);
                    break;
                case ">=":
                    comparison = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    comparison = Expression.LessThanOrEqual(left, right);
                    break;
                case "like":
                    comparison = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, right);
                    break;
                case "contains":
                    comparison = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, right);
                    break;
                default:
                    continue;
            }

            finalExpression = finalExpression == null ? comparison : Expression.AndAlso(finalExpression, comparison);
        }

        if (finalExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    private IQueryable<T> ApplyOrdering(IQueryable<T> queryable, string? orderBy, bool descending)
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
}