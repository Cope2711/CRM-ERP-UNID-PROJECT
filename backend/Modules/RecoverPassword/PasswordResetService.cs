using System.ComponentModel.DataAnnotations;
using CRM_ERP_UNID.Data.Models;
using CRM_ERP_UNID.Exceptions;
using CRM_ERP_UNID.Helpers;

namespace CRM_ERP_UNID.Modules;

public interface IPasswordResetService
{
    Task<bool> RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword, string confirmPassword);
}

public class PasswordResetService : IPasswordResetService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IMailService _mailService;
    private readonly IGenericServie<User> _genericService;
    private readonly IUsersService _usersService;

    public PasswordResetService(
        IUsersRepository usersRepository,
        IPasswordResetRepository passwordResetRepository,
        IMailService mailService,
        IGenericServie<User> genericServie,
        IUsersService usersService
)

{
        _usersRepository = usersRepository;
        _passwordResetRepository = passwordResetRepository;
        _mailService = mailService;
        _genericService = genericServie;
        _usersService = usersService;
    }

    public async Task<bool> RequestPasswordResetAsync(string email)
    {
        var user = await _usersRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"No se encontró un usuario con el email {email}.", field:"email");
        }

        var passwordReset = new PasswordRecoveryToken
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

    public async Task<bool> ResetPasswordAsync(string token, string newPassword, string confirmPassword)
    {

        
        var passwordReset = await _passwordResetRepository.GetByTokenThrowsNotFoundAsync(token);
        if (passwordReset.ResetTokenExpiry < DateTime.UtcNow)
        {
            throw new BadRequestException("Password reset token has expired.", reason:"TokenExpired");
        }
        
        if (newPassword != confirmPassword)
        {
            throw new BadRequestException("Passwords do not match.", reason:"PasswordDoesNotMatch.");
        }

        

        var user = await _usersService.GetByIdThrowsNotFoundAsync(id:passwordReset.UserId);
       


        user.UserPassword = HasherHelper.HashString(newPassword);
        user.UpdatedDate = DateTime.UtcNow;
        await _passwordResetRepository.DeleteAsync(passwordReset);
        await _passwordResetRepository.SaveAsync();
        return true;
    }

    public async Task<User> GetByIdThrowsNotFoundAsync(Guid id)
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