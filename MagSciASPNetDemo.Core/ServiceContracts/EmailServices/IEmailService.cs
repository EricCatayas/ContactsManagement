namespace ContactsManagement.Core.ServiceContracts.EmailServices
{
    public interface IEmailService
    {
        public Task<bool> SendEmail(string recipient, string subject, string body);
    }
}
