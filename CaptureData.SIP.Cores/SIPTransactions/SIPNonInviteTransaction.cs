using CaptureData.SIP.Cores.SIP;

namespace CaptureData.SIP.Cores.SIPTransactions
{
    public class SIPNonInviteTransaction : SIPTransaction
    {
        public event SIPTransactionResponseReceivedDelegate NonInviteTransactionInfoResponseReceived;
        public event SIPTransactionResponseReceivedDelegate NonInviteTransactionFinalResponseReceived;
        public event SIPTransactionTimedOutDelegate NonInviteTransactionTimedOut;
        public event SIPTransactionRequestReceivedDelegate NonInviteRequestReceived;
        public event SIPTransactionRequestRetransmitDelegate NonInviteTransactionRequestRetransmit;

        internal SIPNonInviteTransaction(SIPTransport sipTransport, SIPRequest sipRequest, SIPEndPoint dstEndPoint, SIPEndPoint localSIPEndPoint, SIPEndPoint outboundProxy)
            : base(sipTransport, sipRequest, dstEndPoint, localSIPEndPoint, outboundProxy)
        {
            TransactionType = SIPTransactionTypesEnum.NonInvite;
            TransactionRequestReceived += SIPNonInviteTransaction_TransactionRequestReceived;
            TransactionInformationResponseReceived += SIPNonInviteTransaction_TransactionInformationResponseReceived;
            TransactionFinalResponseReceived += SIPNonInviteTransaction_TransactionFinalResponseReceived;
            TransactionTimedOut += SIPNonInviteTransaction_TransactionTimedOut;
            TransactionRemoved += SIPNonInviteTransaction_TransactionRemoved;
            TransactionRequestRetransmit += SIPNonInviteTransaction_TransactionRequestRetransmit;
        }

        private void SIPNonInviteTransaction_TransactionRemoved(SIPTransaction transaction)
        {
            // Remove all event handlers.
            NonInviteTransactionInfoResponseReceived = null;
            NonInviteTransactionFinalResponseReceived = null;
            NonInviteTransactionTimedOut = null;
            NonInviteRequestReceived = null;
        }

        private void SIPNonInviteTransaction_TransactionTimedOut(SIPTransaction sipTransaction)
        {
            if (NonInviteTransactionTimedOut != null)
            {
                NonInviteTransactionTimedOut(this);
            }
        }

        private void SIPNonInviteTransaction_TransactionRequestReceived(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPRequest sipRequest)
        {
            if (NonInviteRequestReceived != null)
            {
                NonInviteRequestReceived(localSIPEndPoint, remoteEndPoint, this, sipRequest);
            }
        }

        private void SIPNonInviteTransaction_TransactionInformationResponseReceived(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPResponse sipResponse)
        {
            if (NonInviteTransactionInfoResponseReceived != null)
            {
                NonInviteTransactionInfoResponseReceived(localSIPEndPoint, remoteEndPoint, this, sipResponse);
            }
        }

        private void SIPNonInviteTransaction_TransactionFinalResponseReceived(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPResponse sipResponse)
        {
            if (NonInviteTransactionFinalResponseReceived != null)
            {
                NonInviteTransactionFinalResponseReceived(localSIPEndPoint, remoteEndPoint, this, sipResponse);
            }
        }

        private void SIPNonInviteTransaction_TransactionRequestRetransmit(SIPTransaction sipTransaction, SIPRequest sipRequest, int retransmitNumber)
        {
            if (NonInviteTransactionRequestRetransmit != null)
            {
                NonInviteTransactionRequestRetransmit(sipTransaction, sipRequest, retransmitNumber);
            }
        }
    }
}