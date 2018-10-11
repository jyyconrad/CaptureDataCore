using System;
using System.Runtime.Serialization;

namespace CaptureData.SIP.Cores.SIP
{
    [Serializable]
    internal class SIPValidationException : Exception
    {
        public SIPValidationFieldsEnum SIPErrorField;
        public SIPResponseStatusCodesEnum SIPResponseErrorCode;

        public SIPValidationException(SIPValidationFieldsEnum sipErrorField, string message)
            : base(message)
        {
            SIPErrorField = sipErrorField;
            SIPResponseErrorCode = SIPResponseStatusCodesEnum.BadRequest;
        }

        public SIPValidationException(SIPValidationFieldsEnum sipErrorField, SIPResponseStatusCodesEnum responseErrorCode, string message)
            : base(message)
        {
            SIPErrorField = sipErrorField;
            SIPResponseErrorCode = responseErrorCode;
        }
    }
}