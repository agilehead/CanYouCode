using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using canyoucode.Core.Models;
using canyoucode.Core;
using canyoucode.Core.Utils;

namespace canyoucode.Web
{
    //Vanity route makes sexy urls for Companies and Employers
    //Such as   http://www.canyoucode.com/microsoft
    // or       http://www.canyoucode.com/microsoft/projects
    public class VanityRoute : Route
    {
        public VanityRoute(string url)
            : base(url, new MvcRouteHandler())
        {
        }

        public VanityRoute(string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
        }

        public VanityRoute(string url, object defaults, object constraints)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler())
        {
        }

        public VanityRoute(string url, object defaults, object constraints, object dataTokens)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens), new MvcRouteHandler())
        {
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Items[ContextKeys.DbContext] == null)
                httpContext.Items.Add(ContextKeys.DbContext, DataContext.Get());

            var context = httpContext.Items[ContextKeys.DbContext] as Entities;

            var routeData = base.GetRouteData(httpContext);
            if (null == routeData) return routeData;

            if (routeData.Values["username"] != null)
            {
                if (httpContext.Items[ContextKeys.RequestedAccount] == null ||
                    ((Account)httpContext.Items[ContextKeys.RequestedAccount]).Username != (string)routeData.Values["username"])
                {
                    httpContext.Items[ContextKeys.RequestedAccount] = context.Account.Where(a => a.Username == (string)routeData.Values["username"]).SingleOrDefault();
                }

                Account account = httpContext.Items[ContextKeys.RequestedAccount] as Account;

                if (account != null)
                {
                    if (account.Type == ACCOUNT_TYPE.EMPLOYER)
                        routeData.Values["controller"] = "Employers";
                    else if (account.Type == ACCOUNT_TYPE.COMPANY)
                        routeData.Values["controller"] = "Companies";

                    httpContext.Items[ContextKeys.RequestedAccount] = account;

                    return routeData;
                }
            }

            //Not a vanity url. Pass.
            return null;
        }
    }
}
