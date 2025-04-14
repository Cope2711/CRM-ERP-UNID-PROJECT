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
[Route("api/inventory")]
public class InventoryController(
    IInventoryManagementService _inventoryManagementService,
    IInventoryQueryService _inventoryQueryService
) : ControllerBase
{
    [HttpGet("schema")]
    [PermissionAuthorize("View", "Inventory")]
    public IActionResult GetSchema([FromQuery] string type)
    {
        if (!Utils.ValidSchemaTypes.Contains(type.ToLower()))
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");
        
        var dtoType = type.ToLower() switch
        {
            "create" => typeof(CreateInventoryDto),
            "update" => typeof(UpdateInventoryDto),
            "model" or "read" => typeof(InventoryDto),
            _ => null
        };

        if (dtoType == null)
            throw new BadRequestException(message: "Invalid schema type requested.", field: "type");

        return Ok(DtoSchemaHelper.GetDtoSchema(dtoType));
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Inventory")]
    public async Task<ActionResult<InventoryDto>> GetInventoryById([FromQuery] Guid id)
    {
        Inventory inventory = await _inventoryQueryService.GetByIdThrowsNotFoundAsync(id);

        return Ok(inventory.ToDto());
    }
    
    [HttpGet("get-by-productId")]
    [PermissionAuthorize("View", "Inventory")]
    public async Task<ActionResult<InventoryDto>> GetInventoryByProductId([FromQuery] Guid productId, [FromQuery] Guid branchId)
    {
        Inventory inventory = await _inventoryQueryService.GetByProductIdInBranchIdThrowsNotFound(productId, branchId);

        return Ok(inventory.ToDto());
    }
    
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Inventory")]
    public async Task<ActionResult<GetAllResponseDto<Inventory>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        if (getAllDto.OrderBy != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.OrderBy, typeof(Inventory));

        if (getAllDto.Filters != null)
            CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Filters, typeof(Inventory));

        CustomValidators.ValidateModelContainsColumnsNames(getAllDto.Selects, typeof(Inventory));

        GetAllResponseDto<Inventory> getAllResponseDto = await _inventoryQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpPost("create")]
    [PermissionAuthorize("Create", "Inventory")]
    public async Task<ActionResult<InventoryDto>> Create([FromBody] CreateInventoryDto createInventoryDto)
    {
        Inventory inventory = await _inventoryManagementService.Create(createInventoryDto);

        return Ok(inventory.ToDto());
    }
    
    [HttpPatch("update")]
    [PermissionAuthorize("Edit_Content", "Inventory")]
    public async Task<ActionResult<InventoryDto>> Update([FromBody] UpdateInventoryDto updateInventoryDto)
    {
        Inventory inventory = await _inventoryManagementService.Update(updateInventoryDto);

        return Ok(inventory.ToDto());
    }
}