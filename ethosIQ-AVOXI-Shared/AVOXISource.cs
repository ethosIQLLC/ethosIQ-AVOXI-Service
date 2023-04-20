using ethosIQ_AVOXI_Shared.API;
using ethosIQ_AVOXI_Shared.DAO;
using ethosIQ_Configuration;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Timers;

namespace ethosIQ_AVOXI_Shared
{
    public class AVOXISource : ethosIQSource
    {
        private readonly string AccessToken;
        private RestClient Client;
        private Timer HistoricalTimer;
        private int Interval;
        private string TimeZoneOffset;
        private DateTime Now;
        private int Delay;

        public AVOXISource(string Name, string AccessToken)
        {
            this.AccessToken = AccessToken;
            this.Name = Name;

           
        }
        public override void Start()
        {
            InitializeEventLog("AVOXI Service");
            var options = new RestClientOptions("https://genius.avoxi.com/api/v2/cdrs")
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer")
            };
            Client = new RestClient(options);            
            if(Client != null)
            {
                HistoricalTimer = new Timer(60000);
                ElapsedEventHandler handler = new ElapsedEventHandler(GetIntervalReport);
                HistoricalTimer.Elapsed += handler;
                HistoricalTimer.Start();

                Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"].ToString());
                TimeZoneOffset = (ConfigurationManager.AppSettings["TimeZoneOffset"].ToString());
                Delay = 5;
             //   TestReportFunction();
            }
        }
        public void Repost(long startDate, long endDate)
        {
            InitializeEventLog("AVOXI Service");
            var options = new RestClientOptions("https://genius.avoxi.com/api/v2/cdrs")
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer")
            };
            Client = new RestClient(options);
            GetCDRsResponse response = null;
            bool GetCDRs = false;
            while (!GetCDRs)
            {
                try
                {
                    int offset = 0;

                    string format = "yyyy-MM-ddTHH:mm:ss";
                    string startTime = DateTimeOffset.FromUnixTimeSeconds(startDate).DateTime.ToString(format);
                    string endTime = DateTimeOffset.FromUnixTimeSeconds(endDate).DateTime.ToString(format);
                    CDR cdr = new CDR(startTime, endTime);
                    cdr.SetWebClient(Client);
                    response = cdr.GetCDR(0);

                    if (response != null)
                    {
                        ProcessCDR(response);
                        LogInformation("Successfully reposted CDR data for " + response.Data.Count + " calls  for Interval " + startDate + " to " + endDate);
                        Console.WriteLine("Successfully reposted CDR data for " + response.Data.Count + " calls  for Interval " + startDate + " to " + endDate);

                    }
                    while (response.Data.Count % 10000 == 0)
                    {
                        offset += 10000;
                        response = cdr.GetCDR(offset);
                        if (response != null)
                        {
                            ProcessCDR(response);
                            LogInformation("Successfully pulled and processed CDR data for " + response.Data.Count + " calls  for Interval " + startTime + " to " + endTime);
                            Console.WriteLine("Successfully pulled and processed CDR data for " + response.Data.Count + " calls  for Interval " + startTime + " to " + endTime);
                        }
                    }
                    GetCDRs = true;
                }
                catch (Exception exception)
                {
                    LogError("Failed to repost CDR data: " + exception.Message + "\n" + exception.StackTrace);
                    System.Threading.Thread.Sleep(5000);

                }
            }
        }
        private void GetIntervalReport(object sender, ElapsedEventArgs args)
        {
            try
            {
                HistoricalTimer.Stop();
                if(CheckInterval())
                {
                    GetCDRsResponse response = null;
                    bool GetCDRs = false;
                    int offset = 0;
                    while(!GetCDRs)
                    {
                        try
                        { 
                            string startTime = GetIntervalStartTime(DateTime.UtcNow);
                            string endTime = GetIntervalEndTime(DateTime.UtcNow);
                            CDR cdr = new CDR(startTime, endTime);
                            cdr.SetWebClient(Client);
                            response = cdr.GetCDR(offset);

                            if(response != null)
                            {
                                ProcessCDR(response);
                                LogInformation("Successfully pulled and processed CDR data for "+ response.Data.Count + " calls  for Interval " + startTime + " to "+ endTime);
                                Console.WriteLine("Successfully pulled and processed CDR data for " + response.Data.Count + " calls  for Interval " + startTime + " to " + endTime);

                            }
                            while(response.Data.Count%10000==0)
                            {
                                offset += 10000;
                                response = cdr.GetCDR(offset);
                                if (response != null)
                                {
                                    ProcessCDR(response);
                                    LogInformation("Successfully pulled and processed CDR data for " + response.Data.Count + " calls  for Interval " + startTime + " to " + endTime);
                                    Console.WriteLine("Successfully pulled and processed CDR data for " + response.Data.Count + " calls  for Interval " + startTime + " to " + endTime);
                                }
                            }
                            GetCDRs = true;
                        }
                        catch(Exception exception)
                        {
                            LogError("Failed to pull and process CDR data: " + exception.Message + "\n" + exception.StackTrace);
                            System.Threading.Thread.Sleep(5000);

                        }
                    }
                }
            }
            catch(Exception e)
            {
                LogError("Failed to pull and process CDR data: " + e.Message + "\n" + e.StackTrace);
                System.Threading.Thread.Sleep(10000);
            }
            HistoricalTimer.Start();
        }

        private void TestReportFunction()
        {
            GetCDRsResponse response = null;
            bool GetCDRs = false;
            while (!GetCDRs)
            {
                try
                {
                    string startTime = GetIntervalStartTime(DateTime.UtcNow);
                    string endTime = GetIntervalEndTime(DateTime.UtcNow);
                    CDR cdr = new CDR(startTime, endTime);
                    cdr.SetWebClient(Client);
                    response = cdr.GetCDR(0);

                    if (response != null)
                    {
                        ProcessCDR(response);
                        LogInformation("Successfully pulled and processed CDR data for " + response.Data.Count + "calls  for Interval " + startTime + " to " + endTime);
                        Console.WriteLine("Successfully pulled and processed CDR data for " + response.Data.Count + "calls  for Interval " + startTime + " to " + endTime);
                    }
                    GetCDRs = true;
                }
                catch (Exception exception)
                {
                    LogError("Failed to pull and process CDR data: " + exception.Message + "\n" + exception.StackTrace);
                    System.Threading.Thread.Sleep(5000);
                }
                
            }
        }
        private bool CheckInterval()
        {
            Now = DateTime.Now;
            if ((Now.Minute - Delay) % Interval == 0)
                return true;   
            return false;
        }

        private string GetIntervalStartTime(DateTime CurrentTime)
        {
            string format = "yyyy-MM-ddTHH:mm:ss";
            DateTime startTime = new DateTime();
            string IntervalStartTime="";
            
                startTime = CurrentTime.AddMinutes((-1*Interval));
                startTime = DateTimeExtensions.RoundDown(startTime, TimeSpan.FromMinutes(Interval));
                IntervalStartTime = startTime.ToString(format)+TimeZoneOffset;
           
            return IntervalStartTime;
        }
        private string GetIntervalEndTime(DateTime CurrentTime)
        {
            string format = "yyyy-MM-ddTHH:mm:ss";
            string IntervalEndTime = "";
            DateTime endTime = new DateTime();
            endTime = DateTimeExtensions.RoundDown(CurrentTime, TimeSpan.FromMinutes(Interval));
            IntervalEndTime = endTime.ToString(format) + TimeZoneOffset;
            
            return IntervalEndTime;
        }
        

        private void ProcessCDR(GetCDRsResponse response)
        {
            List<CallDetailRecord> CallDetailRecords = new List<CallDetailRecord>();
            foreach(CallData report in response.Data)
            {
                CallDetailRecords.Add(new CallDetailRecord(report));
            }

            TBL_AVOXI_TFN tbl_avoxi_tfn = new TBL_AVOXI_TFN(CollectionDatabase);
            tbl_avoxi_tfn.Insert(CallDetailRecords);

        }

        public override void Restart()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime RoundDown(this DateTime dateTime, TimeSpan interval)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
        }
    }

}
