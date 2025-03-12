using CRM_ERP_UNID.Constants;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public class PasswordResetService(
    IUsersQueryService _usersQueryService,
    IPasswordResetRepository _passwordResetRepository,
    IMailService _mailService,
    IGenericService<PasswordRecoveryToken> _genericService
) : IPasswordResetService
{
    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        User user = await _usersQueryService.GetByEmailThrowsNotFoundAsync(email);

        var passwordReset = new PasswordRecoveryToken
        {
            UserId = user.UserId,
            ResetToken = GenerateResetToken(),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow
        };

        await _passwordResetRepository.AddAsync(passwordReset);
        await _passwordResetRepository.SaveAsync();

        await SendPasswordResetEmailAsync(user.UserEmail, passwordReset.ResetToken);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        PasswordRecoveryToken passwordReset = await GetByTokenAndEmailThrowsNotFoundAsync(resetPasswordDto.Token, resetPasswordDto.Email);
        if (passwordReset.ExpiresAt < DateTime.UtcNow)
        {
            throw new BadRequestException("Password reset token has expired.", reason: Reasons.ExpiredToken);
        }

        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
        {
            throw new BadRequestException("Passwords do not match.", reason: Reasons.WrongPassword);
        }

        User user = await _usersQueryService.GetByIdThrowsNotFoundAsync(id: passwordReset.UserId);

        user.UserPassword = HasherHelper.HashString(resetPasswordDto.NewPassword);
        user.UpdatedDate = DateTime.UtcNow;
        await _passwordResetRepository.SaveAsync();
        return true;
    }

    public async Task<PasswordRecoveryToken> GetByTokenAndEmailThrowsNotFoundAsync(string token, string email)
    {
        PasswordRecoveryToken? passwordReset = await _passwordResetRepository.GetByTokenAndEmailThrowsNotFoundAsync(token, email);
        if (passwordReset == null)
            throw new NotFoundException("Password reset token or email not found.", field: Fields.PasswordRecoveryTokens.ResetToken);
        return passwordReset;
    }

    public async Task<PasswordRecoveryToken> GetByIdThrowsNotFoundAsync(Guid id)
    {
        return await _genericService.GetByIdThrowsNotFoundAsync(id);
    }

    private string GenerateResetToken()
    {
        return Guid.NewGuid().ToString("N");
    }


    private async Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        var resetLink = $"https://tudominio.com/reset-password?token={resetToken}";
        var emailBody = $@"
            <h1>Restablecimiento de contraseña</h1>
            <p>Para restablecer tu contraseña, haz clic en el siguiente enlace:</p>
            <a href='{resetLink}'>Restablecer contraseña</a>
            <p>Este enlace expirará en 1 hora.</p>
        ";

        await _mailService.SendEmailAsync(email, "Restablecimiento de contraseña", emailBody);
    }
}