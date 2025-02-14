using System.Security.Claims;
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
    private readonly ILogger<UsersRolesService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid AuthenticatedUserId => Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
    
    public UsersRolesService(IUsersRolesRepository usersRolesRepository, IRoleService roleService,
        IUsersService userService, IGenericServie<UserRole> genericService, ILogger<UsersRolesService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _usersRolesRepository = usersRolesRepository;
        _roleService = roleService;
        _userService = userService;
        _genericService = genericService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto)
    {
        return await _genericService.GetAllAsync(getAllDto,
            query => query.Include(ur => ur.User).Include(ur => ur.Role));
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
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRoleToUser for UserId {TargetUserId} and RoleId {TargetRoleId}", AuthenticatedUserId, userAndRoleDto.UserId, userAndRoleDto.RoleId);
        
        // Is already assigned?
        UserRole userRole = await GetByUserIdAndRoleIdThrowsNotFoundAsync(userAndRoleDto.UserId, userAndRoleDto.RoleId);

        // Remove from database
        _usersRolesRepository.Remove(userRole);
        await _usersRolesRepository.SaveChangesAsync();

        _logger.LogInformation("User with Id {AuthenticatedUserId} requested RevokeRoleToUser for UserId {TargetUserId} and RoleId {TargetRoleId} and the role was revoked", AuthenticatedUserId, userAndRoleDto.UserId, userAndRoleDto.RoleId);
        
        return userRole;
    }

    public async Task<UserRole> AssignRoleToUserAsync(UserAndRoleDto userAndRoleDto)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested AssignRoleToUser for UserId {TargetUserId} and RoleId {TargetRoleId}", AuthenticatedUserId, userAndRoleDto.UserId, userAndRoleDto.RoleId);
        
        // Is already assigned?
        if (await IsRoleAssignedToUserAsync(userAndRoleDto.UserId, userAndRoleDto.RoleId))
        {
            _logger.LogInformation("User with Id {AuthenticatedUserId} requested AssignRoleToUser for UserId {TargetUserId} and RoleId {TargetRoleId} but the role is already assigned", AuthenticatedUserId, userAndRoleDto.UserId, userAndRoleDto.RoleId);
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

        _logger.LogInformation("User with Id {AuthenticatedUserId} requested AssignRoleToUser for UserId {TargetUserId} and RoleId {TargetRoleId} and the role was assigned", AuthenticatedUserId, userAndRoleDto.UserId, userAndRoleDto.RoleId);
        
        return userRole;
    }
}