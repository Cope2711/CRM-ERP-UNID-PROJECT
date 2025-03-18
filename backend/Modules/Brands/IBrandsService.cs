using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IBrandsService
{
    Task<Brand> GetByIdThrowsNotFound(Guid brandId);
    Task<Brand> GetByNameThrowsNotFound(string brandName);
    Task<bool> ExistByIdThrowsNotFound(Guid brandId);
    Task<GetAllResponseDto<Brand>> GetAll(GetAllDto getAllDto);
    Task<Brand> Create(CreateBrandDto createBrandDto);
    Task<Brand> Update(UpdateBrandDto updateBrandDto);
}