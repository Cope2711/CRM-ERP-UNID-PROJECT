using CRM_ERP_UNID.Dtos;

namespace CRM_ERP_UNID.Modules;

public interface IPasswordResetService
{
    Task<bool> RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}