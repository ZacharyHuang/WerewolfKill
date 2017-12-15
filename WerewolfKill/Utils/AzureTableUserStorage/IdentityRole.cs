using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class IdentityRole : Microsoft.WindowsAzure.Storage.Table.TableEntity, IRole<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IdentityRole() { }
        public IdentityRole(string name)
        {
            string hash = HashValue.md5(name);
            Id = PartitionKey = hash;
            RowKey = string.Empty;
            Name = name;
        }
    }
}