﻿using CaptureData.SIP.Cores.SIP;
using System;

namespace CaptureData.SIP.Cores.SIPTransactions
{
    public class SIPCancelTransaction : SIPTransaction
    {
        public event SIPTransactionResponseReceivedDelegate CancelTransactionFinalResponseReceived;

        private UASInviteTransaction m_originalTransaction;

        internal SIPCancelTransaction(SIPTransport sipTransport, SIPRequest sipRequest, SIPEndPoint dstEndPoint, SIPEndPoint localSIPEndPoint, UASInviteTransaction originalTransaction)
            : base(sipTransport, sipRequest, dstEndPoint, localSIPEndPoint, originalTransaction.OutboundProxy)
        {
            m_originalTransaction = originalTransaction;
            TransactionType = SIPTransactionTypesEnum.NonInvite;
            TransactionRequestReceived += SIPCancelTransaction_TransactionRequestReceived;
            TransactionFinalResponseReceived += SIPCancelTransaction_TransactionFinalResponseReceived;
            TransactionRemoved += SIPCancelTransaction_TransactionRemoved;
        }

        private void SIPCancelTransaction_TransactionRemoved(SIPTransaction transaction)
        {
            // Remove event handlers.
            CancelTransactionFinalResponseReceived = null;
        }

        private void SIPCancelTransaction_TransactionFinalResponseReceived(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPResponse sipResponse)
        {
            if (sipResponse.StatusCode < 200)
            {
                logger.Warn("A SIP CANCEL transaction received an unexpected SIP information response " + sipResponse.ReasonPhrase + ".");
            }
            else
            {
                if (CancelTransactionFinalResponseReceived != null)
                {
                    CancelTransactionFinalResponseReceived(localSIPEndPoint, remoteEndPoint, sipTransaction, sipResponse);
                }
            }
        }

        private void SIPCancelTransaction_TransactionRequestReceived(SIPEndPoint localSIPEndPoint, SIPEndPoint remoteEndPoint, SIPTransaction sipTransaction, SIPRequest sipRequest)
        {
            try
            {
                //logger.Debug("CANCEL request received, attempting to locate and cancel transaction.");

                //UASInviteTransaction originalTransaction = (UASInviteTransaction)GetTransaction(GetRequestTransactionId(sipRequest.Header.Via.TopViaHeader.Branch, SIPMethodsEnum.INVITE));

                SIPResponse cancelResponse;

                if (m_originalTransaction != null)
                {
                    //logger.Debug("Transaction found to cancel " + originalTransaction.TransactionId + " type " + originalTransaction.TransactionType + ".");
                    m_originalTransaction.CancelCall();
                    cancelResponse = GetCancelResponse(sipRequest, SIPResponseStatusCodesEnum.Ok);
                }
                else
                {
                    cancelResponse = GetCancelResponse(sipRequest, SIPResponseStatusCodesEnum.CallLegTransactionDoesNotExist);
                }

                //UpdateTransactionState(SIPTransactionStatesEnum.Completed);
                SendFinalResponse(cancelResponse);
            }
            catch (Exception excp)
            {
                logger.Error("Exception SIPCancelTransaction GotRequest. " + excp.Message);
            }
        }

        private SIPResponse GetCancelResponse(SIPRequest sipRequest, SIPResponseStatusCodesEnum sipResponseCode)
        {
            try
            {
                SIPResponse cancelResponse = new SIPResponse(sipResponseCode, null, sipRequest.LocalSIPEndPoint);

                SIPHeader requestHeader = sipRequest.Header;
                cancelResponse.Header = new SIPHeader(requestHeader.From, requestHeader.To, requestHeader.CSeq, requestHeader.CallId);
                cancelResponse.Header.CSeqMethod = SIPMethodsEnum.CANCEL;
                cancelResponse.Header.Vias = requestHeader.Vias;
                cancelResponse.Header.MaxForwards = Int32.MinValue;

                return cancelResponse;
            }
            catch (Exception excp)
            {
                logger.Error("Exception GetCancelResponse. " + excp.Message);
                throw excp;
            }
        }
    }
}