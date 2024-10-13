using System.Net;

namespace Parceldropweb
{
    public class Audit
    {
        public static string GetIPAddress()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            string IP4Address = String.Empty;
            foreach (IPAddress curAdd in heserver.AddressList)
            {
                if (curAdd.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = curAdd.ToString();
                    break;
                }
            }
            if (IP4Address != String.Empty)
            {
                return IP4Address;
            }

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }
    }
}
