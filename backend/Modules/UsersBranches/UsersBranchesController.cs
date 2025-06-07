using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
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
        GetAllResponseDto<UserBranch> getAllResponseDto = await _usersBranchesQueryService.GetAll(getAllDto);
        
        return Ok(getAllResponseDto);
    }
    
    [HttpPost("assign")]
    [PermissionAuthorize("Assign", "UsersBranches")]
    public async Task<ActionResult<ResponsesDto<ModelAndAssignResponseStatusDto>>> AssignBranch([FromBody] ModelsAndAssignsDtos modelsAndAssignsDtos, [FromQuery] string? modelName)
    {
        if (modelName != null && modelName.ToLower() == "users")
        {
            foreach (var assign in modelsAndAssignsDtos.ModelAssignIds)
            {
                var temp = assign.ModelId;
                assign.ModelId = assign.AssignId;
                assign.AssignId = temp;
            }
        }
        ResponsesDto<ModelAndAssignResponseStatusDto> userBranchResponsesDto = await _usersBranchesManagementService.AssignBranchToUserAsync(modelsAndAssignsDtos);
        return Ok(userBranchResponsesDto);
    }
    
    [HttpDelete("revoke")]
    [PermissionAuthorize("Revoke", "UsersBranches")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> RevokeBranch([FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> userBranchResponsesDto = await _usersBranchesManagementService.RevokeBranchToUser(idsDto);
        return Ok(userBranchResponsesDto);
    }
}