using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using AgileFx.MVC.Controls;
using canyoucode.Web.Utils;
using canyoucode.Core.Models;

namespace canyoucode.Web.ViewModels.Employers
{
    public class Edit : CanYouCodeViewModel
    {
        public Employer Employer { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public Edit()
        {
            Countries = UIHelper.GetCountrySelectItemList(null);
        }
    }
}