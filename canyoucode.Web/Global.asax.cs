using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using canyoucode.Core.Utils;
using System.Configuration;
using canyoucode.Core.Models;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace canyoucode.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("Public/{*pathInfo}");
            routes.IgnoreRoute("Styles/{a}/{b}");

            routes.MapRoute(
                "HomePage", // Route name
                "", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );

            //This handles the http://www.canyoucode.com/company vanity url
            routes.Add("Portfolio", new VanityRoute("{username}",
                new { action = "ViewItem" }));

            //This handles the http://www.canyoucode.com/login vanity url
            routes.MapRoute("RootUrls", "{action}", new { controller = "Home" });

            //This handles the http://www.canyoucode.com/company/action vanity url
            routes.Add("CompaniesOrEmployersAction", new VanityRoute("{username}/{action}"));

            //This handles the http://www.canyoucode.com/company/projects/lost vanity url
            routes.Add("CompaniesOrEmployersActionWithFilter",  new VanityRoute("{username}/{action}/{filter}"));

            //tag search
            routes.MapRoute(
                "tag-search",
                "Search/{action}/{type}/{searchkey}",
                new { controller = "Search", action = "", type = "", searchkey = "" }
            );

            //This handles /projects/1/edit
            routes.MapRoute("generic route (id with action)", "{controller}/{id}/{action}", 
                new { action = "ViewItem" },
                new { id = @"\d+" } );

            //This handles /controller/action
            routes.MapRoute("generic route", "{controller}/{action}");
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles

                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }

        protected void Application_Start()
        {
            //For azure storage
            CloudStorageAccount.SetConfigurationSettingPublisher(
                (configName, configSettingPublisher) =>
                {
                    var connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                    configSettingPublisher(connectionString);
                }
            );


            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            AgileFx.I18n.Configure(() => new canyoucode.Web.I18n.Handler());
            DataContext.Configure(ConfigurationManager.ConnectionStrings["CanYouCodeDb"].ConnectionString);

            NotificationUtil.Init(ConfigurationManager.AppSettings["SMTP_HOST"],
                ConfigurationManager.AppSettings["SMTP_PORT"],
                HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MailTemplatesDirectory"]));

            Tenant.GetAll();
        }
    }
}