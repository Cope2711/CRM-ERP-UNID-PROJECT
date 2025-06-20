using System.Text.Json;
using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/categories")]
public class CategoriesController(
    ICategoriesManagementService _categoriesManagementService,
    ICategoriesQueryService categoriesQueryService
) : ControllerBase
{
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Roles")]
    public IActionResult GetSchema([FromQuery] bool ignoreRequired = false)
    {
        return Ok(DtoSchemaHelper.GetDtoSchema(typeof(Category), ignoreRequired));
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Categories")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById([FromQuery] Guid id)
    {
        Category category = await categoriesQueryService.GetByIdThrowsNotFound(id);

        return Ok(category.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Categories")]
    public async Task<ActionResult<GetAllResponseDto<Category>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        GetAllResponseDto<Category> getAllResponseDto = await categoriesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Categories")]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] Category data)
    {
        Category category = await _categoriesManagementService.Create(data);

        return Ok(category.ToDto());
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Categories")]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, [FromBody] JsonElement data)
    {
        Category category = await _categoriesManagementService.Update(id, data);

        return Ok(category.ToDto());
    }
    
    [HttpDelete("delete")]
    [PermissionAuthorize("Delete", "Categories")]
    public async Task<ActionResult<CategoryDto>> Delete([FromQuery] Guid id)
    {
        Category category = await _categoriesManagementService.Delete(id);

        return Ok(category.ToDto());
    }
}