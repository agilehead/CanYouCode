using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using canyoucode.Core.Models;
using canyoucode.Web.Utils;

namespace canyoucode.Web.ViewModels.Companies
{
    public class ViewItem : CanYouCodeViewModel
    {
        public Company Company { get; set; }
        public string Key { get; set; }
    }
}