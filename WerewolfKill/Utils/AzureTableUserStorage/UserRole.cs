using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class UserRole : TableEntity
    {
        public UserRole()
        {
        }

        public UserRole(string userId, string roleName)
        {
            string roleId = HashValue.md5(roleName);
            PartitionKey = UserId = userId;
            RowKey = RoleName = roleName;
            RoleId = roleId;
        }

        public string UserId { get; set; }

        public string RoleId { get; set; }

        public string RoleName { get; set; }
    }
}