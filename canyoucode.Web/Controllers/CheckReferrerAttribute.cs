using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CheckReferrerAttribute : ActionFilterAttribute  
{
    public readonly string[] ValidReferrers = new[] { "localhost", "www.canyoucode.com", "canyoucode.com" };

    public override void OnActionExecuting(ActionExecutingContext filterContext)  
    {
        //The second condition handles requests with port values.
        //such as localhost:53455
        if (!ValidReferrers.Any(validRef => filterContext.HttpContext.Request.Headers["HOST"] == validRef ||
                filterContext.HttpContext.Request.Headers["HOST"].StartsWith(validRef + ":")))
            throw new Exception("Cross site request detected. Aborting.");
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)  
    {  
  
    }  
} 