namespace cnArcherMailProxy
{
    public class NetworkConfig
    {
        public int packetFilterPppoe { get; set; }
        public int packetFilterArp { get; set; }
        public int packetFilterUser1 { get; set; }
        public int packetFilterSnmpIpv6 { get; set; }
        public int packetFilterSmb { get; set; }
        public int packetFilterOtherIpv4 { get; set; }
        public int packetFilterAllOthers { get; set; }
        public int packetFilterBootpServer { get; set; }
        public int packetFilterBootpClientIpv6 { get; set; }
        public int packetFilterMulticastIpv6 { get; set; }
        public int packetFilterAllIpv6Others { get; set; }
        public int packetFilterSnmp { get; set; }
        public int packetFilterUser3 { get; set; }
        public int packetFilterSmbIpv6 { get; set; }
        public int packetFilterAllIpv4 { get; set; }
        public int packetFilterBootpServerIpv6 { get; set; }
        public int packetFilterUser2 { get; set; }
        public int packetFilterAllIpv6 { get; set; }
        public int packetFilterBootpClient { get; set; }
        public int packetFilterMulticastIpv4 { get; set; }
        public int packetFilterBPDU { get; set; }
        public int packetFilterDirection { get; set; }
    }
}
