using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules.RecoverPassword;

public class PasswordResetService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;

    public PasswordResetService(
        IUsersRepository usersRepository,
        IPasswordResetRepository passwordResetRepository,
        IMailService mailService)
    {
        _usersRepository = usersRepository;
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        if(user == null)
            return false;

        var passwordReset = new PasswordReset
        {
            UserId = user.UserId,
            ResetToken = GenerateResetToken(),
            ResetTokenExpiry = DateTime.UtcNow.AddHours(1),
        };
        
        await _passwordResetRepository.AddAsync(passwordReset);
        await _passwordResetRepository.SaveAsync();
        
        await SendPasswordResetEmailAsync(user.UserEmail, passwordReset.ResetToken);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var passwordReset = await _passwordResetRepository.GetByTokenAsync(token);
        if(passwordReset == null|| passwordReset.ResetTokenExpiry < DateTime.UtcNow)
            return false;

        var user = await _usersRepository.GetByIdAsync(passwordReset.UserId);
        if (user == null)
            return false;

        user.UserPassword = HasherHelper.HashString(newPassword);
        user.UpdatedDate = DateTime.UtcNow;
        await _passwordResetRepository.DeleteAsync(passwordReset);
        await _passwordResetRepository.SaveAsync();
        return true;
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
    
        await _mailService.SendEmailAsync(email, "Restablecimiento de contraseña",emailBody);
    }
}