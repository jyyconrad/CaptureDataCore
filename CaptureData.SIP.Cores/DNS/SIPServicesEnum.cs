namespace CaptureData.SIP.Cores.DNS
{
    public class SIPDNSConstants
    {
        public const string NAPTR_SIP_UDP_SERVICE = "SIP+D2U";
        public const string NAPTR_SIP_TCP_SERVICE = "SIP+D2T";
        public const string NAPTR_SIPS_TCP_SERVICE = "SIPS+D2T";

        public const string SRV_SIP_TCP_QUERY_PREFIX = "_sip._tcp.";
        public const string SRV_SIP_UDP_QUERY_PREFIX = "_sip._udp.";
        public const string SRV_SIP_TLS_QUERY_PREFIX = "_sip._tls.";
        public const string SRV_SIPS_TCP_QUERY_PREFIX = "_sips._tcp.";
    }

    /// <summary>
    /// A list of the different combinations of SIP schemes and transports. 
    /// </summary>
    public enum SIPServicesEnum
    {
        none = 0,
        sipudp = 1,     // sip over udp. SIP+D2U and _sip._udp
        siptcp = 2,     // sip over tcp. SIP+D2T and _sip._tcp
        sipsctp = 3,    // sip over sctp. SIP+D2S and _sip._sctp
        siptls = 4,     // sip over tls. _sip._tls.
        sipstcp = 5,    // sips over tcp. SIPS+D2T and _sips._tcp
        sipssctp = 6,   // sips over sctp. SIPS+D2S and _sips._sctp
    }

    public class SIPServices
    {
        public static SIPServicesEnum GetService(string service)
        {
            if (service == SIPDNSConstants.NAPTR_SIP_UDP_SERVICE)
            {
                return SIPServicesEnum.sipudp;
            }
            else if (service == SIPDNSConstants.NAPTR_SIP_TCP_SERVICE)
            {
                return SIPServicesEnum.siptcp;
            }
            else if (service == SIPDNSConstants.NAPTR_SIPS_TCP_SERVICE)
            {
                return SIPServicesEnum.sipstcp;
            }
            else
            {
                return SIPServicesEnum.none;
            }
        }
    }
}