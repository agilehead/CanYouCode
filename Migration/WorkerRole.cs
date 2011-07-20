using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using canyoucode.Core.Models;
using System.Configuration;
using System.IO;
using AgileFx.ORM;

namespace Migration
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            Rename();
            //CreatePortolio_800();

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        private string GetCompletePath(string path, bool getOriginal)
        {
            string suffix = @"C:\Projects\canyoucode\canyoucode.Web";
            path = path.Replace("/", @"\");
            string compPath = suffix + path;

            if (getOriginal)
            {
                var directory = Path.GetDirectoryName(compPath);
                var fileNameWithoutExtn = Path.GetFileNameWithoutExtension(compPath);
                var extn = Path.GetExtension(compPath);

                return Path.Combine(directory, fileNameWithoutExtn + "-original" + extn);
            }
            else return compPath;
        }

        private string GetContentType(string path)
        {
            var extn = Path.GetExtension(path).ToLower();
            if (extn.Equals(".jpg") || extn.Equals(".jpeg"))
                return "image/jpeg";
            else if (extn.Equals(".png"))
                return "image/png";
            else if (extn.Equals(".gif"))
                return "image/gif";
            else throw new NotImplementedException();
        }

        private void Rename()
        {
            Entities Entities = new Entities(ConfigurationManager.ConnectionStrings["canyoucodedb"].ConnectionString);
            var excludeLogs = new[] { "/images/company-logo.png", "/images/profile-pic.png" };

            var companiesToModify = Entities.Company.Where(c => !excludeLogs.Contains(c.Logo.ToLower())).ToList();
            companiesToModify.ForEach(c =>
                {
                    var oldFilename = Path.GetFileName(c.Logo);
                    var extn = Path.GetExtension(c.Logo);

                    var new80Filename = oldFilename.Replace(extn, "-80" + extn);
                    CYCBlobHelper.Rename("company", oldFilename, new80Filename);

                    var originalFilename = oldFilename.Replace(extn, "-original" + extn);
                    var newOriginalFilename = oldFilename;
                    CYCBlobHelper.Rename("company", originalFilename, newOriginalFilename);
                });

            var excludePics = new[] { "/images/profile-pic.png" };
            var consultantsToModify = Entities.Consultant.Where(c => !excludePics.Contains(c.Picture.ToLower())).ToList();
            consultantsToModify.ForEach(c =>
                {
                    var oldFilename = Path.GetFileName(c.Picture);
                    var extn = Path.GetExtension(c.Picture);

                    var new80Filename = oldFilename.Replace(extn, "-80" + extn);
                    CYCBlobHelper.Rename("profile", oldFilename, new80Filename);

                    var originalFilename = oldFilename.Replace(extn, "-original" + extn);
                    var newOriginalFilename = oldFilename;
                    CYCBlobHelper.Rename("profile", originalFilename, newOriginalFilename);
                });

            var excludePortfolios = new[] { "/images/page-placeholder.png", "/images/page-placeholder2.png", "/images/page-placeholder3.png" };
            var portfoliosToModify = Entities.PortfolioEntry.Where(c => !excludePortfolios.Contains(c.Image.ToLower())).ToList();
            portfoliosToModify.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.Image))
                {
                    var oldFilename = Path.GetFileName(p.Image);
                    var extn = Path.GetExtension(p.Image);

                    var new80Filename = oldFilename.Replace(extn, "-564" + extn);
                    CYCBlobHelper.Rename("portfolio", oldFilename, new80Filename);

                    var originalFilename = oldFilename.Replace(extn, "-original" + extn);
                    var newOriginalFilename = oldFilename;
                    CYCBlobHelper.Rename("portfolio", originalFilename, newOriginalFilename);
                }
            });
        }

        private void CreatePortolio_800()
        {
            Entities Entities = new Entities(ConfigurationManager.ConnectionStrings["canyoucodedb"].ConnectionString);
            
            var excludePortfolios = new[] { "/images/page-placeholder.png", "/images/page-placeholder2.png", "/images/page-placeholder3.png" };
            var portfoliosToModify = Entities.PortfolioEntry.Where(c => !excludePortfolios.Contains(c.Image.ToLower())).ToList();

            portfoliosToModify.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.Image))
                {
                    var blob = CYCBlobHelper.GetBlob("portfolio", p.Image);
                    if (blob != null) FileUtil.SaveResizedImage(blob, 800, 2000);
                }
            });         
        }

        private void MigrateImagesToBlob()
        {
            Entities Entities = new Entities(ConfigurationManager.ConnectionStrings["canyoucodedb"].ConnectionString);

            //Company
            var excludeLogs = new [] { "/images/company-logo.png", "/images/profile-pic.png"};
            var companiesToModify = Entities.Company.Where(c => !c.Logo.ToLower().StartsWith("http://")
                && !excludeLogs.Contains(c.Logo.ToLower())).ToList();
            companiesToModify.ForEach(c =>
                {
                    var currentLogo = GetCompletePath(c.Logo, true);
                    c.Logo = FileUtil.SaveResizedImage(currentLogo, GetContentType(currentLogo), "company", 80, 80);
                });
            Entities.SaveChanges();

            //Consultant
            var excludePics = new[] { "/images/profile-pic.png" };
            var consultantsToModify = Entities.Consultant.Where(c => !c.Picture.ToLower().StartsWith("http://")
                && !excludePics.Contains(c.Picture.ToLower())).ToList();
            consultantsToModify.ForEach(c =>
                {
                    var currentPic = GetCompletePath(c.Picture, true);
                    c.Picture = FileUtil.SaveResizedImage(currentPic, GetContentType(currentPic), "profile", 80, 80);
                });
            Entities.SaveChanges();

            //PortfolioEntry
            var excludePortfolios = new[] { "/images/page-placeholder.png", "/images/page-placeholder2.png", "/images/page-placeholder3.png" };
            var portfoliosToModify = Entities.PortfolioEntry.Where(c => !c.Image.ToLower().StartsWith("http://")
                && !excludePortfolios.Contains(c.Image.ToLower())).ToList();
            portfoliosToModify.ForEach(c =>
            {
                var currentImage = GetCompletePath(c.Image, true);
                c.Image = FileUtil.SaveResizedImage(currentImage, GetContentType(currentImage), "portfolio", 564, 2000);
            });
            Entities.SaveChanges();

            //Attachment
            var excludeDocs = new[] { "/Public/Project/Test.doc" };
            var docsToModify = Entities.Attachment.Where(c => !c.Url.ToLower().StartsWith("http://")
                && !excludeDocs.Contains(c.Url.ToLower())).ToList();
            docsToModify.ForEach(c =>
            {
                var currentDoc = GetCompletePath(c.Url, false);
                c.Url = FileUtil.SaveFile(currentDoc, "application/msword", "project");
            });
            Entities.SaveChanges();
        }

        private static string PUBLIC_DATA_STORE = "PublicDataStore";

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            // This code sets up a handler to update CloudStorageAccount instances when their corresponding
            // configuration settings change in the service configuration file.
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                // Provide the configSetter with the initial value
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));

                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        // The corresponding configuration setting has changed, propagate the value
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            // In this case, the change to the storage account credentials in the
                            // service configuration is significant enough that the role needs to be
                            // recycled in order to use the latest settings. (for example, the 
                            // endpoint has changed)
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
