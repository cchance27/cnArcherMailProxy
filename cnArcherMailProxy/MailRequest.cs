namespace cnArcherMailProxy
{
    public class MailRequest
    {
        public string Name;
        public string Date;
        public string Tech;
        public string TechEmail;
        public string ESN;
        public string Company;
        public string EIP;
        public string Account;
        public string Phones;
        public string Address;
        public string Username;
        public string Password;
        public string VLAN;
        public string Notes;
        public string Firmware;
        public string Package;

        public string ToCnArcherJSON()
        {
            var singleLineAddress = Address.Replace("\n", ",");

            string output = "";
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
                $"    \"firmware\": \"{Firmware}\"" +
                $"}}";
            return output;
        }
    }
}
