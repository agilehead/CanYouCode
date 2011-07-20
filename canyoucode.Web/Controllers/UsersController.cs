using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using canyoucode.Core;
using canyoucode.Core.Models;
using canyoucode.Web.Utils;

using AgileFx;
using AgileFx.ORM;
using AgileFx.MVC;
using canyoucode.Core.Utils;

namespace canyoucode.Web.Controllers
{
    public class UsersController : CanYouCodeControllerBase
    {
        public JsonResult Exists(string username)
        {
            try
            {
                return Json(new AjaxResponse<bool>
                    {
                        Success = true,
                        Result = Account.Exists(username, Tenant.Id) ? true : false
                    });
            }
            catch (Exception)
            {
                return Json(new AjaxResponse<bool> { Success = false });
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string id, string oldPassword, string password)
        {
            var account = DbContext.Account.Single(a => a.Id == LoggedInAccount.Id);

            string messageCode = MessageCodes.PASSWORD_CHANGED;
            try
            {
                account.ChangePassword(oldPassword, password);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                messageCode = ex.Message;
            }

            return Redirect(string.Format("/{0}/Edit?Message={1}", account.Username, messageCode));
        }

        public JsonResult Contact(long recipientId, string senderName, string senderEmail, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderName)) throw new InvalidOperationException();
                Message.Send(recipientId, senderName, senderEmail, message, LoggedInAccount, DbContext);
                NotificationUtil.SendAdminEmail("SendMessage-Copy",
                    string.Format("Message Text:{0} Email:{1} Name{2} recid:{3}", message, senderEmail, senderName, recipientId));
                return Json(new AjaxResponse<bool> { Success = true, MessageCode = MessageCodes.SENT });
            }
            catch (Exception)
            {
                return Json(new AjaxResponse<bool> { Success = false, MessageCode = MessageCodes.SEND_FAILED });
            }
        }
    }
}
