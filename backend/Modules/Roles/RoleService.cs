﻿using System.Security.Claims;
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
    Task<bool> ExistRoleNameAsync(string roleName);
}

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IGenericServie<Role> _genericService;
    private readonly ILogger<RoleService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private Guid AuthenticatedUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   Guid.Empty.ToString());

    public RoleService(IRoleRepository roleRepository, IGenericServie<Role> genericService, ILogger<RoleService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _roleRepository = roleRepository;
        _genericService = genericService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ExistRoleNameAsync(string roleName)
    {
        return await _genericService.ExistsAsync(r => r.RoleName, roleName);
    }
    
    public async Task<Role> DeleteById(Guid id)
    {
        _logger.LogInformation("User with Id {AuthenticatedUserId} requested DeleteById for RoleId {TargetRoleId}",
            AuthenticatedUserId, id);

        Role role = await GetByIdThrowsNotFoundAsync(id);
        _roleRepository.Remove(role);
        await _roleRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested DeleteById for RoleId {TargetRoleId} and the role was deleted",
            AuthenticatedUserId, id);

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
        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName}",
            AuthenticatedUserId, createRoleDto.RoleName);

        // Exist roleName?
        if (await GetByNameAsync(createRoleDto.RoleName) != null)
        {
            _logger.LogError(
                "User with Id {AuthenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName} but the rolename already exists",
                AuthenticatedUserId, createRoleDto.RoleName);

            throw new UniqueConstraintViolationException(
                $"A role with the name '{createRoleDto.RoleName}' already exists.",
                field: "RoleName");
        }

        Role newRole = new Role
        {
            RoleName = createRoleDto.RoleName,
            RoleDescription = createRoleDto.RoleDescription,
        };

        _roleRepository.Add(newRole);
        await _roleRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {AuthenticatedUserId} requested CreateRoleAsync for RoleName {TargetRoleName} and the role was created",
            AuthenticatedUserId, createRoleDto.RoleName);
        
        return newRole;
    }

    public async Task<Role> UpdateAsync(UpdateRoleDto updateRoleDto)
    {
        Role role = await GetByIdThrowsNotFoundAsync(updateRoleDto.RoleId);

        if (updateRoleDto.RoleName != null)
        {
            if (await ExistRoleNameAsync(updateRoleDto.RoleName))
            {
                _logger.LogError(
                    "User with Id {AuthenticatedUserId} requested UpdateAsync for RoleId {TargetRoleId} but the rolename already exists",
                    AuthenticatedUserId, updateRoleDto.RoleId);
                
                throw new UniqueConstraintViolationException(
                    $"A role with the name '{updateRoleDto.RoleName}' already exists.",
                    field: "RoleName");
            }
            
            role.RoleName = updateRoleDto.RoleName;
        }
        
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