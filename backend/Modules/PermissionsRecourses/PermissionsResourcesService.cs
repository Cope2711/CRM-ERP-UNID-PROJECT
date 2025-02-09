using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IPermissionsResourcesService
{
    Task<GetAllResponseDto<PermissionResource>> GetAllAsync(GetAllDto getAllDto);
}

public class PermissionsResourcesService : IPermissionsResourcesService
{
    private readonly IGenericServie<PermissionResource> _genericService;
    
    public PermissionsResourcesService(IGenericServie<PermissionResource> genericService)
    {
        _genericService = genericService;
    }

    public async Task<GetAllResponseDto<PermissionResource>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, query => query.Include(pr => pr.Permission).Include(pr => pr.Resource));
    }
}