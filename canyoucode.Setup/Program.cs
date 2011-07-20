using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;

using AgileFx;
using AgileFx.ORM;
using AgileFx.Security;
using System.Web;

using canyoucode.Core;
using canyoucode.Core.Models;
using canyoucode.Core.Utils;
using canyoucode.Core.Utils;

namespace canyoucode.Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            DataContext.Configure("Data Source=.;Initial Catalog=CanYouCodeDb;Integrated Security=True;Connect Timeout=10;User Instance=False");

            Console.WriteLine(DateTime.Now.ToString());

            Init();
            var program = new Program();
            program.SetupDB();
            Console.WriteLine(DateTime.Now.ToString());

            program.CreateEntities();

            Console.WriteLine(DateTime.Now.ToString());
        }

        public void CreateEntities()
        {
            CreateAdmin();
            CreateTags();
            CreateEmployerAndProject();
            CreateCompany();
            CreateBids();
        }

        private void CreateAdmin()
        {
            var context = DataContext.Get();
            //goldenbr0wn
            var account = Admin.Create("sativa", "services@agilehead.com", "123123123", 1);
            context.AddObject(account);
            context.SaveChanges();
        }

        private void CreateTags()
        {
            var context = DataContext.Get();

            var tags = new[] {
                "Android",
                "Application Development:Application_Development",
                "Asp.net:aspdotnet",
                "Asp.net-mvc:aspdotnet_mvc",
                "Clojure",
                "Cocoa",
                "Cocoa touch:Cocoa_touch",
                "C++:cplusplus",
                "C#:csharp",
                "Delphi",
                "Django",
                "Drupal",
                ".Net Framework:dotNet_Framework",
                "Erlang",
                "Facebook",
                "Flash",
                "Flex",
                "Google App Engine:GAE",
                "Hadoop",
                "HTML",
                "IOS",
                "Ipad",
                "Iphone",
                "Java",
                "Javascript",
                "Jsp",
                "LISP",
                "Mono",
                "MySql",
                "Objective-C:objective_c",
                "Oracle",
                "Perl",
                "PHP",
                "Python",
                "Ruby",
                "Ruby on Rails:Ruby_on_Rails",
                "Scala",
                "Sharepoint",
                "Silverlight",
                "SQL Server:SQL_Server",
                "Swing",
                "WCF",
                "Windows Phone 7:Windows_Phone_7",
                "Wordpress",
                "Web Design:Web_Design"
            };

            tags.Distinct().ToList().ForEach(t =>
            {
                var tInfo = t.Split(new[] { ':' });

                var name = tInfo[0];
                var slug = tInfo.Length > 1 ? tInfo[1] : tInfo[0];
                Tag.Create(name, slug, 1, context);
            });

            context.SaveChanges();
        }

        private void CreateCompany()
        {
            var context = DataContext.Get();

            var tags = context.Tag.ToList();
            //on server - rand0mstr1ng

            var company1 = Company.Create("See Fonts [Sample]", "http://SeeFonts.com", "Montana", "United States",
                "seefonts", "123123123", 35, "USD", "FranklinMB@example.com", "406 281 5341", COMPANY_TYPE.COMPANY,1, context);
            var consultant1 = company1.AddConsultant("Franklin M Burnett", "Sr. Designer", "http://www.example.com/FranklinMBurnett", null, null, null, null);
            company1.Description = "This is not a real company. This is just sample data intented to demonstrate application features.";
            company1.AddConsultant("Natasha Bjorklund", "Sr. Engineer", "http://www.example.com/NatashaBjorklund", null, null, null, null);
            company1.Tags.Add(tags.Single(t => t.Name == "IOS"));
            company1.Tags.Add(tags.Single(t => t.Name == "Ipad"));
            company1.Tags.Add(tags.Single(t => t.Name == "Iphone"));
            company1.Tags.Add(tags.Single(t => t.Name == "Objective-C"));
            company1.Tags.Add(tags.Single(t => t.Name == "Ruby on Rails"));
            company1.Logo = "/public/company/samples/icon1.png";

            var company2 = Company.Create("Gaming Problem [Sample]", "http://GamingProblem.com", "Avoca", "United States",
                "gamingproblem", "123123123", 65, "USD", "MarkLS@example.com", "570 997 1647", COMPANY_TYPE.COMPANY,1, context);
            var consultant2 = company2.AddConsultant("Mark L Sanders", "Engineer", "http://www.example.com/MarkLSanders", null, null, null, null);
            company2.Description = "This is not a real company. This is just sample data intented to demonstrate application features.";
            company2.AddConsultant("Ruth L Edwards", "Engineer", "http://www.example.com/RuthLEdwards", null, null, null, null);
            company2.Tags.Add(tags.Single(t => t.Name == "C#"));
            company2.Tags.Add(tags.Single(t => t.Name == "Silverlight"));
            company2.Tags.Add(tags.Single(t => t.Name == "Windows Phone 7"));
            company2.Tags.Add(tags.Single(t => t.Name == "SQL Server"));
            company2.Logo = "/public/company/samples/icon2.png";

            var company3 = Company.Create("Green Projector [Sample]", "http://GreenProjector.com", "Northbrook", "United States",
                "greenprojector", "123123123", 85, "USD", "sbass@example.com", "708 650 6481", COMPANY_TYPE.COMPANY,1, context);
            var consultant3 = company3.AddConsultant("Sonia Bass", "Co-Founder", "http://www.example.com/Soniabass", null, null, null, null);
            company3.Description = "This is not a real company. This is just sample data intented to demonstrate application features.";
            company3.AddConsultant("Margaret J Jones", "Consultant", "http://www.example.com/MargaretJJones", null, null, null, null);
            company3.Tags.Add(tags.Single(t => t.Name == "Django"));
            company3.Tags.Add(tags.Single(t => t.Name == "Python"));
            company3.Tags.Add(tags.Single(t => t.Name == "PHP"));
            company3.Tags.Add(tags.Single(t => t.Name == "Perl"));
            company3.Tags.Add(tags.Single(t => t.Name == "Drupal"));
            company3.Tags.Add(tags.Single(t => t.Name == "MySql"));
            company3.Logo = "/public/company/samples/icon3.png";

            var company4 = Company.Create("Willard M. Green [Sample]", "http://willardgreeen.com", "Lexington", "United States",
                "wmgreeen", "123123123", 120, "USD", "willardgreeen@example.com", "605 654 2006", COMPANY_TYPE.INDIVIDUAL,1 , context);
            var consultant4 = company4.AddConsultant("Willard M. Green", "Sr. UX Engineer", "http://www.example.com/willardgreen", null, null, null, null);
            company4.Description = "This is not a real company. This is just sample data intented to demonstrate application features.";
            company4.Tags.Add(tags.Single(t => t.Name == "C++"));
            company4.Tags.Add(tags.Single(t => t.Name == "Scala"));
            company4.Tags.Add(tags.Single(t => t.Name == "Java"));
            company4.Tags.Add(tags.Single(t => t.Name == "Erlang"));
            company4.Tags.Add(tags.Single(t => t.Name == "Android"));
            company4.Tags.Add(tags.Single(t => t.Name == "Hadoop"));
            company4.Logo = "/public/company/samples/icon4.png";

            context.SaveChanges();
        }

        private void CreateEmployerAndProject()
        {
            var context = DataContext.Get();
            var tags = context.Tag.ToList();
            //on server - rand0mstr1ng
            var employer = Employer.Create("BounceThru Inc [Sample]", "Chicago", "United States", "bouncethru", "123123123",
                "bouncethru@example.com", "847 377 6284", 1, context);
            employer.IsVerified = true;

            var project1 = employer.CreateProject("Duke Nukem Never Ever [Sample]",
                @"Duke Nukem Forever is an upcoming first-person shooter video game for the Windows PC, Xbox 360 and Playstation 3 systems [6] , currently in development by Gearbox Software, and a sequel to the 1996 game Duke Nukem 3D, as part of the long-running Duke Nukem video game series. Intended to be groundbreaking, it has become notorious in the video games industry for its severely-protracted development schedule; the game has been in development since 1997. 3D Realms and director George Broussard, one of the creators of the original Duke Nukem game, first announced the title's development in April 1997, and promotional information for the game was released in one form or another in 1997 to 2008. This information, including screenshots, showed different looks for the game, as 3D Realms was constantly changing game engines and graphics.",
                10000, "USD", DateTime.Now.AddDays(30), new long[] { 3, 5, 7, 15 },null, null);

            var project2 = employer.CreateProject("Create a LISP Machine [Sample]",
                @"Lisp machines were general-purpose computers designed (usually through hardware support) to efficiently run Lisp as their main software language. In a sense, they were the first commercial single-user workstations. Despite being modest in number (perhaps 7,000 units total as of 1988[1]), Lisp machines commercially pioneered many now-commonplace technologies — including effective garbage collection, laser printing, windowing systems, computer mice, high-resolution bit-mapped graphics, computer graphic rendering, and networking innovations like CHAOSNet. Several companies were building and selling Lisp Machines in the 1980s: Symbolics (3600, 3640, XL1200, MacIvory and other models), Lisp Machines Incorporated (LMI Lambda), Texas Instruments (Explorer and MicroExplorer) and Xerox (InterLisp-D workstations). The operating systems were written in Lisp Machine Lisp, InterLisp (Xerox) and later partly in Common Lisp.",
                3000, "USD", DateTime.Now.AddDays(30), new long[] { 1, 2, 4 }, null, null);

            var project4 = employer.CreateProject("Porting the Symbolics OS [Sample]",
                @"Symbolics, Inc.[2] was a computer manufacturer headquartered in Cambridge, Massachusetts and later in Concord, Massachusetts, with manufacturing facilities in Chatsworth, California (a suburb of Los Angeles). Its first CEO, chairman, and founder was Russell Noftsker.[3] Symbolics designed and manufactured a line of Lisp machines, single-user computers optimized to run the Lisp programming language. Symbolics also made significant advances in software technology, and offered one of the premier software development environments of the 1980s and 1990s, now sold commercially as Open Genera for Tru64 UNIX on the HP Alpha. The Lisp Machine was the first commercially available (although that word had not yet been coined).",
                5000, "USD", DateTime.Now.AddDays(30), new long[] { 11, 9, 13, 14 }, null, null);

            var project3 = employer.CreateProject("An MS-DOS Clone [Sample]",
                @"DOS is a single-user, single-task operating system with basic kernel functions that are non-reentrant: only one program at a time can use them. There is an exception with Terminate and Stay Resident (TSR) programs, and some TSRs can allow multitasking. However, there is still a problem with the non-reentrant kernel: once a process calls a service inside of operating system kernel (system call), it must not be interrupted with another process calling system call, until the first call is finished.",
                7000, "USD", DateTime.Now.AddDays(30), new long[] { 8, 12, 10 }, null, null);

            foreach (var project in new[] { project1, project2, project3, project4 })
                project.Attachments.Add(CreateAttachment("/Public/Project/Test.doc"));

            context.AddObject(employer);
            context.SaveChanges();
        }

        private void CreateBids()
        {
            var context = DataContext.Get();

            var marcose = context.Employer.LoadRelated(e => e.Projects).Single(e => e.Account.Username == "bouncethru");

            int i = 0;
            foreach (var project in marcose.Projects)
            {
                var companies = context.Company;
                foreach (var company in companies)
                {
                    if (i == 0)
                    {
                        //skip. Company.Create auto adds this.
                    }
                    else if (i < 3)
                    {

                        company.PlaceBid(project.Id, 150, TIMEFRAME.MONTHS_3, 50000, 100000,
                            "We are the sheep and kettle on the technology you are looking for.");
                    }
                    else
                    {
                        project.Invite(company);
                    }
                }
                i++;
            }
            context.SaveChanges();

            var seefontsbid = context.Bid.LoadRelated(b => b.Project).Single(b => b.Company.Account.Username == "seefonts" && b.Project.Title.StartsWith("Duke Nukem Never Ever"));
            seefontsbid.Status = BID_STATUS.ACCEPTED;
            seefontsbid.Project.Status = PROJECT_STATUS.CLOSED;

            var gamingproblembid = context.Bid.LoadRelated(b => b.Project).Single(b => b.Company.Account.Username == "gamingproblem" && b.Project.Title.StartsWith("Porting the Symbolics OS"));
            gamingproblembid.Project.Status = PROJECT_STATUS.CLOSED;
            gamingproblembid.Status = BID_STATUS.ACCEPTED;
            context.SaveChanges();

        }


        private static Attachment CreateAttachment(string path)
        {
            var attachment = new Attachment();
            attachment.DateAdded = DateTime.Now;
            attachment.OriginalFileName = System.IO.Path.GetFileName(path);
            attachment.Url = path;
            attachment.Token = Guid.NewGuid();

            return attachment;
        }

        private void SetupDB()
        {
            var connectionString = Properties.Settings.Default.canyoucodeDB;
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            ServerConnection svrConnection = new ServerConnection(sqlConnection);
            ExecuteSqlFile(svrConnection, "../../../canyoucode.Core/DbScripts/killdb.sql");
            ExecuteSqlFile(svrConnection, "../../../canyoucode.Core/DBScripts/CanYouCodeCreateDB.sql");
            ExecuteSqlFile(svrConnection, "../../../canyoucode.Core/DbScripts/PreFill.sql");
            Console.WriteLine("Database setup complete.....");
        }

        private void ExecuteSqlFile(ServerConnection connection, string filePath)
        {
            connection.ExecuteNonQuery(System.IO.File.ReadAllText(filePath));
        }

        static void Init()
        {
            NotificationUtil.Init(ConfigurationManager.AppSettings["SMTP_HOST"],
                ConfigurationManager.AppSettings["SMTP_PORT"],
                ConfigurationManager.AppSettings["MailTemplatesDirectory"]);
        }

    }
}
