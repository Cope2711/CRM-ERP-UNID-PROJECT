using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface ISuppliersManagementService
{
    Task<Supplier> Create(CreateSupplierDto createSupplierDto);
    Task<Supplier> Update(UpdateSupplierDto updateSupplierDto);
}