using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureStorage
{
    public class AzureStorageFactory
    {
        private static CloudStorageAccount s_account = null;
        private static CloudTableClient s_tableClient = null;
        private static CloudBlobClient s_blobClient = null;
        public static void Initialze(string connectionString)
        {
            s_account = CloudStorageAccount.Parse(connectionString);
            s_tableClient = s_account.CreateCloudTableClient();
            s_blobClient = s_account.CreateCloudBlobClient();
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
        public static AzureBlobContainer GetBlobContainer(string containerName, bool createIfNotExist = false)
        {
            var container = s_blobClient.GetContainerReference(containerName);
            if (createIfNotExist)
            {
                container.CreateIfNotExists();
            }
            return new AzureBlobContainer(container);
        }
    }
}