using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class PasswordRecoveryTokens
    {
        public static PasswordRecoveryToken TestValidTokenAsync = new PasswordRecoveryToken
        {
            ResetId = Guid.NewGuid(),
            UserId = Models.Users.TestUser.UserId,
            ResetToken = "valid-reset-token",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        public static PasswordRecoveryToken TestExpiredTokenAsync = new PasswordRecoveryToken
        {
            ResetId = Guid.NewGuid(),
            UserId = Models.Users.TestUser.UserId,
            ResetToken = "expired-reset-token",
            ExpiresAt = DateTime.UtcNow.AddHours(-1)
        };
    }
}