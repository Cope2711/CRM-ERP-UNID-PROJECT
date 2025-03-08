﻿using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID.Controllers;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<RefreshToken> GenerateAndStoreRefreshTokenAsync(Guid userId);
    Task<RefreshToken?> GetRefreshTokenByRefreshToken(string refreshToken);
    Task<RefreshToken> RevokeRefreshTokenByObject(RefreshToken refreshToken);
}