using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class RefreshTokens
    {
        public static readonly RefreshToken TestUserRefreshTokenRevoked = new RefreshToken
        {
            userId = Models.Users.TestUser.id,
            token = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            deviceId = "1",
            expiresAt = DateTime.UtcNow.AddHours(1),
            revokedAt = DateTime.UtcNow
        };
        
        public static readonly RefreshToken TestUserExpiredRefreshToken = new RefreshToken
        {
            userId = Models.Users.TestUser.id,
            token = "zVrwFcCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            deviceId = "1",
            expiresAt = DateTime.UtcNow.AddHours(-1),
            revokedAt = null
        };
    }
}