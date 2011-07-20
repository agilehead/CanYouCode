using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgileFx.MVC.Controls;
using canyoucode.Core.Models;

namespace canyoucode.Web.Utils
{
    public static class UIHelper
    {
        //Find what is in the brackets, if there are any.
        static Func<string, string> valueGetter = c => !c.Contains(",") ? c : c.Split(',')[1];
        static Func<string, string> displayGetter = c => !c.Contains(",") ? c : c.Split(',')[0];

        static string[] countries = new string[] { "United States", "England", "Canada", "France", "Germany", "Spain", "Belgium", 
            "Italy", "Denmark", "Sweden", "Ireland", "Portugal", "Australia", "Austria", "Switzerland", "Hungary", "Netherlands", "Iceland",
            "New Zealand", "Scotland", "Northern Ireland", "Wales"};

        public static string ALL_COUNTRIES = "All Countries";

        public static IEnumerable<SelectListItem> GetCountrySelectItemList(string selected)
        {
            return HtmlUtil.GetSelectItems(countries.OrderBy(c => c), valueGetter, displayGetter, i => valueGetter(i) == (selected ?? "United States"));
        }

        public static IEnumerable<SelectListItem> GetCountryWithAllSelectItemList(string selected)
        {
            var allCountries = new List<string>(countries);
            allCountries.Add(ALL_COUNTRIES);
            return HtmlUtil.GetSelectItems(allCountries.OrderBy(c => c), valueGetter, displayGetter, i => valueGetter(i) == (selected ?? ALL_COUNTRIES));
        }

        public static IEnumerable<SelectListItem> GetCurrencySelectItemList(string selected)
        {
            var currency = new string[] { "$" };
            return HtmlUtil.GetSelectItems(currency, valueGetter, displayGetter, i => valueGetter(i) == (selected ?? "$"));
        }

        public static IEnumerable<SelectListItem> GetMinimumRatesSelectItemList(string selected)
        {
            var minimumRates = new[] { "35", "40", "45", "50", "55", "60", "70", "80", "100", "120", "150", "200", "250", "Unspecified" };
            return HtmlUtil.GetSelectItems(minimumRates, valueGetter, displayGetter, i => valueGetter(i) == (selected ?? "35"));
        }

        public static IEnumerable<SelectListItem> GetBidQuoteSelectItemList()
        {
            var bids = new[] {"Not specified:", "less than 1,000:0-1000", "1,000 - 2,000:1000-2000", "2,000 - 5,000:2000-5000", "5,000 - 10,000:5000-10000",
            "10,000 - 50,000:10000-50000", "50,000 - 100,000:50000-100000", "100,000 - 200,000:100000-200000"};
            return HtmlUtil.GetSelectItems(bids, b => b.Split(':')[1], b => b.Split(':')[0]);
        }

        public static IEnumerable<SelectListItem> GetProjectBudgetSelectItemList(int selectedValue)
        {
            var budgets = new[] { "1,000", "2,000", "3,000", "5,000", "7,000", "10,000", "15,000", "25,000", "50,000", "75,000", "100,000", "150,000",
                "200,000", "300,000" };
            return HtmlUtil.GetSelectItems(budgets, b => b.Replace(",", ""), b => b, s => s.Replace(",", "") == selectedValue.ToString());
        }

        public static IEnumerable<SelectListItem> GetTimeFramesSelectItemList(string selected)
        {
            var timeFrames = Enumerable.Range(2, 22).Select(x => x.ToString()).ToList();
            timeFrames.Add("More than 24");
            return HtmlUtil.GetSelectItems(timeFrames, valueGetter, displayGetter, i => valueGetter(i) == (selected ?? "2"));
        }

        public static string GetHTMLDisplayTag(Tag tag, string type)
        {
            return "<span class='skill'>" + MvcHtmlString.Create(tag.Name) + "</span>";
            //return string.Format("<a class='skill' href='/Search/Tags/{0}/{1}'>{2}</a>", type, tag.Slug, tag.Name);
        }
    }
}