using System.Security.Claims;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CRM_ERP_UNID.Modules;

public interface IUsersRolesService
{
    Task<UserRole> GetByUserIdAndRoleIdThrowsNotFoundAsync(Guid userId, Guid roleId);
    Task<UserRole?> GetByUserIdAndRoleId(Guid userId, Guid roleId);
    Task<ResponsesDto<UserAndRoleResponseStatusDto>> AssignRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto);
    Task<ResponsesDto<UserAndRoleResponseStatusDto>> RevokeRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto);
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

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

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
        UserRole? userRole = await GetByUserIdAndRoleId(userId, roleId);
        if (userRole == null)
            throw new NotFoundException("This role is not assigned to the user.", field: "RoleId");
        return userRole;
    }

    public async Task<UserRole?> GetByUserIdAndRoleId(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.GetByUserIdAndRoleIdAsync(userId, roleId);
    }

    public async Task<bool> IsRoleAssignedToUserAsync(Guid userId, Guid roleId)
    {
        return await _usersRolesRepository.IsRoleAssignedToUserAsync(userId, roleId);
    }

    public async Task<ResponsesDto<UserAndRoleResponseStatusDto>> RevokeRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        ResponsesDto<UserAndRoleResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, usersAndRolesDto);

        foreach (UserAndRoleIdDto userAndRoleIdDto in usersAndRolesDto.UserAndRoleId)
        {
            UserRole? userRole = await GetByUserIdAndRoleId(userAndRoleIdDto.UserId, userAndRoleIdDto.RoleId);

            if (userRole == null)
            {
                responseDto.Failed.Add(new UserAndRoleResponseStatusDto
                {
                    UserAndRoleId = userAndRoleIdDto,
                    Status = ResponseStatus.NotFound,
                    Field = "UserRole",
                    Message = "Relation not exists"
                });
                continue;
            }

            _usersRolesRepository.Remove(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new UserAndRoleResponseStatusDto
            {
                UserAndRoleId = userAndRoleIdDto,
                Status = ResponseStatus.Success,
                Message = "Role revoked"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested RevokeRolesToUsers and the result was {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }

    public async Task<ResponsesDto<UserAndRoleResponseStatusDto>> AssignRolesToUsersAsync(UsersAndRolesDtos usersAndRolesDto)
    {
        Guid authenticatedUserId = AuthenticatedUserId;
        ResponsesDto<UserAndRoleResponseStatusDto> responseDto = new();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers with the object {UsersAndRolesDto}",
            authenticatedUserId, usersAndRolesDto);

        foreach (UserAndRoleIdDto userAndRoleIdDto in usersAndRolesDto.UserAndRoleId)
        {
            if (await IsRoleAssignedToUserAsync(userAndRoleIdDto.UserId, userAndRoleIdDto.RoleId))
            {
                responseDto.Failed.Add(new UserAndRoleResponseStatusDto
                {
                    UserAndRoleId = userAndRoleIdDto,
                    Status = ResponseStatus.AlreadyProcessed,
                    Message = "Role already assigned to user"
                });
                continue;
            }

            if (!await _userService.ExistById(userAndRoleIdDto.UserId))
            {
                responseDto.Failed.Add(new UserAndRoleResponseStatusDto
                {
                    UserAndRoleId = userAndRoleIdDto,
                    Status = ResponseStatus.NotFound,
                    Field = "UserId",
                    Message = "User not exist"
                });
                continue;
            }

            if (!await _roleService.ExistById(userAndRoleIdDto.RoleId))
            {
                responseDto.Failed.Add(new UserAndRoleResponseStatusDto
                {
                    UserAndRoleId = userAndRoleIdDto,
                    Status = ResponseStatus.NotFound,
                    Field = "RoleId",
                    Message = "Role not exist"
                });
                continue;
            }

            // Add to database
            UserRole userRole = new UserRole
            {
                UserId = userAndRoleIdDto.UserId,
                RoleId = userAndRoleIdDto.RoleId
            };

            _usersRolesRepository.Add(userRole);
            await _usersRolesRepository.SaveChangesAsync();

            responseDto.Success.Add(new UserAndRoleResponseStatusDto
            {
                UserAndRoleId = userAndRoleIdDto,
                Status = ResponseStatus.Success,
                Message = "RoleAssigned"
            });
        }

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested AssignRolesToUsers and the result was: {responseDto}",
            authenticatedUserId, responseDto);

        return responseDto;
    }
}