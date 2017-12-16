using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WerewolfKill.Utils.AzureStorage;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class RoleStore<TRole> :
        IRoleStore<TRole>,
        IQueryableRoleStore<TRole, string>
        where TRole : IdentityRole, new()
    {
        private AzureTable m_roleTable;

        public RoleStore() : this("Role")
        {
        }

        public RoleStore(string roleTableName)
        {
            m_roleTable = AzureTableFactory.GetTable(roleTableName, true);
        }

        //
        // Summary:
        //     Create a new role
        //
        // Parameters:
        //   role:
        public async Task CreateAsync(TRole role)
        {
            await m_roleTable.CreateAsync(role);
        }
        //
        // Summary:
        //     Delete a role
        //
        // Parameters:
        //   role:
        public async Task DeleteAsync(TRole role)
        {
            await m_roleTable.DeleteAsync(role);
        }
        //
        // Summary:
        //     Find a role by id
        //
        // Parameters:
        //   roleId:
        public async Task<TRole> FindByIdAsync(string roleId)
        {
            return await m_roleTable.FindAsync<TRole>(roleId, string.Empty);
        }
        //
        // Summary:
        //     Find a role by name
        //
        // Parameters:
        //   roleName:
        public async Task<TRole> FindByNameAsync(string roleName)
        {
            string roleId = IdentityRole.GetRoleId(roleName);
            return await m_roleTable.FindAsync<TRole>(roleId, string.Empty);
        }
        //
        // Summary:
        //     Update a role
        //
        // Parameters:
        //   role:
        public async Task UpdateAsync(TRole role)
        {
            await m_roleTable.UpdateAsync(role);
        }

        //
        // Summary:
        //     IQueryable Roles
        public IQueryable<TRole> Roles
        {
            get
            {
                return m_roleTable.GetAll<TRole>().AsQueryable();
            }
        }

        public void Dispose()
        {
        }
    }
}