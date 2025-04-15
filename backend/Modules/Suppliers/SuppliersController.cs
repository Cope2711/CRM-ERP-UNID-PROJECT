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
[Route("api/suppliers")]
public class SupplierController(
    ISuppliersQueryService _suppliersQueryService,
    ISuppliersManagementService _suppliersManagementService
) : ControllerBase
{
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Suppliers")]
    public IActionResult GetSchema([FromQuery] string type)
    {
        if (!Utils.ValidSchemaTypes.Contains(type.ToLower()))
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");
        
        var dtoType = type.ToLower() switch
        {
            "create" => typeof(CreateSupplierDto),
            "update" => typeof(UpdateSupplierDto),
            "model" or "read" => typeof(SupplierDto),
            _ => null
        };

        if (dtoType == null)
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");

        return Ok(DtoSchemaHelper.GetDtoSchema(dtoType));
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Suppliers")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid id)
    {
        Supplier supplier = await _suppliersQueryService.GetByIdThrowsNotFoundAsync(id);
        return Ok(supplier.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Suppliers")]
    public async Task<ActionResult<GetAllResponseDto<Supplier>>> GetAll(GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Supplier));
        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Supplier));
        
        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Supplier));
        
        GetAllResponseDto<Supplier> getAllResponseDto = await _suppliersQueryService.GetAll(getAllDto);
        
        return Ok(getAllResponseDto);
    }
    
    [HttpPatch("update/{id}")]
    [PermissionAuthorize("Edit_Content", "Suppliers")]
    public async Task<ActionResult<SupplierDto>> UpdateSupplier(Guid id, [FromBody] UpdateSupplierDto updateSupplierDto)
    {
        Supplier supplier = await _suppliersManagementService.Update(id, updateSupplierDto);
        return Ok(supplier.ToDto());
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Suppliers")]
    public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] CreateSupplierDto createSupplierDto)
    {
        Supplier supplier = await _suppliersManagementService.Create(createSupplierDto);
        
        return Ok(supplier.ToDto());
    }
}