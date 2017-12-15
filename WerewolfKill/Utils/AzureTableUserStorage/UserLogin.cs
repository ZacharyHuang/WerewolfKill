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
            PartitionKey = RowKey = UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            
        }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string UserId { get; set; }
    }
}