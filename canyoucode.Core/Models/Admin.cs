using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileFx.Security;

namespace canyoucode.Core.Models
{
    public class Admin
    {
        public static Account Create(string username, string email, string password, long tenantId)
        {
            var encPassword = CryptoUtil.HashPassword(password);
            var account = new Account()
            {
                Password = encPassword,
                Username = username,
                Type = ACCOUNT_TYPE.ADMIN,
                Email = email,
                DateAdded = DateTime.UtcNow,
                LastLoginDate = DateTime.UtcNow,
                Status = ACCOUNT_STATUS.ACTIVE,
                TenantId = tenantId
            };
            return account;
        }
    }
}
