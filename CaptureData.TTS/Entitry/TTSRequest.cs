using System;
using System.Collections.Generic;
using System.Text;

namespace CaptureData.TTS.Entitry
{
    [Serializable]
    public class TTSRequest
    {
        private string _cti { get; set; }
        private string _callId { get; set; }
    }
}
