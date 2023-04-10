using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V108.Network;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Xml.Linq;
//using OpenQA.Selenium.Chrome;

namespace SeleniumBot
{
    internal class Program
    {
        //private static string _targetVilla = "405004";//"324834"; //405004,404205
        private static string _server = string.Empty;
        private static string _username = string.Empty;
        private static string _password = string.Empty;
        private static List<dorfVillage> lstDorf = new List<dorfVillage>();
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Validation Failed! Supported Parameter: SeleniumBot.exe <server> <username> <password> <dorf>. Closing in 5 second");
                Thread.Sleep(5000);
                Environment.Exit(0);
            }

            string server = args[0];
            string username = args[1];
            string password = args[2];
            string dorfId = args[3];

            if ((string.IsNullOrEmpty(server)) ||
                (string.IsNullOrEmpty(dorfId)) ||
                (string.IsNullOrEmpty(username)) ||
                (string.IsNullOrEmpty(password)))
            {
                Console.WriteLine("Validation Failed! Supported Parameter: SeleniumBot.exe <server> <username> <password> <dorf>. Closing in 5 second");
                Thread.Sleep(5000);
            }
            else
            {
                _server = server;
                _username = username;
                _password = password;              
            }

            var options = new ChromeOptions();
            //options.AddArgument("--window-size=1920,1080");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--headless");
            //options.AddArgument("--disable-gpu");
            //options.AddArgument("--disable-crash-reporter");
            //options.AddArgument("--disable-extensions");
            //options.AddArgument("--disable-in-process-stack-traces");
            //options.AddArgument("--disable-logging");
            //options.AddArgument("--disable-dev-shm-usage");
            //options.AddArgument("--log-level=3");
            //options.AddArgument("--output=/dev/null");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                Tools.PrintOutput("Starting Headless Chrome");
                Tools.PrintOutput("Param: Server ["+ _server + "] UserId ["+ _username + "] DorfId ["+ dorfId + "]");

                ProfileManagement pm = new ProfileManagement(driver);

                pm.Login();
                Tools.PrintOutput("Logged in");
                Thread.Sleep(Tools.StandardTimeout);
                PlayerInfo.dorfVillages = pm.GetProfileData();
                Thread.Sleep(Tools.StandardTimeout);

                ResourceManagement rm = new ResourceManagement(driver);
                rm.UpdateResources();
                Thread.Sleep(Tools.StandardTimeout);

                //PrintOutput("Processing Dorf Id -> " + dorfId);
                //UpgradeResources(driver, dorfId);


                //foreach (dorfVillage d in lstDorf)
                //{
                //    PrintOutput("Processing Dorf Id -> " + d);
                //    UpgradeResources(driver, d);
                //}
                UpgradeResourcesMonitor(driver);


            }
        }



        static DateTime UpgradeResourcesV2(IWebDriver driver, string dorfId, string buildId)
        {

            return DateTime.Now;
        }

        static void UpgradeResourcesMonitor(IWebDriver driver)
        {
            foreach (dorfVillage villa in lstDorf)
            {
                if (villa.dorfInfos != null)
                {
                    foreach (dorfInfo info in villa.dorfInfos)
                    {
                        if ((info != null) && (info.Level != "10") && (!string.IsNullOrEmpty(villa.VillageId)) && (!string.IsNullOrEmpty(info.Id)))
                        {
                            double freeCheck = 0;
                            while (freeCheck < 60)
                            {
                                DateTime nextFree = UpgradeResourcesV2(driver, villa.VillageId, info.Id);
                                TimeSpan ts = nextFree - DateTime.Now;
                                freeCheck = ts.TotalSeconds;
                            }
                        }
                    }
                }
            }
        }
           
    }
}