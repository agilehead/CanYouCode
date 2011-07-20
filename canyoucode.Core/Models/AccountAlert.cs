using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    partial class AccountAlert
    {
        public static void Send(string accountType, string message, long tenantId)
        {
            var entites = DataContext.Get();
            List<Account> accounts = entites.Account.Where(a => a.Type == accountType && a.TenantId == tenantId).ToList();
            
            AccountAlert acctAlert = null;
            accounts.ForEach(a => {
                acctAlert = new AccountAlert();
                acctAlert.AccountId = a.Id;
                acctAlert.Message = message;
                acctAlert.Type = ACCOUNT_ALERT_TYPE.NORMAL;
                acctAlert.Status = ALERT_STATUS.NEW;
                entites.AddObject(acctAlert);
            });

            entites.SaveChanges();
        }

        public void MarkViewed()
        {
            this.Status = ALERT_STATUS.VIEWED;
        }
    }
}
