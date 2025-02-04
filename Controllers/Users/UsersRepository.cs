using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Controllers;

public interface IUsersRepository
{
    Task<List<User>> GetAll(GetAllDto getAllDto);
    Task<int> GetTotalItems(GetAllDto getAllDto);
    Task<User?> GetById(Guid id);
    Task<User?> GetByUserName(string userName);
    Task<User?> GetByEmail(string email);
    void Add(User user);
    Task SaveChangesAsync();
}

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context)
    {
        this._context = context;
    }

    public async Task<List<User>> GetAll(GetAllDto getAllDto)
    {
        IQueryable<User> queryable = this._context.Users;

        // Si no existe un searchterm no se aplica ningun filtro de busqueda
        if (!string.IsNullOrEmpty(getAllDto.SearchTerm) && !string.IsNullOrEmpty(getAllDto.SearchColumn))
        {
            var property = typeof(User).GetProperty(getAllDto.SearchColumn);

            if (property != null)
            {
                if (property.PropertyType == typeof(bool))
                {
                    // Convertir el SearchTerm a booleano
                    if (bool.TryParse(getAllDto.SearchTerm, out bool boolValue))
                    {
                        queryable = queryable.Where(u => EF.Property<bool>(u, getAllDto.SearchColumn) == boolValue);
                    }
                }
                else
                {
                    // Filtrado para propiedades de tipo string
                    queryable = queryable.Where(u =>
                        EF.Functions.Like(EF.Property<string>(u, getAllDto.SearchColumn), $"%{getAllDto.SearchTerm}%"));
                }
            }
        }

        if (!string.IsNullOrEmpty(getAllDto.OrderBy))
        {
            if (getAllDto.Descending)
            {
                queryable = queryable.OrderByDescending(u => EF.Property<object>(u, getAllDto.OrderBy));
            }
            else
            {
                queryable = queryable.OrderBy(u => EF.Property<object>(u, getAllDto.OrderBy));
            }
        }

        return await queryable
            .Skip((getAllDto.PageNumber - 1) * getAllDto.PageSize)
            .Take(getAllDto.PageSize)
            .ToListAsync();
    }


    public async Task<int> GetTotalItems(GetAllDto getAllDto)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(getAllDto.SearchTerm) && !string.IsNullOrEmpty(getAllDto.SearchColumn))
        {
            var property = typeof(User).GetProperty(getAllDto.SearchColumn);

            if (property != null)
            {
                if (property.PropertyType == typeof(bool))
                {
                    // Conversión de SearchTerm a bool
                    if (bool.TryParse(getAllDto.SearchTerm, out bool boolValue))
                    {
                        query = query.Where(u => EF.Property<bool>(u, getAllDto.SearchColumn) == boolValue);
                    }
                }
                else
                {
                    // Filtrado para propiedades de tipo string
                    query = query.Where(u =>
                        EF.Functions.Like(EF.Property<string>(u, getAllDto.SearchColumn), $"%{getAllDto.SearchTerm}%"));
                }
            }
        }
        else if (!string.IsNullOrEmpty(getAllDto.SearchTerm))
        {
            query = query.Where(u =>
                u.UserUserName.Contains(getAllDto.SearchTerm) ||
                u.UserFirstName.Contains(getAllDto.SearchTerm) ||
                u.UserLastName.Contains(getAllDto.SearchTerm) ||
                u.UserEmail.Contains(getAllDto.SearchTerm)
            );
        }

        return await query.CountAsync();
    }


    public async Task<User?> GetById(Guid id)
    {
        return await this._context.Users.Include(u=> u.Role).FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await this._context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserUserName == userName);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await this._context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserEmail == email);
    }

    public void Add(User user)
    {
        this._context.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }
}