using Newtonsoft.Json;
using System;

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

        public string ToCnArcherJSON()
        {
            int isPPPoE = IsThisPppoeRadio(Int32.Parse(VLAN), Package) ? 1 : 0;

            StagingConfig stagingConfig = new StagingConfig()
            {
                userParameters = new UserParameters()
                {
                    networkConfig = new NetworkConfig()
                    {
                        packetFilterPppoe = 0,
                        packetFilterArp = isPPPoE,
                        packetFilterUser1 = isPPPoE,
                        packetFilterSnmpIpv6 = isPPPoE,
                        packetFilterSmb = isPPPoE,
                        packetFilterOtherIpv4 = isPPPoE,
                        packetFilterAllOthers = isPPPoE,
                        packetFilterBootpServer = isPPPoE,
                        packetFilterBootpClientIpv6 = isPPPoE,
                        packetFilterMulticastIpv6 = isPPPoE,
                        packetFilterAllIpv6Others = isPPPoE,
                        packetFilterSnmp = isPPPoE,
                        packetFilterUser3 = isPPPoE,
                        packetFilterSmbIpv6 = isPPPoE,
                        packetFilterAllIpv4 = isPPPoE,
                        packetFilterBootpServerIpv6 = isPPPoE,
                        packetFilterUser2 = isPPPoE,
                        packetFilterAllIpv6 = isPPPoE,
                        packetFilterBootpClient = isPPPoE,
                        packetFilterMulticastIpv4 = isPPPoE,
                        packetFilterBPDU = isPPPoE,
                        packetFilterDirection = isPPPoE
                    }
                }
            };

            string singleLineAddress = Address.Replace("\n", ",");
            QRContent qrContent = new QRContent()
            {
                type = "cnArcherWO",
                product = "canopy",
                id = EIP,
                name = Name,
                address = singleLineAddress,
                phone = Phones.Split(',')[0],
                mode = "automated",
                sm_name = $"{Name} ({EIP})",
                security = "aaa",
                aaa_user_type = "mac_hyphen",
                aaa_pass = "",
                aaa_phase1 = "eapMSChapV2",
                aaa_phase2 = "mschapv2",
                aaa_identity = "anonymous",
                aaa_realm = "canopy.net",
                nat = false,
                ip_setting = "dhcp",
                comments = $"Install {Date} by {Tech}",
                firmware = Firmware,
                staging_config = JsonConvert.SerializeObject(stagingConfig)
            };

            return JsonConvert.SerializeObject(qrContent);
        }

        private bool IsThisPppoeRadio(int vlan, string packageName)
        {
            return vlan == 20 || packageName.ToLower().Contains("pppoe");
        }
    }
}
