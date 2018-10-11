﻿using CaptureData.SIP.Cores.SIP;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaptureData.SIP.Cores.DNS
{
    public class SIPDNSServiceResult
    {
        public SIPServicesEnum SIPService;                  // The type of SIP NAPTR/SRV record that was resolved.
        public int Priority;                                // The priority value assigned to the NAPTR/SRV record. A client MUST attempt to contact the target host with the lowest-numbered priority it can
                                                            // reach; target hosts with the same priority SHOULD be tried in an order defined by the weight field.
        public int Weight;                                  // The weight value assigned to the SRV record. Larger weights SHOULD be given a proportionately higher probability of being selected.
        public int TTL;                                     // The time-to-live in seconds for this record.
        public string Data;                                 // The SRV record for the NAPTR record and target for a SRV record.
        public int Port;                                    // The SRV record port.
        public DateTime? ResolvedAt;                        // Time this record was created.
        public DateTime? EndPointsResolvedAt;               // The time an attempt was made to resolve the A records for the SRV.

        public SIPDNSServiceResult(SIPServicesEnum sipService, int priority, int weight, int ttl, string data, int port, DateTime resolvedAt)
        {
            SIPService = sipService;
            Priority = priority;
            Weight = weight;
            TTL = ttl;
            Data = data;
            Port = port;
            ResolvedAt = resolvedAt;
        }
    }

    public class SIPDNSLookupEndPoint
    {
        public SIPEndPoint LookupEndPoint;
        public int TTL;
        public DateTime ResolvedAt;
        public DateTime FailedAt;
        public string FailureReason;

        public SIPDNSLookupEndPoint(SIPEndPoint sipEndPoint, int ttl)
        {
            LookupEndPoint = sipEndPoint;
            TTL = ttl;
            ResolvedAt = DateTime.Now;
        }
    }

    public class SIPDNSLookupResult
    {
        private static ILog logger = AssemblyState.logger;

        public SIPURI URI;
        public string LookupError;
        public DateTime? NAPTRTimedoutAt;
        public DateTime? SRVTimedoutAt;
        public DateTime? ATimedoutAt;
        public DateTime Inserted = DateTime.Now;
        public Dictionary<SIPServicesEnum, SIPDNSServiceResult> SIPNAPTRResults;
        public List<SIPDNSServiceResult> SIPSRVResults;
        public List<SIPDNSLookupEndPoint> EndPointResults;
        public bool Pending;                // If an aysnc lookup request is made this will be set to true if no immediate result is available.

        public SIPDNSLookupResult(SIPURI uri)
        {
            URI = uri;
        }

        public SIPDNSLookupResult(SIPURI uri, string lookupError)
        {
            URI = uri;
            LookupError = lookupError;
        }

        /// <summary>
        /// Used when the result is already known such as when the lookup is for an IP address but a DNS lookup
        /// object still needs to be returned.
        /// </summary>
        /// <param name="uri">The URI being looked up.</param>
        /// <param name="resultEndPoint">The known result SIP end point.</param>
        public SIPDNSLookupResult(SIPURI uri, SIPEndPoint resultEndPoint)
        {
            URI = uri;
            EndPointResults = new List<SIPDNSLookupEndPoint>() { new SIPDNSLookupEndPoint(resultEndPoint, Int32.MaxValue) };
        }

        public void AddLookupResult(SIPDNSLookupEndPoint lookupEndPoint)
        {
            //logger.Debug(" adding SIP end point result for " + URI.ToString() + " of " + lookupEndPoint.LookupEndPoint + ".");

            if (EndPointResults == null)
            {
                EndPointResults = new List<SIPDNSLookupEndPoint>() { lookupEndPoint };
            }
            else
            {
                EndPointResults.Add(lookupEndPoint);
            }
        }

        public void AddNAPTRResult(SIPDNSServiceResult sipNAPTRResult)
        {
            if (SIPNAPTRResults == null)
            {
                SIPNAPTRResults = new Dictionary<SIPServicesEnum, SIPDNSServiceResult>() { { sipNAPTRResult.SIPService, sipNAPTRResult } };
            }
            else
            {
                SIPNAPTRResults.Add(sipNAPTRResult.SIPService, sipNAPTRResult);
            }
        }

        public void AddSRVResult(SIPDNSServiceResult sipSRVResult)
        {
            if (SIPSRVResults == null)
            {
                SIPSRVResults = new List<SIPDNSServiceResult>() { sipSRVResult };
            }
            else
            {
                SIPSRVResults.Add(sipSRVResult);
            }
        }

        public SIPDNSServiceResult GetNextUnusedSRV()
        {
            if (SIPSRVResults != null && SIPSRVResults.Count > 0)
            {
                return (from srv in SIPSRVResults where srv.EndPointsResolvedAt == null orderby srv.Priority select srv).FirstOrDefault();
            }

            return null;
        }

        public SIPEndPoint GetSIPEndPoint()
        {
            if (EndPointResults != null && EndPointResults.Count > 0)
            {
                return EndPointResults[0].LookupEndPoint;
            }
            else
            {
                return null;
            }
        }
    }
}
