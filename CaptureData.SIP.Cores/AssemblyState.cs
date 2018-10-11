using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaptureData.SIP.Cores
{
    public class AssemblyState
    {
        public const string LOGGER_NAME = "capture.sip";

        public static ILog logger = null;

        static AssemblyState()
        {
            try
            {
                // Configure logging.
                logger = AppState.GetLogger(LOGGER_NAME);
            }
            catch (Exception excp)
            {
                Console.WriteLine("Exception AssemblyState. " + excp.Message);
            }
        }
    }
}
