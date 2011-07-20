using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net;
//using Elmah;

namespace canyoucode.Core.Utils
{
    public class NotificationUtil
    {
        public static void Init(string smtpHost, string smtpPort, string mailTemplatesDirectory)
        {
            _SMTP_HOST = smtpHost;
            _SMTP_PORT = int.Parse(smtpPort);
            _MailTemplatesDirectory = mailTemplatesDirectory;
        }

        static string _MailTemplatesDirectory = null;
        static string _SMTP_HOST = null;
        static int _SMTP_PORT;
        static string _SystemEmail = "no-reply@canyoucode.com";
        static string _AdminEmail = "admin@canyoucode.com";
        static string _IgnoreDomain = "@example.com";

        public static void SendMail(string from, string to, string subject, string body, string[] attachments)
        {
            try
            {
                //ignore emails to example.com
                if (to.ToLowerInvariant().Contains(_IgnoreDomain)) return;

                var msg = new MailMessage(from, to, subject, body);
                if (attachments != null) attachments.ToList().ForEach(att => msg.Attachments.Add(new Attachment(att)));
                var mailClient = new SmtpClient(_SMTP_HOST, _SMTP_PORT);
                try
                {
                    mailClient.Send(msg);
                }
                catch (Exception ex)
                {
                    try
                    {
                        //ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    catch { }
                }
            }
            catch { }
        }

        public static void SendSystemMail(string to, string subject, string body, string[] attachments)
        {
            SendMail(_SystemEmail, to, subject, body, attachments);
        }

        public static void SendWithTemplate(string from, string to, string subject, string mailTemplate, params string[] replacements)
        {
            SendWithTemplate(null, from, to, subject, mailTemplate, replacements);
        }

        public static void SendWithTemplate(string[] attachments, string from, string to, string subject, string mailTemplate, params string[] replacements)
        {
            string template = Path.Combine(_MailTemplatesDirectory, mailTemplate + ".txt");
            string body = File.ReadAllText(template);
            string formattedBody = string.Format(body, replacements);
            SendMail(from, to, subject, formattedBody, attachments);
        }

        public static void SendSystemEmailWithTemplate(string to, string subject, string mailTemplate, params string[] replacements)
        {
            SendWithTemplate(_SystemEmail, to, subject, mailTemplate, replacements);
        }

        public static void SendSystemEmailWithTemplate(string[] attachments, string to, string subject, string mailTemplate, params string[] replacements)
        {
            SendWithTemplate(attachments, _SystemEmail, to, subject, mailTemplate, replacements);
        }

        public static void SendAdminEmail(string subject, string message)
        {
            SendMail(_SystemEmail, _AdminEmail, subject, message, null);
        }
    }
}
