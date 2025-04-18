using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using CRM_ERP_UNID.Constants;
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

    Task<List<Dictionary<string, object>>> GetAllAsync(GetAllDto getAllDto, Func<IQueryable<T>, IQueryable<T>> queryModifier = null);

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

    public async Task<List<Dictionary<string, object>>> GetAllAsync(GetAllDto getAllDto, Func<IQueryable<T>, IQueryable<T>> queryModifier = null)
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
        queryable = ApplyPagination(queryable, getAllDto.PageNumber, getAllDto.PageSize);

        // Aplica selección de campos
        var selectedQuery = ApplySelects(queryable, getAllDto.Selects);

        return await selectedQuery.ToListAsync();
    }
    
    private static IQueryable<Dictionary<string, object>> ApplySelects<T>(IQueryable<T> query, List<string> selects)
    {
        if (selects == null || !selects.Any()) return query.Select(e => typeof(T).GetProperties()
            .ToDictionary(p => p.Name, p => (object)p.GetValue(e, null) ?? DBNull.Value));

        var parameter = Expression.Parameter(typeof(T), "e");

        var bindings = selects
            .Select(column =>
            {
                var propertyPath = column.Split('.');  // Dividir el path en partes (por ejemplo: "Role.roleName")
                Expression expression = Expression.Property(parameter, propertyPath[0]);  // Primer nivel

                // Navegar a través de las propiedades anidadas si hay más niveles
                for (int i = 1; i < propertyPath.Length; i++)
                {
                    var property = expression.Type.GetProperty(propertyPath[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null) return null;

                    expression = Expression.Property(expression, property);
                }

                return new { Column = column, Expression = expression };
            })
            .Where(x => x != null)
            .ToList();

        if (!bindings.Any()) return query.Select(e => new Dictionary<string, object>());

        var newExpression = Expression.New(typeof(Dictionary<string, object>));
        var addMethod = typeof(Dictionary<string, object>).GetMethod("Add");

        var memberInit = Expression.ListInit(newExpression, bindings.Select(b =>
            Expression.ElementInit(addMethod, Expression.Constant(b.Column), Expression.Convert(b.Expression, typeof(object)))));

        var lambda = Expression.Lambda<Func<T, Dictionary<string, object>>>(memberInit, parameter);

        return query.Select(lambda);
    }
    
    private static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, List<FilterDto>? filters)
    {
        if (filters == null || !filters.Any()) return query;

        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        Expression? finalExpression = null;

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(filter.Column,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null) continue;

            Type propertyType = property.PropertyType;
            Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            object? filterValue = string.IsNullOrEmpty(filter.Value)
                ? null
                : (underlyingType == typeof(Guid) 
                    ? (object)Guid.Parse(filter.Value)  
                    : Convert.ChangeType(filter.Value, underlyingType)); 

            Expression left = Expression.Property(parameter, property);
            Expression right = Expression.Constant(filterValue, propertyType);

            Expression? comparison = filter.Operator switch
            {
                FilterOperators.Equal => Expression.Equal(left, right),
                FilterOperators.NotEqual => Expression.NotEqual(left, right),
                FilterOperators.GreaterThan => Expression.GreaterThan(left, right),
                FilterOperators.LessThan => Expression.LessThan(left, right),
                FilterOperators.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, right),
                FilterOperators.LessThanOrEqual => Expression.LessThanOrEqual(left, right),
                FilterOperators.Like or FilterOperators.Contains => Expression.Call(left,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!, right),
                FilterOperators.StartsWith => Expression.Call(left,
                    typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, right),
                FilterOperators.EndsWith => Expression.Call(left,
                    typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, right),
                FilterOperators.In when filterValue is not null =>
                    Expression.Call(Expression.Constant(filterValue),
                        typeof(List<>).MakeGenericType(propertyType).GetMethod("Contains", new[] { propertyType })!,
                        left),
                _ => null
            };

            if (comparison == null) continue;

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
            var parameter = Expression.Parameter(typeof(T), "e");

            // Dividir el orderBy por puntos (para manejar propiedades anidadas)
            var propertyPath = orderBy.Split('.');

            // Navegar a través de las propiedades anidadas
            Expression propertyAccess = Expression.Property(parameter, propertyPath[0]);  // Primer nivel

            for (int i = 1; i < propertyPath.Length; i++)
            {
                var property = propertyAccess.Type.GetProperty(propertyPath[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null) return queryable;  // Si no encontramos la propiedad, no aplicamos el orden

                propertyAccess = Expression.Property(propertyAccess, property);  // Navegar al siguiente nivel
            }

            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            // Seleccionar el método adecuado: OrderBy o OrderByDescending
            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var orderByCall = Expression.Call(
                typeof(Queryable), methodName,
                new Type[] { typeof(T), propertyAccess.Type }, 
                queryable.Expression,
                Expression.Quote(orderExpression)
            );

            queryable = queryable.Provider.CreateQuery<T>(orderByCall);
        }

        return queryable;
    }

    private IQueryable<T> ApplyPagination(IQueryable<T> queryable, int pageNumber, int pageSize)
    {
        return queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}