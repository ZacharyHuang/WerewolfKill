using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WerewolfKill.Utils.AzureStorage;

namespace WerewolfKill.Utils.AzureTableUserStorage
{
    public class UserStore<TUser> : IUserStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser> where TUser : IdentityUser
    {
        private AzureTable m_userTable;
        private AzureTable m_userClaimTable;
        private AzureTable m_userRoleTable;
        public UserStore()
        { }
        #region UserStore
        /// <summary>
        /// Insert a new TUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(TUser user)
        {
            await m_userTable.CreateAsync(user);
        }
        public async Task DeleteAsync(TUser user)
        {
            await m_userTable.DeleteAsync(user);
        }
        /// <summary>
        /// Returns an TUser instance based on a userId 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId)
        {
            return m_userTable.FindAsync<TUser>(userId, userId);
        }
        /// <summary>
        /// Returns an TUser instance based on a userName 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            string key = HashValue.md5(userName);
            return m_userTable.FindAsync<TUser>(key, key);
        }
        /// <summary>
        /// Updates the UsersTable with the TUser instance values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(TUser user)
        {
            return m_userTable.UpdateAsync(user);
        }
        #endregion
        #region UserClaimStore

        //
        // Summary:
        //     Add a new user claim
        //
        // Parameters:
        //   user:
        //
        //   claim:
        public Task AddClaimAsync(TUser user, Claim claim)
        {
            user.Claims.Add(claim);
            return m_userTable.UpdateAsync(user);
            //return m_userClaimTable.CreateAsync(new UserClaim(user.Id, claim.Type, claim.Value));
        }
        //
        // Summary:
        //     Returns the claims for the user with the issuer set
        //
        // Parameters:
        //   user:
        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                return m_userTable.FindAsync<TUser>(user.Id, "").Result.Claims as IList<Claim>;
            });
            //return Task.Factory.StartNew(() => {
            //    return m_userClaimTable.FindAsync<UserClaim>(user.Id).Result.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList() as IList<Claim>;
            //});
        }
        //
        // Summary:
        //     Remove a user claim
        //
        // Parameters:
        //   user:
        //
        //   claim:
        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            user.Claims.Remove(user.Claims.Find(c => c.Type == claim.Type && c.Value == claim.Value));
            return m_userTable.UpdateAsync(user);
        }
        #endregion
        public void Dispose() { }
    }
}