using CRM_ERP_UNID.Data;

namespace CRM_ERP_UNID.Modules;

public interface IPermissionRepository
{

}

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context)
    {
        _context = context;
    }
}