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
[Route("api/brands")]
public class BrandsController(
    IBrandsService _brandsService
) : ControllerBase
{
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Brands")]
    public IActionResult GetSchema([FromQuery] string type)
    {
        if (!Utils.ValidSchemaTypes.Contains(type.ToLower()))
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");
        
        var dtoType = type.ToLower() switch
        {
            "create" => typeof(CreateBrandDto),
            "update" => typeof(UpdateBrandDto),
            "model" or "read" => typeof(BrandDto),
            _ => null
        };

        if (dtoType == null)
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");

        return Ok(DtoSchemaHelper.GetDtoSchema(dtoType));
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Brands")]
    public async Task<ActionResult<BrandDto>> GetById([FromQuery] Guid id)
    {
        Brand brand = await _brandsService.GetByIdThrowsNotFound(id);

        return Ok(brand.ToDto());
    }
    
    [HttpGet("get-by-name")]
    [PermissionAuthorize("View", "Brands")]
    public async Task<ActionResult<BrandDto>> GetByName([FromQuery] string name)
    {
        Brand brand = await _brandsService.GetByNameThrowsNotFound(name);

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

        GetAllResponseDto<Brand> getAllResponseDto = await _brandsService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Brands")]
    public async Task<ActionResult<BrandDto>> Create([FromBody] CreateBrandDto createBrandDto)
    {
        Brand brand = await _brandsService.Create(createBrandDto);

        return Ok(brand.ToDto());
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Brands")]
    public async Task<ActionResult<BrandDto>> Update(Guid id, [FromBody] UpdateBrandDto updateBrandDto)
    {
        Brand brand = await _brandsService.Update(id, updateBrandDto);

        return Ok(brand.ToDto());
    }
    
    [HttpPatch("deactivate")]
    [PermissionAuthorize("Deactivate", "Brands")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Deactivate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> deactivateResponseDto =
            await _brandsService.Deactivate(idsDto);
        return Ok(deactivateResponseDto);
    }

    [HttpPatch("activate")]
    [PermissionAuthorize("Activate", "Brands")]
    public async Task<ActionResult<ResponsesDto<IdResponseStatusDto>>> Activate(
        [FromBody] IdsDto idsDto)
    {
        ResponsesDto<IdResponseStatusDto> activateResponseDto =
            await _brandsService.Activate(idsDto);
        return Ok(activateResponseDto);
    }
}