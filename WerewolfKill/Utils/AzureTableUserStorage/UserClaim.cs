using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class UserClaim : TableEntity
    {
        public UserClaim()
        {
        }

        public UserClaim(string userId, string claimType, string claimValue)
        {
            PartitionKey = UserId = userId;
            RowKey = ClaimType = claimType;
            ClaimValue = claimValue;
        }        

        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}