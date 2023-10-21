using System.Net.Mail;

namespace Common.EmailManager
{
    public interface IEmailManager
    { 
        void SendEmail(string subject, string messageBody, IEnumerable<string> recipients, MailPriority priority = MailPriority.Normal);
    }
}
