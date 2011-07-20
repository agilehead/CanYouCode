using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgileFx.ORM;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public class Message
    {
        public static void Send(string message, string recipientEmail, string recipientName, string type, string senderName)
        {
            string from = "no-reply@canyoucode.com";
            string to = "team@canyoucode.com";
            string subject = string.Empty;
            if (type == SEND_MESSAGE_TYPE.FEEDBACK)
            {
                subject = "Feedback from " + senderName;
                NotificationUtil.SendMail(from, to, subject, message, null);
            }
            else
            {
                to = recipientEmail;
                subject = "Invite to Canyoucode from " + senderName;
                NotificationUtil.SendSystemEmailWithTemplate(to, subject, EMAIL_TEMPLATES.CYC_INVITE, recipientName, senderName);
            }
        }

        public static void Send(long recipientId, string senderName, string senderEmail, string message, Account account, Entities context)
        {

            var recipientAccount = context.Account.Single(a => a.Id == recipientId);
            string recipientName = string.Empty;
            if (recipientAccount.Type == ACCOUNT_TYPE.COMPANY)
            {
                recipientAccount.Load(context, a => a.Company);
                recipientName = recipientAccount.Company.Name;
            }
            else
            {
                recipientAccount.Load(context, a => a.Employer);
                recipientName = recipientAccount.Employer.Name;
            }

            var subject = string.Format("You have a message from {0}", senderName);
            NotificationUtil.SendSystemEmailWithTemplate(recipientAccount.Email, subject, EMAIL_TEMPLATES.CONTACT_COMPANY,
                recipientName, senderName, account == null ? senderEmail : account.Email, account == null ? null : account.Phone,
                message);
                
        }
    }
}
