using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using canyoucode.Web.ViewModels;
using canyoucode.Core.Models;
using canyoucode.Core;
using AgileFx.MVC.Utils;
using AgileFx.MVC.ViewModels;
using AgileFx.MVC.Controllers;
using canyoucode.Web.ViewModels.Companies;
using System.Web.Security;
using AgileFx.ORM;
using canyoucode.Web.Utils;
using canyoucode.Core.Utils;
using System.Text.RegularExpressions;

namespace canyoucode.Web.Controllers
{
    [HandleError]
    public class HomeController : CanYouCodeControllerBase
    {

        public ActionResult Index()
        {
            return View<HomePage>(GetViewName("Index"), v =>
                {
                    var recentCompanies = Company.ActiveCompanies(Tenant.Id, DbContext).LoadRelated(c => c.Account, c => c.Tags)
                        .Where(c => c.Account.Rating > 0);

                    //Of these, select the ones who have a picture/logo
                    var withLogos = recentCompanies.Where(c => (c.Type == COMPANY_TYPE.COMPANY && c.Logo != DEFAULT_IMAGES.COMPANY_LOGO)
                        || (c.Type == COMPANY_TYPE.INDIVIDUAL && c.Logo != DEFAULT_IMAGES.PROFILE_PICTURE)).Take(30)
                        .OrderByDescending(c => c.Portfolio.Count)
                        .OrderByDescending(c => c.Account.DateAdded)
                        .OrderByDescending(c => c.Account.Rating)
                        .ToList();
                   // var withoutLogos = recentCompanies.Except(withLogos);

                    v.Pane1 = new List<Company>(); 
                    v.Pane2 = new List<Company>();
                    
                    if (withLogos.Count() > 1)
                    {
                        var parts = withLogos.Split(2).ToList();
                        v.Pane1.AddRange(parts[0]);
                        v.Pane2.AddRange(parts[1]);
                    }
                });
        }

        public ActionResult About()
        {
            return View(GetViewName("About"));
        }


        public ActionResult HTTP404()
        {
            return View(GetViewName("HTTP404"));
        }


        public ActionResult HTTP500()
        {
            return View(GetViewName("HTTP500"));
        }

        public ActionResult Terms()
        {
            return View(GetViewName("Terms"));
        }

        public ActionResult Login(string username)
        {
            NotificationUtil.SendAdminEmail(string.Format("Login Page Request - Username {0}", username), "");
            return View<Login>(v => v.Username = username);
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password, string returnUrl)
        {
            NotificationUtil.SendAdminEmail(string.Format("Login Page Attempted - Username {0}", Username), "");
            Account acct = Account.VerifyCredentials(Username, Password, Tenant.Id);
            if (acct != null)
            {

                Response.Cookies.Add(AuthHelper.GetAuthTicketWithRoles(acct.Username, acct.Type, true, new TimeSpan(0, 30, 0)));

                if (acct.Type != ACCOUNT_TYPE.ADMIN)
                {
                    if (acct.Type == ACCOUNT_TYPE.COMPANY)
                    {
                        var company = DbContext.Company.Where(c => c.Account == acct)
                                            .LoadRelated(c => c.Portfolio, c => c.Account, c => c.Consultants, c => c.Tags)
                                            .Single();

                        if (company.GetInvites().Count() <= 0 && company.GetActiveBids().Count() <= 0)
                        {
                            return Redirect("/" + Username);
                        }
                    }
                    
                    if (!string.IsNullOrWhiteSpace(returnUrl)) return Redirect(returnUrl);
                    return Redirect("/" + Username + "/Projects");
                }
                else
                {
                    return Redirect("/Admin/Index");
                }
                NotificationUtil.SendAdminEmail(string.Format("Login Page Successfull - Username {0}", Username), "");
            }
            else
            {
                return View<Login>(GetViewName("Login"), v =>
                {
                    v.Username = Username;
                    v.AddError(MessageCodes.PASSWORD_MISMATCH);
                });
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult ForgotPassword()
        {
            return View(GetViewName("ForgotPassword"));
        }

        [HttpPost]
        public ActionResult ForgotPassword(string username)
        {
            string emailRegexPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Account account = null;
            if (Regex.Match(username, emailRegexPattern).Success)
            {
                account = DbContext.Account.FirstOrDefault(a => a.Email == username);
            }
            else
            {
                account = DbContext.Account.FirstOrDefault(a => a.Username == username);
            }

            string messageCode;
            if (account != null)
            {
                account.CreatePasswordResetToken();
                messageCode = MessageCodes.RESET_LINK_SENT;
                return Redirect("/?Message=" + messageCode);
            }
            else
            {
                return View(GetViewName("ForgotPassword"), v => { v.MessageCode = MessageCodes.INVALID_USERNAME; });
            }
        }

        public ActionResult ResetPassword(string username, string token)
        {
            var account = DbContext.Account.FirstOrDefault(a => a.Username == username);
            return View<ResetPassword>(GetViewName("ResetPassword"), v =>
            {
                v.IsValidToken = account != null;
                v.Username = username;
                v.Token = token;
            });
        }

        [HttpPost]
        public ActionResult ResetPassword(string username, string password, string token)
        {
            var account = DbContext.Account.Single(a => a.Username == username);
            account.ResetPassword(password, token);
            Response.Cookies.Add(AuthHelper.GetAuthTicketWithRoles(account.Username, account.Type, true, new TimeSpan(0, 30, 0)));
            return Redirect("/" + account.Username + "/Projects?Message=" + MessageCodes.PASSWORD_CHANGED);
        }

        [HttpPost]
        public JsonResult SendMessage(string type, string recipientName, string recipientEmail, string messageText, string yourname)
        {
            try
            {
                Message.Send(messageText, recipientEmail, recipientName, type, yourname);
                NotificationUtil.SendAdminEmail("SendMessage-Copy", 
                    string.Format("Message Text:{0} Email:{1} rec-Name:{2} Type{3} Name{4}", messageText, recipientEmail, recipientName, type, yourname));
                return Json(new AjaxResponse<bool> { Success = true, MessageCode = MessageCodes.SENT });
            }
            catch (Exception)
            {
                return Json(new AjaxResponse<bool> { Success = false, MessageCode = MessageCodes.SEND_FAILED });
            }
        }
    }
}
