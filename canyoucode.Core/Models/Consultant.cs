using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileFx.ORM;
using System.Web;
using canyoucode.Core.Utils;

namespace canyoucode.Core.Models
{
    public partial class Consultant
    {
        public void SavePicture(HttpPostedFileBase image)
        {
            if (image.ContentLength > 0)
                Picture = FileUtil.SaveResizedImage(image, "profile", 80, 80, true);
        }

        public string Picture_80
        {
            get { return FileUtil.GetImage(80, this.Picture); }
        }

        public void Update(string fullName, string designation, string linkedIn, string blog, string hackernews, string stackoverflow, string github, HttpPostedFileBase image)
        {
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                Name = fullName;
            }
            Designation = designation;
            LinkedinProfile = linkedIn;
            SavePicture(image);
            UpdateCredential(CREDENTIAL_TYPE.BLOG, blog);
            UpdateCredential(CREDENTIAL_TYPE.GITHUB, github);
            UpdateCredential(CREDENTIAL_TYPE.HACKERNEWS, hackernews);
            UpdateCredential(CREDENTIAL_TYPE.STACKOVERFLOW, stackoverflow);

            if (Company.Type == COMPANY_TYPE.INDIVIDUAL)
                Company.SaveLogo(image);
        }

        public void AddCredential(string type, string link)
        {
            var cred = new Credential();
            cred.Type = type;
            cred.Link = link;
            cred.TenantId = TenantId;

            this.Credentials.Add(cred);
        }

        public void UpdateCredential(string type, string link)
        {
            if (!this.IsLoaded(c => c.Credentials)) this.Load(EntityContext, c => c.Credentials);
            
            var cred = this.Credentials.SingleOrDefault(c => c.Type == type);
            if (cred == null)
            {
                cred = new Credential();
                cred.Type = type;
                cred.ConsultantId = Id;
                EntityContext.AddObject(cred);
            }

            if (!string.IsNullOrEmpty(link))
            {
                cred.Link = link;
            }
            else 
            {
                EntityContext.DeleteObject(cred);
            }

        }

        public string Blog
        {
            get
            {
                return GetLink(CREDENTIAL_TYPE.BLOG);
            }
        }

        public string Github
        {
            get
            {
                return GetLink(CREDENTIAL_TYPE.GITHUB);
            }
        }

        public string HackerNews
        {
            get
            {
                return GetLink(CREDENTIAL_TYPE.HACKERNEWS);
            }
        }

        public string Stackoverflow
        {
            get
            {
                return GetLink(CREDENTIAL_TYPE.STACKOVERFLOW);
            }
        }

        private string GetLink(string type)
        {
            if (!this.IsLoaded(c => c.Credentials)) this.Load(EntityContext, c => c.Credentials);
            var cred = this.Credentials.SingleOrDefault(c => c.Type == type);
            if (cred != null)
            {
                return cred.Link;
            }
            else return string.Empty;
        }
    }
}
