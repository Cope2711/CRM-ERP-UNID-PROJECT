using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;

namespace CRM_ERP_UNID.Modules;

public interface IRoleService
{
    Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto);
    Task<Role> GetByIdThrowsNotFoundAsync(Guid id);
    Task<Role> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<Role> UpdateAsync(UpdateRoleDto updateRoleDto);
    Task<Role> GetByNameThrowsNotFoundAsync(string roleName);
    Task<Role> DeleteById(Guid id);
    Task<Role?> GetByNameAsync(string roleName);
}

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IGenericServie<Role> _genericService;

    public RoleService(IRoleRepository roleRepository, IGenericServie<Role> genericService)
    {
        _roleRepository = roleRepository;
        _genericService = genericService;
    }

    public async Task<Role> DeleteById(Guid id)
    {
        Role role = await GetByIdThrowsNotFoundAsync(id);
        _roleRepository.Remove(role);
        await _roleRepository.SaveChangesAsync();
        return role;
    }
    
    public async Task<GetAllResponseDto<Role>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto);
    }

    public async Task<Role> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }


    public async Task<Role> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        // Exist roleName?
        if (await GetByNameAsync(createRoleDto.RoleName) != null)
        {
            throw new UniqueConstraintViolationException($"A role with the name '{createRoleDto.RoleName}' already exists.",
                field: "RoleName");
        }
        
        Role newRole = new Role
        {
            RoleName = createRoleDto.RoleName,
            RoleDescription = createRoleDto.RoleDescription,
        };

        _roleRepository.Add(newRole);
        await _roleRepository.SaveChangesAsync();

        return newRole;
    }

    public async Task<Role> UpdateAsync(UpdateRoleDto updateRoleDto)
    {
        Role role = await GetByIdThrowsNotFoundAsync(updateRoleDto.RoleId);
        
        role.RoleName = updateRoleDto.RoleName ?? role.RoleName;
        role.RoleDescription = updateRoleDto.RoleDescription ?? role.RoleDescription;
        
        _roleRepository.Update(role);
        await _roleRepository.SaveChangesAsync();
        
        return role;
    }
    
    public async Task<Role> GetByNameThrowsNotFoundAsync(string roleName)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(r => r.RoleName, roleName);
    }
    
    public async Task<Role?> GetByNameAsync(string roleName)
    {
        return await _genericService.GetFirstAsync(r => r.RoleName, roleName);
    }
}