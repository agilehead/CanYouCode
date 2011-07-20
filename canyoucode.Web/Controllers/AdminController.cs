
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using canyoucode.Web.ViewModels.Admin;
using canyoucode.Core;
using AgileFx.ORM;
using AgileFx.Security;
using canyoucode.Web.ViewModels;
using canyoucode.Web.Utils;
using canyoucode.Core.Utils;
using canyoucode.Core.Models;

namespace canyoucode.Web.Controllers
{
    [Authorize(Roles=ACCOUNT_TYPE.ADMIN)]
    public class AdminController : CanYouCodeControllerBase
    {
        public ActionResult Index()
        {
            return View<CanYouCodeViewModel>(GetViewName("Index"));
        }

        //This is for marketing, to create company accounts that are not active.
        public ActionResult CreateMarketingAccounts()
        {
            return View<CreateMarketingAccounts>(GetViewName("CreateMarketingAccounts"));
        }

        //This is for marketing, to create company accounts that are not active.
        [HttpPost]
        public ActionResult CreateMarketingAccounts(FormCollection coll)
        {
            var type = coll["Type"];
            var company = Company.Create(coll["CompanyName"],
                    Common.FixUrl(coll["Website"]), coll["City"], coll["Country"], coll["Username"], Common.RandomString(10),
                    null, CURRENCY.USD, coll["Email"], coll["Phone"], type, Tenant.Id, DbContext);

            company.Description = coll["Description"];

            //account should not be active.
            company.Account.Status = ACCOUNT_STATUS.DISABLED;
            //reset the portfolio
            company.Portfolio.Clear();
            //We reset the password to "";
            company.Account.Password = "";
            if (Request.Files["Logo"] != null && Request.Files["Logo"].ContentLength > 0)
            {
                company.SaveLogo(Request.Files["Logo"] as HttpPostedFileBase);
            }

            var name = type == COMPANY_TYPE.INDIVIDUAL ? coll["CompanyName"] : "John Doe";
            company.AddConsultant(name, "Vice President", "http://www.linkedin.com/changethis", null, null, null, null);

            var portfolioEntryIds = coll.AllKeys.Where(k => k.StartsWith("Title_")).Select(t => Convert.ToInt32(t.Replace("Title_", ""))).ToList();

            foreach (var id in portfolioEntryIds)
            {
                if (Request.Files["Image_" + id].ContentLength > 0)
                    company.Add_ImageAndDescription_Page(coll["Title_" + id], coll["Description_" + id], Request.Files["Image_" + id]);
            }

            var token = Token.Create(TOKEN_TYPE.MARKETING_INVITE, coll["Username"], Tenant.Id);
            
            DbContext.AddObject(token);
            DbContext.SaveChanges();

            return View<CreateMarketingAccounts>(GetViewName("CreateMarketingAccounts"), v =>
            {
                // can't pass parameters to show a message, so using the Add Error
                v.AddError(MessageCodes.ACCOUNT_CREATED_FOR_MARKETING,
                    string.Format("http://canyoucode.com/Companies/Preview?key={1}", coll["Username"], token.Key),
                    coll["Email"]);
            });
        }

        public ActionResult SendAlerts()
        {
            return View<SendAlerts>(GetViewName("SendAlerts"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendAlerts(string type, string message)
        {
            AccountAlert.Send(type, message, Tenant.Id);

            return View<SendAlerts>(("SendAlerts"), v => { v.MessageCode = MessageCodes.ALERT_MESSAGE_SENT; });
        }
    }
}
