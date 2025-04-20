using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class BranchesManagementService(
    IBranchesQueryService _branchesQueryService,
    IBranchesRepository _branchesRepository,
    ILogger<BranchesManagementService> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IUsersBranchesQueryService _usersBranchesQueryService
) : IBranchesManagementService
{
    public async Task<Branch> Create(CreateBranchDto createBranchDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for BranchName {TargetBranchName}",
            authenticatedUserId, createBranchDto.BranchName);
        
        // Check unique camps
        if (await _branchesQueryService.ExistByName(createBranchDto.BranchName)){
            _logger.LogError(
                "User with Id {authenticatedUserId} requested CreateAsync for BranchName {TargetBranchName} but the branchname already exists",
                authenticatedUserId, createBranchDto.BranchName);
            throw new UniqueConstraintViolationException("Branch with this name already exists", Fields.Branches.BranchName);
        }

        // Create branch
        Branch branch = new()
        {
            BranchName = createBranchDto.BranchName,
            BranchAddress = createBranchDto.BranchAddress,
            BranchPhone = createBranchDto.BranchPhone,
            IsActive = createBranchDto.IsActive
        };

        _branchesRepository.Add(branch);

        await _branchesRepository.SaveChangesAsync();

        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested CreateAsync for BranchName {TargetBranchName} and the branch was created",
            authenticatedUserId, createBranchDto.BranchName);
        
        return branch;
    }
    
    public async Task<Branch> Update(Guid id, UpdateBranchDto updateBranchDto)
    {
        Guid authenticatedUserId = HttpContextHelper.GetAuthenticatedUserId(_httpContextAccessor);
        Branch branch = await _branchesQueryService.GetByIdThrowsNotFoundAsync(id);
        await _usersBranchesQueryService.EnsureUserHasAccessToBranch(authenticatedUserId, id);
        _logger.LogInformation(
            "User with Id {authenticatedUserId} requested UpdateAsync for BranchId {TargetBranchId}",
            authenticatedUserId, id);
        
        bool hasChanges = ModelsHelper.UpdateModel(branch, updateBranchDto, async (field, value) =>
        {
            switch (field)
            {
                case nameof(updateBranchDto.BranchName):
                    return await _branchesQueryService.ExistByName((string)value);
                
                default:
                    return false;
            }
        });
        
        if (hasChanges)
        {
            await _branchesRepository.SaveChangesAsync();
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for BranchId {TargetBranchId} and the branch was updated",
                authenticatedUserId, id);
        }
        else
        {
            _logger.LogInformation(
                "User with Id {authenticatedUserId} requested UpdateAsync for BranchId {TargetBranchId} and the branch was not updated",
                authenticatedUserId, id);
        }
        
        return branch;
    }
}