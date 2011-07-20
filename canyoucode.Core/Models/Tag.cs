using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using canyoucode.Core.Utils;
using System.Text.RegularExpressions;
using AgileFx.ORM;

namespace canyoucode.Core.Models
{
    public partial class Tag
    {
        static Tag()
        {
            GetAll();
        }

        //will have been initialized in the Application start to avoid threading issues.
        static List<Tag> allTags;
        public static List<Tag> GetAll()
        {
            if (allTags == null)
            {
                var context = DataContext.Get();
                allTags = context.Tag.ToList();
            }
            return allTags;
        }

        public static void Create(string name, string slug, long tenantId, EntityContext context)
        {
            var tag = new Tag();

            tag.Name = name;
            tag.Slug = slug;
            tag.TenantId = tenantId;
            context.AddObject(tag);

            NotificationUtil.SendAdminEmail(string.Format("New Tag - Name:{0} Slug:{1}", name, slug), "");
        }

        public static Tag Create(string name, long tenantId, EntityContext context)
        {
            var tag = new Tag();

            name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name.Trim().ToLowerInvariant());
            var slug = new Regex("[a-zA-Z_]*").Match(name.Replace(" ", "_")).Value;

            tag.Name = name.Trim();
            tag.Slug = slug;
            tag.TenantId = tenantId;
            context.AddObject(tag);

            NotificationUtil.SendAdminEmail(string.Format("New Tag - Name:{0} Slug:{1}", name, slug), "");
            return tag;
        }
    }
}
