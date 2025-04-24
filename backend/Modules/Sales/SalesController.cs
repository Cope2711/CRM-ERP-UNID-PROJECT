using CRM_ERP_UNID.Attributes;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM_ERP_UNID.Modules;

[ApiController]
[Authorize]
[Route("api/sales")]
public class SalesController(
    ISalesQueryService _salesQueryService,
    ISalesManagementService _salesManagementService
    )
    : ControllerBase
{
    [HttpPost("get-all")]
    [PermissionAuthorize("View", "Sales")]
    public async Task<ActionResult<GetAllResponseDto<Sale>>> GetAll([FromBody] GetAllDto getAllDto)
    {
        GetAllResponseDto<Sale> getAllResponseDto = await _salesQueryService.GetAll(getAllDto);

        return Ok(getAllResponseDto);
    }
    
    [HttpGet("get-by-id")]
    [PermissionAuthorize("View", "Sales")]
    public async Task<ActionResult<SaleDto>> GetById([FromQuery] Guid id)
    {
        Sale sale = await _salesQueryService.GetByIdThrowsNotFound(id);
        return Ok(sale.ToDto());
    }

    [HttpPost("create")]
    [PermissionAuthorize("Create", "Sales")]
    public async Task<ActionResult<SaleDto>> Create([FromBody] CreateFullSaleDto createFullSaleDto)
    {
        Sale sale = await _salesManagementService.Create(createFullSaleDto);
        return Ok(sale.ToDto());
    }

    [HttpDelete("delete")]
    [PermissionAuthorize("Delete", "Sales")]
    public async Task Delete([FromQuery] Guid id)
    {
        await _salesManagementService.Delete(id);
    }
}