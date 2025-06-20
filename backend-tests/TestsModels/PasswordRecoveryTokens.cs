using CRM_ERP_UNID.Data.Models;

namespace CRM_ERP_UNID_TESTS.TestsModels;

public static partial class Models
{
    public static class PasswordRecoveryTokens
    {
        public static PasswordRecoveryToken TestValidTokenAsync = new PasswordRecoveryToken
        {
            id = Guid.NewGuid(),
            userId = Models.Users.TestUser.id,
            resetToken = "valid-reset-token",
            expiresAt = DateTime.UtcNow.AddDays(1)
        };

        public static PasswordRecoveryToken TestExpiredTokenAsync = new PasswordRecoveryToken
        {
            id = Guid.NewGuid(),
            userId = Models.Users.TestUser.id,
            resetToken = "expired-reset-token",
            expiresAt = DateTime.UtcNow.AddHours(-1)
        };
    }
}