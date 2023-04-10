using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBot
{
    public class ProfileManagement
    {
        private IWebDriver driver;
        

        public ProfileManagement(IWebDriver _webDriver)
        {
            driver = _webDriver;
        }

        public List<dorfVillage> GetProfileData()
        {
            List<dorfVillage> dorfVillages = new List<dorfVillage>();

            Tools.PrintOutput("Checking for villages");
            driver.Navigate().GoToUrl(ServerInfo.ServerUrl + "profile/");
            WebElement detailsElem = (WebElement)driver.FindElement(By.Id("details"));
            WebElement villagesElem = (WebElement)driver.FindElement(By.Id("villages"));
            IList<IWebElement> karteCollection = villagesElem.FindElements(By.CssSelector("a[href*='karte.php?d=']"));
            int i = 1;
            foreach (IWebElement element in karteCollection)
            {
                string dorfFull = element.GetAttribute("href");
                string dorfName = element.Text;
                dorfVillages.Add(new dorfVillage
                {
                    VillageId = dorfFull.Substring(dorfFull.Length - 6),
                    VillageName = dorfName
                });
                Tools.PrintOutput("Village No " + i.ToString() + " - " + dorfName);
                i++;
            }
            return dorfVillages;
        }

        public bool Login()
        {
            Thread.Sleep(2000);
            Tools.PrintOutput("Navigate login page");
            driver.Navigate().GoToUrl(ServerInfo.ServerUrl + "login.php");
            Thread.Sleep(2000);
            IWebElement travUser = driver.FindElement(By.Name("name"));
            IWebElement travPass = driver.FindElement(By.Name("password"));
            IWebElement travSubmit = driver.FindElement(By.CssSelector("button[version='textButtonV1']"));
            travUser.SendKeys(ServerInfo.UserId);
            Thread.Sleep(2000);
            travPass.SendKeys(ServerInfo.UserPassword);
            Thread.Sleep(2000);
            Tools.PrintOutput("Attempt to login");
            travSubmit.Click();
            Thread.Sleep(2000);

            return true;
        }
    }
}
