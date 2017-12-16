using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureStorage
{
    public class AzureTableFactory
    {
        private static CloudStorageAccount s_account = null;
        private static CloudTableClient s_tableClient = null;
        public static void Initialze(string connectionString)
        {
            s_account = CloudStorageAccount.Parse(connectionString);
            s_tableClient = s_account.CreateCloudTableClient();
        }
        public static AzureTable GetTable(string tableName, bool createIfNotExist = false)
        {
            var table = s_tableClient.GetTableReference(tableName);
            if (createIfNotExist)
            {
                table.CreateIfNotExists();
            }
            return new AzureTable(table);
        }
    }
}