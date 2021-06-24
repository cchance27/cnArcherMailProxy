using System;
using System.Web.SessionState;

namespace cnArcherMailProxy
{
    public class MailRequest
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Tech { get; set; }
        public string TechEmail { get; set; }
        public string ESN { get; set; }
        public string Company { get; set; }
        public string EIP { get; set; }
        public string Account { get; set; }
        public string Phones { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VLAN { get; set; }
        public string Notes { get; set; }
        public string Firmware { get; set; }
        public string Package { get; set; }

    public string ToCnArcherJSON(HttpSessionState session)
        {
            var singleLineAddress = Address.Replace("\n", ",");


            // TODO: Proper serialization instead of hand writing
            string output = "";
            if (session == null)
            {
                throw new UnauthorizedAccessException();
            }

            output = 
                $"{{ "+
                $"    \"type\":\"cnArcherWO\"," +
                $"    \"product\": \"canopy\"," +
                $"    \"id\": \"{EIP}\"," +
                $"    \"name\": \"{Name}\"," +
                $"    \"address\": \"{singleLineAddress}\"," +
                $"    \"phone\": \"{Phones.Split(',')[0]}\"," +
                $"    \"mode\": \"automated\"," +
                $"    \"sm_name\": \"{Name} ({EIP})\"," +
                $"    \"security\": \"aaa\"," +
                $"    \"aaa_user_type\": \"mac_hyphen\"," +
                $"    \"aaa_pass\": \"\"," +
                $"    \"aaa_phase1\": \"eapMSChapv2\"," +
                $"    \"aaa_phase2\": \"mschapv2\"," +
                $"    \"aaa_identity\": \"anonymous\"," +
                $"    \"aaa_realm\": \"canopy.net\"," +
                $"    \"nat\": false," +
                $"    \"ip_setting\": \"dhcp\"," +
                $"    \"comments\": \"Install {Date} by {Tech}\", " +
                $"    \"firmware\": \"{Firmware}\", " +
                $"    \"sessionInfo\": \"{session.Keys.ToString()}\"" +
                $"}}";
            return output;
        }
    }
}
