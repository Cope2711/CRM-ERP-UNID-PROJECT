namespace CRM_ERP_UNID.Modules;

public interface IMailService
{
    Task SendRecoverPasswordMailAsync(string email, string token);
    Task SendReactivateAccountMailAsync(string email);
    Task SendNewDeviceLoggedInMailAsync();
    Task SendEmailAsync(string to, string subject, string body);
}
