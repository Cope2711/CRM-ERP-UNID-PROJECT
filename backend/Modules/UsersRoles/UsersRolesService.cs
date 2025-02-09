using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesService
{
    Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId);
    Task<UserRole> AssignRoleToUserAsync(UserAndRoleDto userAndRoleDto);
    Task<UserRole> RevokeRoleToUserAsync(UserAndRoleDto userAndRoleDto);
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto);
}

public class UsersRolesService : IUsersRolesService
{
    private readonly IUsersRolesRepository _usersRolesRepository;
    private readonly IRoleService _roleService;
    private readonly IUsersService _userService;
    private readonly IGenericServie<UserRole> _genericService;
    
    public UsersRolesService(IUsersRolesRepository usersRolesRepository, IRoleService roleService, IUsersService userService, IGenericServie<UserRole> genericService)
    {
        _usersRolesRepository = usersRolesRepository;
        _roleService = roleService;
        _userService = userService;
        _genericService = genericService;
    }
    
    public async Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto, query => query.Include(ur => ur.User).Include(ur => ur.Role));
    }

    public async Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId)
    {
        UserRole? userRole = await _usersRolesRepository.GetByUserIdAndRoleIdAsync(userId, roleId);
        if (userRole == null)
        {
            throw new NotFoundException("This role is not assigned to the user.", field: "RoleId");
        }
        
        return userRole;
    }
    
    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.IsRoleAssignedToUserAsync(userId, roleId);
    }
    
    public async Task<UserRole> RevokeRoleToUserAsync(UserAndRoleDto userAndRoleDto)
    {
        // Is already assigned?
        UserRole userRole = await GetByUserIdAndRoleIdThrowsNotFoundAsync(userAndRoleDto.UserId, userAndRoleDto.RoleId);
        
        // Remove from database
        _usersRolesRepository.Remove(userRole);
        await _usersRolesRepository.SaveChangesAsync();
        
        return userRole;
    }
    
    public async Task<UserRole> AssignRoleToUserAsync(UserAndRoleDto userAndRoleDto)
    {
        // Is already assigned?
        if (await IsRoleAssignedToUserAsync(userAndRoleDto.UserId, userAndRoleDto.RoleId))
        {
            throw new UniqueConstraintViolationException("This role is already assigned to the user.",
                field: "RoleId");
        }
        
        // Exist source and target?
        await _userService.GetByIdThrowsNotFoundAsync(userAndRoleDto.UserId);
        await _roleService.GetByIdThrowsNotFoundAsync(userAndRoleDto.RoleId);
        
        // Add to database
        UserRole userRole = new UserRole
        {
            UserId = userAndRoleDto.UserId,
            RoleId = userAndRoleDto.RoleId
        };
        
        _usersRolesRepository.Add(userRole);
        await _usersRolesRepository.SaveChangesAsync();
        
        return userRole;
    }
}