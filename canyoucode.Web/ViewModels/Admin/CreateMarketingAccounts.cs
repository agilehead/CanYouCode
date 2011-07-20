using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx.MVC.ViewModels;
using System.Web.Mvc;
using AgileFx.MVC.Controls;
using canyoucode.Web.Utils;

namespace canyoucode.Web.ViewModels.Admin
{
    public class CreateMarketingAccounts : CanYouCodeViewModel
    {
        public IEnumerable<SelectListItem> Countries { get; set; }
        public IEnumerable<SelectListItem> Currency { get; set; }
        public IEnumerable<SelectListItem> MinimumRates { get; set; }

        public CreateMarketingAccounts()
        {
            Countries = UIHelper.GetCountrySelectItemList(null);
            Currency = UIHelper.GetCurrencySelectItemList(null);
            MinimumRates = UIHelper.GetMinimumRatesSelectItemList(null);
        }
    }
}