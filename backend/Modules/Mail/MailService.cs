namespace CRM_ERP_UNID.Modules;

public interface IMailService
{
    Task SendRecoverPasswordMailAsync(string email, string token);
    
    /// <summary>
    /// Sends an email to recover a user's password.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="token">The token used for password recovery.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendReactivateAccountMailAsync(string email);
    
    /// <summary>
    /// Sends an email to reactivate a user's account.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    
}


public class MailService : IMailService
{

    public MailService()
    {
        
    }
    
    public async Task SendRecoverPasswordMailAsync(string email, string token)
    {
        return;
    }
    
    public async Task SendReactivateAccountMailAsync(string email)
    {
        return;
    }
}