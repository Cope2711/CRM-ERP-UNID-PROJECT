using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/brands")]
public class BrandsController(
    IBrandsService brandsService
) : ControllerBase
{
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Brands")]
    public async Task<ActionResult<BrandDto>> GetById([FromQuery] Guid id)
    {
        Brand brand = await brandsService.GetByIdThrowsNotFound(id);

        return Ok(brand.ToDto());
    }
    
    [HttpGet("get-by-name")]
    [PermissionAuthorize("View", "Brands")]
    public async Task<ActionResult<BrandDto>> GetByName([FromQuery] string name)
    {
        Brand brand = await brandsService.GetByNameThrowsNotFound(name);

        return Ok(brand.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Brands")]
    public async Task<ActionResult<GetAllResponseDto<Brand>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Brand));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Brand));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Brand));

        GetAllResponseDto<Brand> getAllResponseDto = await brandsService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Brands")]
    public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandDto createBrandDto)
    {
        Brand brand = await brandsService.Create(createBrandDto);

        return Ok(brand.ToDto());
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Brands")]
    public async Task<ActionResult<BrandDto>> Update([FromBody] UpdateBrandDto updateBrandDto)
    {
        Brand brand = await brandsService.Update(updateBrandDto);

        return Ok(brand.ToDto());
    }
}