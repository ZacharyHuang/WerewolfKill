using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class LookupInfo : TableEntity
    {
        public LookupInfo()
        {
        }
        public string UserId { get; set; }
    }
}