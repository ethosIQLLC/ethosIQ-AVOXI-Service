using ethosIQ_AVOXI_Shared.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ethosIQ_AVOXI_Shared.DAO
{
    public class CallDetailRecord
    {
        public string CallID;
        public string FromDN;
        public string ToDN;
        public string FromCountry;
        public string ToCountry;
        public string Status;
        public string Direction;
        public DateTime DateStart;
        public DateTime DateAnswered;
        public DateTime DateEnd;
        public int DurSecs;

        public CallDetailRecord(CallData callData)
        {
            CallID = callData.AvoxiCallId;
            FromDN = callData.From;
            ToDN = callData.To;
            FromCountry = callData.FromCountry;
            ToCountry = callData.ToCountry;
            Status = callData.Status;
            Direction = callData.Direction;
            DateStart = callData.DateStart.ToUniversalTime();
            DateAnswered = callData.DateAnswered.ToUniversalTime();
            DateEnd = callData.DateEnd.ToUniversalTime();
            DurSecs = Convert.ToInt32(callData.Duration.Seconds);
        }
    }
}
