using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class UserLogin : TableEntity
    {
        public UserLogin()
        {
        }

        public UserLogin(string userId, string loginProvider, string providerKey)
        {
            PartitionKey = ProviderKey = providerKey;
            RowKey = LoginProvider = loginProvider;
            UserId = userId;

        }
        public static string UserIdTableQuery(string userId)
        {
            return TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, userId);
        }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string UserId { get; set; }
    }
}