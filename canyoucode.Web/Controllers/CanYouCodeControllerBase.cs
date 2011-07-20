using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.ORM;
using AgileFx.MVC.Controllers;

using canyoucode.Web.ViewModels;
using m = canyoucode.Core.Models;
using System.Web.Mvc;
using canyoucode.Core.Models;
using canyoucode.Core.Utils;
using canyoucode.Core;

namespace canyoucode.Web.Controllers
{
    public class CanYouCodeControllerBase : DefaultController<CanYouCodeViewModel>
    {
        public CanYouCodeControllerBase()
        {
            UpdateDefaultViewModel(v =>
            {
                v.TopMenuSelectedItem = Utils.TopMenuSelection.NONE;
                v.LoggedInAccount = this.LoggedInAccount;
                v.ShowFixedSidePane = true;

                if (this.LoggedInAccount != null)
                {
                    var alerts = DbContext.AccountAlert
                        .Where(aa => aa.AccountId == LoggedInAccount.Id && aa.Status != ALERT_STATUS.VIEWED);
                    var alerttoShow = alerts.FirstOrDefault();
                    if (alerttoShow != null)
                    {
                        alerttoShow.MarkViewed();
                        DbContext.SaveChanges();
                        v.AccountAlert = alerttoShow.Message;
                    }
                }
            });
        }

        public Entities DbContext
        {
            get
            {
                if (HttpContext.Items[ContextKeys.DbContext] == null)
                    HttpContext.Items[ContextKeys.DbContext] = DataContext.Get();

                return HttpContext.Items[ContextKeys.DbContext] as Entities;
            }
        }

        public TenantStore Tenant { get; set; }
        public m.Account LoggedInAccount { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //Tiny optimization.
                //Many times, the vanity url route handler (VanityRoute) would have already loaded the account.
                //  If so, do not load again.

                var account = HttpContext.Items[ContextKeys.RequestedAccount] as Account;

                //If account isn't null and is the same as User.Identity.Name, don't load again.
                //Otherwise Load.
                this.LoggedInAccount = (account != null && account.Username == HttpContext.User.Identity.Name) ?
                        account : DbContext.Account.Where(u => u.Username == HttpContext.User.Identity.Name).SingleOrDefault();
            }

            //Set Tenant.Id.
            var host = Request.Headers["host"];
            if (!host.Contains("http://")) host = "http://" + host;
            var uri = new Uri(host);

            var domain = uri.Host;

            var tenants = m.Tenant.GetAll();
            this.Tenant = tenants.Single(t => t.DomainName == uri.Host);

            base.OnActionExecuting(filterContext);
        }

        public string GetViewName(string name)
        {
            string controller = this.GetType().Name.ToLower().Replace("controller", "");
            return string.Format("/Views/{0}/{1}/{2}.aspx", Tenant.ViewStore, controller, name);
        }
    }
}