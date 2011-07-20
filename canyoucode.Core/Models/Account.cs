using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileFx.Security;
using AgileFx.ORM;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    partial class Account
    {
        public static Account VerifyCredentials(string username, string password, long tenantId)
        {
            string hashedPassword = CryptoUtil.HashPassword(password);

            return DataContext.Get().Account.Where(a => a.Username == username
                    && a.Password == hashedPassword && a.TenantId == tenantId).FirstOrDefault();
        }

        public static Account ApplyChanges(object parent, IModification<Account> mod, EntityContext context)
        {
            Account acct = context.ApplyChanges(parent, mod);
            if (mod.Type == ModificationType.Add)
            {

                acct.Password = CryptoUtil.HashPassword(acct.Password);
                acct. DateAdded = DateTime.UtcNow;
            }
            acct.Status = ACCOUNT_STATUS.ACTIVE;
            acct.LastLoginDate = DateTime.UtcNow;
            return acct;
        }

        public static bool Exists(string username, long tenantId)
        {
            return DataContext.Get().Account.Any(a => a.Username == username && a.TenantId == tenantId);
        }

        public static Account GetLoggedInAccount(string username, long tenantId, EntityContext context)
        {
            var account = context.CreateQuery<Account>().Where(x => x.Username == username && x.TenantId == tenantId).First();
            return account;
        }

        public void ChangePassword(string password, string newPassword)
        {
            if (this.Password == CryptoUtil.HashPassword(password))
            {
                this.Password = CryptoUtil.HashPassword(newPassword);
            }
            else
            {
                throw new Exception(MessageCodes.OLD_PASSWORD_MISMATCH);
            }
        }

        public void CreatePasswordResetToken()
        {
            //Get the account in question
            var account = this.DbContext().Account.LoadRelated(a => a.Company, a => a.Employer)
                                .Single(a => a.Username == Username && a.TenantId == TenantId);

            //See if there is a token with the same username
            var token = this.DbContext().Token.FirstOrDefault(t => t.Type == TOKEN_TYPE.PASSWORD_RESET 
                                    && t.Data == Username && t.TenantId == TenantId);

            //If not create a new one.
            if (token == null)
            {
                token = Token.Create(TOKEN_TYPE.PASSWORD_RESET, Username, TenantId);
                this.DbContext().AddObject(token);
            }
            else 
            {
                token.UpdateKey();
            }

            this.DbContext().SaveChanges();

            var name = "";

            if (account.Type == ACCOUNT_TYPE.COMPANY)
            {
                if (!account.IsLoaded(a => a.Company))
                    account.Load(this.DbContext(), a => a.Company);
                name = account.Company.Name;
            }
            else if (account.Type == ACCOUNT_TYPE.EMPLOYER)
            {
                if (!account.IsLoaded(a => a.Company))
                    account.Load(this.DbContext(), a => a.Company);
                name = account.Company.Name;
            }

            string subject = "Reset your password";
            NotificationUtil.SendSystemEmailWithTemplate(account.Email, subject, EMAIL_TEMPLATES.RESET_PASSWORD,
                name, account.Username, token.Key);
        }

        public bool IsValidResetToken(string key)
        {
            var token = this.DbContext().Token.FirstOrDefault(t => t.Key == key && t.Type == TOKEN_TYPE.PASSWORD_RESET 
                            && t.Data == Username && t.TenantId == TenantId);
            return token == null;
        }

        public void ResetPassword(string password, string key)
        {
            var token =  this.DbContext().Token.FirstOrDefault(t => t.Key == key && t.Type == TOKEN_TYPE.PASSWORD_RESET 
                            && t.Data == Username && t.TenantId == TenantId);

            if (token != null)
            {
                Password = CryptoUtil.HashPassword(password);
                token.Delete(this.DbContext());
                this.DbContext().SaveChanges();
            }
            else
            {
                throw new Exception("The password reset key is invalid.");
            }
        }

        public void MarkAllAlertsViewed()
        {
            this.Load(this.DbContext(), a => a.Alerts);
            foreach (var alert in this.Alerts)
            {
                alert.MarkViewed();
            }
        }
    }
}
