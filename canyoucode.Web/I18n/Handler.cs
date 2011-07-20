using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgileFx;

namespace canyoucode.Web.I18n
{
    public class Handler : II18nHandler
    {
        static MessageTexts texts = new MessageTexts();

        #region II18nHandler Members

        public string GetString(string code)
        {
            try
            {
                return texts[code];
            }
            catch { return code; }
        }

        #endregion
    }
}