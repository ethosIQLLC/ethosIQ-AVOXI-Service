using System;
using System.Configuration;
using ethosIQ_Database;

namespace ethosIQ_AVOXI_Shared.Configuration
{
    public class AVOXICollectionDatabaseConfiguration
    {
        public static Database GetCollectionDatabase()
        {
            Database collectionDB = DatabaseFactory.CreateDatabase(ConfigurationManager.AppSettings["DatabaseType"].ToString(),
                                                 ConfigurationManager.AppSettings["DataSource"].ToString(),
                                                 ConfigurationManager.AppSettings["Server"].ToString(),
                                                 Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                                                 ConfigurationManager.AppSettings["Username"].ToString(),
                                                 ConfigurationManager.AppSettings["Password"].ToString(),
                                                 90, 20, 50);

            return collectionDB;
        }
    }
}
