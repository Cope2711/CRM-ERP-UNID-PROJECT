using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/users-branches")]
public class UsersBranchesController(
    IUsersBranchesQueryService _usersBranchesQueryService,
    IUsersBranchesManagementService _usersBranchesManagementService
) : ControllerBase
{
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "UsersBranches")]
    public async Task<ActionResult<GetAllResponseDto<UserBranch>>> GetAll(GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(UserBranch));
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(UserBranch));
        
        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(UserBranch));
        
        GetAllResponseDto<UserBranch> getAllResponseDto = await _usersBranchesQueryService.GetAll(getAllDto);
        
        return Ok(getAllResponseDto);
    }
    
    [HttpPost("assign-branch")]
    [PermissionAuthorize("Assign", "UsersBranches")]
    public async Task<ActionResult<ResponsesDto<UserBranchResponseStatusDto>>> AssignBranch([FromBody] UsersAndBranchesDtos usersAndBranchesDtos)
    {
        ResponsesDto<UserBranchResponseStatusDto> userBranchResponsesDto = await _usersBranchesManagementService.AssignBranchToUserAsync(usersAndBranchesDtos);
        return Ok(userBranchResponsesDto);
    }
    
    [HttpDelete("revoke-branch")]
    [PermissionAuthorize("Revoke", "UsersBranches")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokeBranch([FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> userBranchResponsesDto = await _usersBranchesManagementService.RevokeBranchToUserAsync(idsDto);
        return Ok(userBranchResponsesDto);
    }
}