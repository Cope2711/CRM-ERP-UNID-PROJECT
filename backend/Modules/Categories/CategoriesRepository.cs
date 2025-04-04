using CRM_ERP_UNID.Data;
using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface ICategoriesRepository
{
    void Add(Category category);
    Task SaveChanges();
    void Remove(Category category);
}

public class CategoriesRepository(
    AppDbContext _context
) : ICategoriesRepository
{
    public void Add(Category category)
    {
        _context.Categories.Add(category);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }

    public void Remove(Category category)
    {
        _context.Categories.Remove(category);
    }
}