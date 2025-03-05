using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Dtos;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IPasswordResetService
{
    Task<bool> RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}

public class PasswordResetService : IPasswordResetService
{
   
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;
    private readonly IGenericServie<PasswordRecoveryToken> _genericService;
    private readonly IUsersService _usersService;

    public PasswordResetService(
        IPasswordResetRepository passwordResetRepository,
        IMailService mailService,
        IGenericServie<PasswordRecoveryToken> genericServie,
        IUsersService usersService
)
{
        
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
        _genericService = genericServie;
        _usersService = usersService;
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        var user = await _usersService.GetByEmailThrowsNotFoundAsync(email);

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
        var passwordReset = await GetByTokenThrowsNotFoundAsync(resetPasswordDto.Token);
        if (passwordReset.ExpiresAt < DateTime.UtcNow)
        {
            throw new BadRequestException("Password reset token has expired.", reason:"TokenExpired");
        }
        
        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
        {
            throw new BadRequestException("Passwords do not match.", reason:"PasswordDoesNotMatch.");
        }
        
        var user = await _usersService.GetByIdThrowsNotFoundAsync(id:passwordReset.UserId);
        
        user.UserPassword = HasherHelper.HashString(resetPasswordDto.NewPassword);
        user.UpdatedDate = DateTime.UtcNow;
        await _passwordResetRepository.SaveAsync();
        return true;
    }

    public async Task<PasswordRecoveryToken> GetByTokenThrowsNotFoundAsync(string token)
    {
        return await _genericService.GetFirstThrowsNotFoundAsync(prt => prt.ResetToken, token);
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
    
        await _mailService.SendEmailAsync(email, "Restablecimiento de contraseña",emailBody);
    }
}