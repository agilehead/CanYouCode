using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Utils;
using System.Web;

using AgileFx.ORM;

namespace canyoucode.Core.Models
{
    public partial class PortfolioEntry
    {
        public void SaveImage(HttpPostedFileBase file)
        {
            var url = FileUtil.SaveResizedImage(file, "portfolio", 564, 2000, true);
            this.Image = url;

            FileUtil.SaveResizedImage(file, "portfolio", 800, 2000, false);
        }

        public string Image_564
        {
            get { return FileUtil.GetImage(564, this.Image); }
        }

        public string Image_800
        {
            get { return FileUtil.GetImage(800, this.Image); }
        }

        public string GetDescription(Account account)
        {
            //if (!this.IsLoaded(c => c.Company.Account)) this.Load(EntityContext, c => c.Company.Account);

            if (string.IsNullOrEmpty(Description) && account != null && account.Type == ACCOUNT_TYPE.COMPANY
                && account.Username == this.Company.Account.Username)
            {
                return string.Format("<a style='color:gray' href='/{0}/Edit#Portfolio{1}'>Add Description</a>", this.Company.Account.Username, this.Id);
            }
            else return HttpUtility.HtmlEncode(Description).Replace("\r\n", "<br />");
 
        }
    }
}
