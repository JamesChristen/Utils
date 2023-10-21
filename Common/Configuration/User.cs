using System.Security.Principal;

namespace Common.Configuration
{
    public static class User
    {
        public static string Sid => WindowsIdentity.GetCurrent().User.ToString();

        public static string IpAddress => System.Net.Dns.GetHostEntry(HostName).AddressList[0].ToString();

        public static string HostName => System.Net.Dns.GetHostName();
    }
}
