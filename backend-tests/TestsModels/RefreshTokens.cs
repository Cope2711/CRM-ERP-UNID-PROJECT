using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class RefreshTokens
    {
        public static readonly RefreshToken TestUserRefreshTokenRevoked = new RefreshToken
        {
            UserId = Models.Users.TestUser.UserId,
            Token = "zVrwFaCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            DeviceId = "1",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            RevokedAt = DateTime.UtcNow
        };
        
        public static readonly RefreshToken TestUserExpiredRefreshToken = new RefreshToken
        {
            UserId = Models.Users.TestUser.UserId,
            Token = "zVrwFcCSNYH12C2a3jbb/ejmKloVSnJgYwJNeQsW/xs=",
            DeviceId = "1",
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            RevokedAt = null
        };
    }
}