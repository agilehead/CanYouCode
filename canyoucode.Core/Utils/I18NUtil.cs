using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace canyoucode.Core.Utils
{
    public static class I18NUtil
    {
        public static string GetCurrencyChar(string currency)
        {
            if (currency == "USD")
                return "$";
            if (currency == "EUR")
                return "€";
            if (currency == "GBP")
                return "£";
            return currency;
        }
    }
}
