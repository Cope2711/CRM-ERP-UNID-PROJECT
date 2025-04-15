using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IBranchesManagementService
{ 
    Task<Branch> Create(CreateBranchDto createBranchDto);
    Task<Branch> Update(Guid id, UpdateBranchDto updateBranchDto);
}