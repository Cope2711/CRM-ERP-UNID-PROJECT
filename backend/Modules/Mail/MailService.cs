namespace CRM_ERP_UNID.Modules;

public class MailService : IMailService
{
    public List<(string Email, string Subject, string Message)> SentEmails { get; } = new();
    
    public MailService()
    {
        
    }

    public async Task SendNewDeviceLoggedInMailAsync()
    {
        return;
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        // Simula el envío de un correo electrónico
        SentEmails.Add((to, subject, body));
        return Task.CompletedTask;
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