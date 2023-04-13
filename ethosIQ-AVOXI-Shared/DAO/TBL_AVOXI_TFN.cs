using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ethosIQ_Database;
using System.Data;


namespace ethosIQ_AVOXI_Shared.DAO
{
    public class TBL_AVOXI_TFN
    {
        private Database Database;

        public TBL_AVOXI_TFN(Database database)
        {
            this.Database = database;
        }

        public void Insert(List<CallDetailRecord> CDRList)
        {
            if(Database != null)
            {
                string[] CALLIDs = new string[CDRList.Count];
                string[] FROMDNs = new string[CDRList.Count];
                string[] TODNs = new string[CDRList.Count];
                string[] FROMCOUNTRYs = new string[CDRList.Count];
                string[] TOCOUNTRYs = new string[CDRList.Count];
                string[] STATUSes = new string[CDRList.Count];
                string[] DIRECTIONs = new string[CDRList.Count];
                string[] DATESTARTs = new string[CDRList.Count];
                string[] DATEANSWEREDs = new string[CDRList.Count];
                string[] DATEENDs = new string[CDRList.Count];
                int[] DURATIONs = new int[CDRList.Count];
                int i = 0;
                foreach(CallDetailRecord cdr in CDRList)
                {
                    CALLIDs[i] = cdr.CallID;
                    FROMDNs[i] = cdr.FromDN;
                    TODNs[i] = cdr.ToDN;
                    FROMCOUNTRYs[i] = cdr.FromCountry;
                    TOCOUNTRYs[i] = cdr.ToCountry;
                    STATUSes[i] = cdr.Status;
                    DIRECTIONs[i] = cdr.Direction;
                    DATESTARTs[i] = cdr.DateStart;
                    DATEANSWEREDs[i] = cdr.DateAnswered;
                    DATEENDs[i] = cdr.DateEnd;
                    DURATIONs[i] = cdr.DurSecs;
                    i++;
                }

                using (IDbConnection connection = Database.CreateOpenConnection())
                {
                    IDataParameter CALLID = Database.CreateParameter(":CALL_ID", "string", CALLIDs, ParameterDirection.Input);
                    IDataParameter FROMDN = Database.CreateParameter(":FROM_DN", "string", FROMDNs, ParameterDirection.Input);
                    IDataParameter TODN = Database.CreateParameter(":TO_DN", "string", TODNs, ParameterDirection.Input);
                    IDataParameter FROMCOUNTRY = Database.CreateParameter(":FROM_COUNTRY", "string", FROMCOUNTRYs, ParameterDirection.Input);
                    IDataParameter TOCOUNTRY = Database.CreateParameter(":TO_COUNTRY", "string", TOCOUNTRYs, ParameterDirection.Input);
                    IDataParameter STATUS = Database.CreateParameter(":STATUS", "string", STATUSes, ParameterDirection.Input);
                    IDataParameter DIRECTION = Database.CreateParameter(":DIRECTION", "string", DIRECTIONs, ParameterDirection.Input);
                    IDataParameter DATESTART = Database.CreateParameter(":DATE_START", "string", DATESTARTs, ParameterDirection.Input);
                    IDataParameter DATEANSWERED = Database.CreateParameter(":DATE_ANSWERED", "string", DATEANSWEREDs, ParameterDirection.Input);
                    IDataParameter DATEEND = Database.CreateParameter(":DATE_END", "string", DATEENDs, ParameterDirection.Input);
                    IDataParameter DURATION = Database.CreateParameter(":DUR_SECS", "int", DURATIONs, ParameterDirection.Input);


                    using (IDbCommand command = Database.CreateCommand("INSERT INTO TBL_AVOXI_TFN (CALL_ID, FROM_DN, TO_DN, FROM_COUNTRY, TO_COUNTRY, STATUS, DIRECTION, DATE_START, DATE_ANSWERED, DATE_END, DUR_SECS) VALUES (:CALL_ID, :FROM_DN, :TO_DN, :FROM_COUNTRY, :TO_COUNTRY, :STATUS, :DIRECTION, :DATE_START, :DATE_ANSWERED, :DATE_END, :DUR_SECS)", connection, CDRList.Count))
                    {
                        command.Parameters.Add(CALLID);
                        command.Parameters.Add(FROMDN);
                        command.Parameters.Add(TODN);
                        command.Parameters.Add(FROMCOUNTRY);
                        command.Parameters.Add(TOCOUNTRY);
                        command.Parameters.Add(STATUS);
                        command.Parameters.Add(DIRECTION);
                        command.Parameters.Add(DATESTART);
                        command.Parameters.Add(DATEANSWERED);
                        command.Parameters.Add(DATEEND);
                        command.Parameters.Add(DURATION);
                      

                        Console.WriteLine(command.CommandText);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Truncate()
        {
            if (Database != null)
            {
                using (IDbConnection connection = Database.CreateOpenConnection())
                {
                    using (IDbCommand command = Database.CreateCommand("TRUNCATE TABLE TBL_AVOXI_TFN", connection))
                    {
                        Console.WriteLine(command.CommandText);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
