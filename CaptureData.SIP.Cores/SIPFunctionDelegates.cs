using CaptureData.SIP.Cores.DNS;
using CaptureData.SIP.Cores.SIP;
using CaptureData.SIP.Cores.SIP.Channel;
using CaptureData.SIP.Cores.SIPTransactions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CaptureData.SIP.Cores
{
    // SIP Channel delegates.
    public delegate void SIPMessageSentDelegate(SIPChannel sipChannel, SIPEndPoint remoteEndPoint, byte[] buffer);
    public delegate void SIPMessageReceivedDelegate(SIPChannel sipChannel, SIPEndPoint remoteEndPoint, byte[] buffer);

    // SIP Transport delegates.
    public delegate void SIPTransportRequestDelegate(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPRequest sipRequest);
    public delegate void SIPTransportResponseDelegate(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPResponse sipResponse);
    public delegate void SIPTransportSIPBadMessageDelegate(SIPEndPoint localSIPEndPoint, SIPEndPoint remotePoint, string message, SIPValidationFieldsEnum errorField, string rawMessage);
    public delegate void STUNRequestReceivedDelegate(IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, byte[] buffer, int bufferLength);
    public delegate SIPDNSLookupResult ResolveSIPEndPointDelegate(SIPURI uri, bool async);

    // SIP Transaction delegates.
    public delegate void SIPTransactionStateChangeDelegate(SIPTransaction sipTransaction);
    public delegate void SIPTransactionResponseReceivedDelegate(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPResponse sipResponse);
    public delegate void SIPTransactionRequestReceivedDelegate(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPRequest sipRequest);
    public delegate void SIPTransactionCancelledDelegate(SIPTransaction sipTransaction);
    public delegate void SIPTransactionTimedOutDelegate(SIPTransaction sipTransaction);
    public delegate void SIPTransactionRequestRetransmitDelegate(SIPTransaction sipTransaction, SIPRequest sipRequest, int retransmitNumber);
    public delegate void SIPTransactionResponseRetransmitDelegate(SIPTransaction sipTransaction, SIPResponse sipResponse, int retransmitNumber);
    public delegate void SIPTransactionRemovedDelegate(SIPTransaction sipTransaction);
    public delegate void SIPTransactionTraceMessageDelegate(SIPTransaction sipTransaction, string message);
}
