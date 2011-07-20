using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.Utils;

namespace canyoucode.Web.Utils
{
    public class AjaxResponse<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string MessageCode { get; set; }
        public string Message
        {
            get
            {
                return JS.String(AgileFx.I18n.GetString(MessageCode));
            }
        }
    }
}