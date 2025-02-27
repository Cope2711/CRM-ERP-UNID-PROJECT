using System.Security.Claims;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesService
{
    Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId);
    
    /// <summary>
    /// Retrieves the user role association for the specified user and role, and throws an exception if not found.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>The user-role association.</returns>
    /// <exception cref="NotFoundException">Thrown when no association for the specified user and role is found.</exception>
    Task<UserRole> AssignRoleToUserAsync(UserAndRoleDto userAndRoleDto);
    
    /// <summary>
    /// Assigns a role to a user using the provided details.
    /// </summary>
    /// <param name="userAndRoleDto">A DTO containing the user and role details.</param>
    /// <returns>The newly created user-role association.</returns>
    Task<UserRole> RevokeRoleToUserAsync(UserAndRoleDto userAndRoleDto);
    
    /// <summary>
    /// Revokes a role from a user using the provided details.
    /// </summary>
    /// <param name="userAndRoleDto">A DTO containing the user and role details.</param>
    /// <returns>The user-role association that was revoked.</returns>
    Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId);
    
    /// <summary>
    /// Checks whether a specific role is assigned to a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier of the role.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains true if the role is assigned to the user; otherwise, false.
    /// </returns>
    Task<GetAllResponseDto<UserRole>> GetAllAsync(GetAllDto getAllDto);
    
    /// <summary>
    /// Retrieves all user-role associations based on the specified filtering and pagination parameters.
    /// </summary>
    /// <param name="getAllDto">A DTO containing filtering and pagination parameters.</param>
    /// <returns>
    /// A response DTO containing the list of user-role associations along with associated metadata.
    /// </returns>
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