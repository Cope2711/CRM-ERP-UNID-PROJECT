using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Modules;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId, string deviceId);
    Task<RefreshToken> GetRefreshTokenByRefreshTokenThrowsNotFound(string refreshToken);
    Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken);
    Task RevokeRefreshTokensByUserId(Guid userId);
    Task<bool> IsNewDevice(Guid userId, string deviceId);
    Task ValidateNumsOfDevices(Guid userId);
}