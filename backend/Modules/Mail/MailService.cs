namespace CRM_ERP_UNID.Modules;

public interface IMailService
{
    Task SendRecoverPasswordMailAsync(string email, string token);
    Task SendReactivateAccountMailAsync(string email);
    Task SendNewDeviceLoggedInMailAsync();
}

public class MailService : IMailService
{

    public MailService()
    {
        
    }

    public async Task SendNewDeviceLoggedInMailAsync()
    {
        return;
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