using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IExampleManagementService
{
    Task<Example> Create(CreateExampleDto createExampleDto);
    Task<Example> Update(UpdateExampleDto updateExampleDto);
}