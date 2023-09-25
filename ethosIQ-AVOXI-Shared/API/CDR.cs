using System;
using System.Diagnostics;


using RestSharp;

namespace ethosIQ_AVOXI_Shared.API
{

    public class CDR
    {
        private RestClient Client;
        private string intervalStart;
        private string intervalEnd;
        private EventLog eventLog;

        public CDR(string intervalStart, string intervalEnd)
        {
            this.intervalEnd = intervalEnd;
            this.intervalStart = intervalStart;
            eventLog = new EventLog
            {
                Source = "AVOXI Service"
            };
        }
        public void SetWebClient(RestClient Client)
        {
            this.Client = Client;
        }

        public GetCDRsResponse GetCDR(int offset)
        {
            GetCDRsResponse cdrList = null;
            try
            {
                if (Client != null)
                {
                    var url = "https://genius.avoxi.com/api/v2/cdrs";
                    var request = new RestRequest(url, Method.Get);
                    request.AddQueryParameter("call_start_oldest", intervalStart);
                    request.AddQueryParameter("call_start_newest", intervalEnd);
                    request.AddQueryParameter("limit", 10000);
                    request.AddQueryParameter("offset", offset);
                    request.AddQueryParameter("timezone", "UTC");
                    RestResponse response = Client.Execute(request);
                    if(response.IsSuccessful)
                    {
                        cdrList = ParseGetCDRs(response.Content);
                    }
                    else
                    {
                        Console.WriteLine("Status Code " + response.StatusCode + ": "+ response.StatusDescription);
                        Console.WriteLine(response.Content);
                        eventLog.WriteEntry("Status Code " + response.StatusCode + ": " + response.StatusDescription + ".\n" + response.Content, EventLogEntryType.Error);
                    }



                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
            
            return cdrList;
        }

        private GetCDRsResponse ParseGetCDRs(string jsonResponse)
        {
            return GetCDRsResponse.FromJson(jsonResponse);
        }
    }
}
