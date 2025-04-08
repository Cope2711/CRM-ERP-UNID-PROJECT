using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
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
    [HttpGet("get-create-schema")]
    [PermissionAuthorize("View", "Categories")]
    public async Task<ActionResult> GetCreateSchema()
    {
        return Ok(DtoSchemaHelper.GetDtoSchema<CreateCategoryDto>());
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
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Category));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Category));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Category));

        GetAllResponseDto<Category> getAllResponseDto = await categoriesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Categories")]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto createCategoryDto)
    {
        Category category = await _categoriesManagementService.Create(createCategoryDto);

        return Ok(category.ToDto());
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Categories")]
    public async Task<ActionResult<CategoryDto>> Update([FromBody] UpdateCategoryDto updateCategoryDto)
    {
        Category category = await _categoriesManagementService.Update(updateCategoryDto);

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