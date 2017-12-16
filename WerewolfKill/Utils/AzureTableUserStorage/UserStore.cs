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
    public class UserStore<TUser> :
        IUserStore<TUser>,
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser>,
        IUserLoginStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserLockoutStore<TUser, string>,
        IUserTwoFactorStore<TUser, string>,
        IQueryableUserStore<TUser>
        where TUser : IdentityUser, new ()
    {
        private AzureTable m_userTable;
        private AzureTable m_userClaimTable;
        private AzureTable m_userRoleTable;
        private AzureTable m_userLoginTable;
        private AzureTable m_lookupTable;

        public UserStore() : this("User", "UserClaim", "UserRole", "UserLogin", "UserLookup")
        {
        }

        public UserStore(string userTableName, string userClaimTableName, string userRoleTableName, string userLoginTableName, string lookupTableName)
        {
            m_userTable = AzureTableFactory.GetTable(userTableName, true);
            m_userClaimTable = AzureTableFactory.GetTable(userClaimTableName, true);
            m_userRoleTable = AzureTableFactory.GetTable(userRoleTableName, true);
            m_userLoginTable = AzureTableFactory.GetTable(userLoginTableName, true);
            m_lookupTable = AzureTableFactory.GetTable(lookupTableName, true);
        }
        #region UserStore
        /// <summary>
        /// Insert a new User
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
        /// Find an User by userId 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId)
        {
            return m_userTable.FindAsync<TUser>(userId, string.Empty);
        }
        /// <summary>
        /// Find an User by userName 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            string userId = IdentityUser.GetUserId(userName);
            return m_userTable.FindAsync<TUser>(userId, string.Empty);
        }
        /// <summary>
        /// Updates the User
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(TUser user)
        {
            return m_userTable.UpdateAsync(user);
        }
        #endregion

        #region ClaimStore

        //
        // Summary:
        //     Add a new user claim
        //
        // Parameters:
        //   user:
        //
        //   claim:
        public async Task AddClaimAsync(TUser user, Claim claim)
        {
            await m_userClaimTable.CreateAsync(new UserClaim(user.Id, claim.Type, claim.Value));
        }
        //
        // Summary:
        //     Returns the claims for the user with the issuer set
        //
        // Parameters:
        //   user:
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var claims = await m_userClaimTable.FindByPartitionKeyAsync<UserClaim>(user.Id);
            return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }
        //
        // Summary:
        //     Remove a user claim
        //
        // Parameters:
        //   user:
        //
        //   claim:
        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            foreach (var userClaim in await m_userClaimTable.FindByPartitionKeyAsync<UserClaim>(user.Id))
            {
                if (userClaim.ClaimType == claim.Type && userClaim.ClaimValue == claim.Value)
                {
                    await m_userClaimTable.DeleteAsync(userClaim);
                }
            }
        }
        #endregion

        #region RoleStore
        //
        // Summary:
        //     Adds a user to a role
        //
        // Parameters:
        //   user:
        //
        //   roleName:
        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            await m_userRoleTable.CreateAsync(new UserRole(user.Id, roleName));
        }
        //
        // Summary:
        //     Returns the roles for this user
        //
        // Parameters:
        //   user:
        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            var userRoles = await m_userRoleTable.FindByPartitionKeyAsync<UserRole>(user.Id);
            return userRoles.Select(r => r.RoleName).ToList();
        }
        //
        // Summary:
        //     Returns true if a user is in the role
        //
        // Parameters:
        //   user:
        //
        //   roleName:
        public async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            var userRole = await m_userRoleTable.FindAsync<UserRole>(user.Id, roleName);
            return userRole != null;
        }
        //
        // Summary:
        //     Removes the role for the user
        //
        // Parameters:
        //   user:
        //
        //   roleName:
        public async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            foreach (var userRole in await m_userRoleTable.FindByPartitionKeyAsync<UserRole>(user.Id))
            {
                if (userRole.RoleName == roleName)
                {
                    await m_userClaimTable.DeleteAsync(userRole);
                }
            }
        }
        #endregion  

        #region LoginStore

        //
        // Summary:
        //     Adds a user login with the specified provider and key
        //
        // Parameters:
        //   user:
        //
        //   login:
        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            await m_userLoginTable.CreateAsync(new UserLogin(user.Id, login.LoginProvider, login.ProviderKey));
            await m_lookupTable.CreateAsync(new LookupInfo() { PartitionKey = "LoginProvider_" + login.LoginProvider, RowKey = login.ProviderKey, UserId = user.Id });
        }
        //
        // Summary:
        //     Returns the user associated with this login
        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            var info = await m_lookupTable.FindAsync<LookupInfo>("LoginProvider_" + login.LoginProvider, login.ProviderKey);
            if (info != null)
            {
                return await m_userTable.FindAsync<TUser>(info.UserId, string.Empty);
            }
            return null;
        }
        //
        // Summary:
        //     Returns the linked accounts for this user
        //
        // Parameters:
        //   user:
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            List<UserLoginInfo> res = new List<UserLoginInfo>();
            foreach (var userLogin in await m_userLoginTable.FindByPartitionKeyAsync<UserLogin>(user.Id))
            {
                res.Add(new UserLoginInfo(userLogin.LoginProvider ,userLogin.ProviderKey));
            }
            return res;
        }
        //
        // Summary:
        //     Removes the user login with the specified combination if it exists
        //
        // Parameters:
        //   user:
        //
        //   login:
        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            var userLogin = await m_userLoginTable.FindAsync<UserLogin>(user.Id, login.ProviderKey);
            if (userLogin != null && userLogin.ProviderKey == login.ProviderKey)
            {
                await m_userLoginTable.DeleteAsync(userLogin);
            }
            var info = await m_lookupTable.FindAsync<LookupInfo>("LoginProvider_" + login.LoginProvider, login.ProviderKey);
            if (info != null)
            {
                await m_lookupTable.DeleteAsync(info);
            }
        }
        #endregion

        #region PasswordStore

        //
        // Summary:
        //     Get the user password hash
        //
        // Parameters:
        //   user:
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }
        //
        // Summary:
        //     Returns true if a user has a password set
        //
        // Parameters:
        //   user:
        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        //
        // Summary:
        //     Set the user password hash
        //
        // Parameters:
        //   user:
        //
        //   passwordHash:
        public async Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region SecurityStampStore
        //
        // Summary:
        //     Get the user security stamp
        //
        // Parameters:
        //   user:
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        //
        // Summary:
        //     Set the security stamp for the user
        //
        // Parameters:
        //   user:
        //
        //   stamp:
        public async Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region EmailStore
        //
        // Summary:
        //     Returns the user associated with this email
        //
        // Parameters:
        //   email:
        public async Task<TUser> FindByEmailAsync(string email)
        {
            var info = await m_lookupTable.FindAsync<LookupInfo>("Email", email);
            return await m_userTable.FindAsync<TUser>(info.UserId, string.Empty);
        }
        //
        // Summary:
        //     Get the user email
        //
        // Parameters:
        //   user:
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }
        //
        // Summary:
        //     Returns true if the user email is confirmed
        //
        // Parameters:
        //   user:
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }
        //
        // Summary:
        //     Set the user email
        //
        // Parameters:
        //   user:
        //
        //   email:
        public async Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            await m_userTable.UpdateAsync(user);
            await m_lookupTable.UpdateAsync(new LookupInfo() { PartitionKey = "Email", RowKey = email, UserId = user.Id });
        }
        //
        // Summary:
        //     Sets whether the user email is confirmed
        //
        // Parameters:
        //   user:
        //
        //   confirmed:
        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region PhoneStore
        //
        // Summary:
        //     Get the user phone number
        //
        // Parameters:
        //   user:
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }
        //
        // Summary:
        //     Returns true if the user phone number is confirmed
        //
        // Parameters:
        //   user:
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }
        //
        // Summary:
        //     Set the user's phone number
        //
        // Parameters:
        //   user:
        //
        //   phoneNumber:
        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            await m_userTable.UpdateAsync(user);
            await m_lookupTable.UpdateAsync(new LookupInfo() { PartitionKey = "PhoneNumber", RowKey = phoneNumber, UserId = user.Id });
        }
        //
        // Summary:
        //     Sets whether the user phone number is confirmed
        //
        // Parameters:
        //   user:
        //
        //   confirmed:
        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region LockOutStore
        //
        // Summary:
        //     Returns the current number of failed access attempts. This number usually will
        //     be reset whenever the password is verified or the account is locked out.
        //
        // Parameters:
        //   user:
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }
        //
        // Summary:
        //     Returns whether the user can be locked out.
        //
        // Parameters:
        //   user:
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }
        //
        // Summary:
        //     Returns the DateTimeOffset that represents the end of a user's lockout, any time
        //     in the past should be considered not locked out.
        //
        // Parameters:
        //   user:
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEndDateUtc ?? DateTimeOffset.MinValue);
        }
        //
        // Summary:
        //     Used to record when an attempt to access the user has failed
        //
        // Parameters:
        //   user:
        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ++user.AccessFailedCount;
            await m_userTable.UpdateAsync(user);
            return user.AccessFailedCount;
        }
        //
        // Summary:
        //     Used to reset the access failed count, typically after the account is successfully
        //     accessed
        //
        // Parameters:
        //   user:
        public async Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            await m_userTable.UpdateAsync(user);
        }
        //
        // Summary:
        //     Sets whether the user can be locked out.
        //
        // Parameters:
        //   user:
        //
        //   enabled:
        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            await m_userTable.UpdateAsync(user);

        }
        //
        // Summary:
        //     Locks a user out until the specified end date (set to a past date, to unlock
        //     a user)
        //
        // Parameters:
        //   user:
        //
        //   lockoutEnd:
        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region TwoFactorStore
        //
        // Summary:
        //     Returns whether two factor authentication is enabled for the user
        //
        // Parameters:
        //   user:
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }
        //
        // Summary:
        //     Sets whether two factor authentication is enabled for the user
        //
        // Parameters:
        //   user:
        //
        //   enabled:
        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            await m_userTable.UpdateAsync(user);
        }
        #endregion

        #region QueryableUserStore

        //
        // Summary:
        //     IQueryable users
        public IQueryable<TUser> Users
        {
            get
            {
                return m_userTable.GetAll<TUser>().AsQueryable();
            }
        }
        
        #endregion
        public void Dispose()
        {
        }
    }
}