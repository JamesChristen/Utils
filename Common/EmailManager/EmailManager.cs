using Common.Logging;
using System.Net.Mail;

namespace Common.EmailManager
{
    public class EmailManager : IEmailManager
    {
        private readonly ILog _logger;
        private readonly SmtpSettings _smtpSettings;

        public EmailManager(
            ILog logger)
            : this(logger, Configuration.ConfigurationProvider.GetConfigObject<SmtpSettings>("SmtpSettings"))
        {
        }

        public EmailManager(
            ILog logger, 
            SmtpSettings settings)
        {
            _logger = logger;
            _smtpSettings = settings;
        }

        public void SendEmail(string subject, string messageBody, IEnumerable<string> recipients, MailPriority priority = MailPriority.Normal)
        {
            if (recipients == null || !recipients.Any())
            {
                return;
            }

            try
            {
                using SmtpClient smtpClient = _smtpSettings.CreateClient();
                using MailMessage message = new();

                message.From = new MailAddress(_smtpSettings.UserName);
                message.Priority = priority;
                foreach (string recipient in recipients)
                {
                    message.To.Add(new MailAddress(recipient));
                }
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = messageBody;
                
                smtpClient.Send(message);
            }
            catch (Exception ex) 
            {
                _logger.Error(ex.Message);
            };
        }
    }
}
