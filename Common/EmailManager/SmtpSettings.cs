using System.Net;
using System.Net.Mail;

namespace Common.EmailManager
{
    public class SmtpSettings
    {
        public int Port { get; set; } = 2525;
        public string Host { get; set; } = "mail.smtp2go.com";
        public bool EnableSsl { get; set; } = true;
        public bool UseDefaultCredentials { get; set; } = false;
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
        public string UserName { get; set; } = "adg@adgcorporate.com";
        public string Password { get; set; } = "Chiswell01!";

        public override string ToString()
        {
            return $"{UserName} :: {Host}:{Port}";
        }

        public SmtpClient CreateClient()
        {
            return new SmtpClient()
            {
                Port = Port,
                Host = Host,
                EnableSsl = EnableSsl,
                UseDefaultCredentials = UseDefaultCredentials,
                Credentials = new NetworkCredential(UserName, Password),
                DeliveryMethod = DeliveryMethod
            };
        }
    }
}
