using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SeleniumBot.Program;

namespace SeleniumBot
{
    public class ResourceManagement
    {
        private IWebDriver driver;

        public ResourceManagement(IWebDriver _webDriver)
        {
            driver = _webDriver;
        }

        public void UpdateResources()
        {
            if (PlayerInfo.dorfVillages != null)
            {
                foreach (dorfVillage dorf in PlayerInfo.dorfVillages)
                {
                    List<dorfInfo> dorfInfos = new List<dorfInfo>();
                    driver.Navigate().GoToUrl(ServerInfo.ServerUrl + "dorf1.php?newdid=" + dorf.VillageId);
                    Thread.Sleep(2000);
                    WebElement resourceElem = (WebElement)driver.FindElement(By.Id("resourceFieldContainer"));
                    IList<IWebElement> karteCollection = resourceElem.FindElements(By.TagName("div"));
                    foreach (IWebElement element in karteCollection)
                    {
                        try
                        {
                            //onclick="window.location.href='/build.php?id=7'"
                            string elem = element.GetAttribute("onclick");
                            if (elem != null)
                            {
                                string elemId = elem.Remove(0, 36);
                                string curLevel = element.Text;
                                dorfInfo info = new dorfInfo();
                                info.Id = elemId;
                                if (!string.IsNullOrEmpty(curLevel))
                                    info.Level = curLevel;
                                else
                                    info.Level = "0";

                                dorfInfos.Add(info);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    Console.WriteLine(resourceElem.Text);
                    string s = resourceElem.Text;
                    string ss = resourceElem.ToString();
                    Console.WriteLine(s);
                    dorf.dorfInfos = dorfInfos;
                }
            }
        }

        private DateTime UpgradeResourcesField(string villageId, string buildId)
        {
            Random rand = new Random();
            string navUrl = ServerInfo.ServerUrl + "dorf1.php?newdid=" + villageId;
            driver.Navigate().GoToUrl(navUrl);
            Thread.Sleep(1500);
            string maxLevel = "level 10";
            string currentLevel = "";
            string currentUpgrade = string.Empty;

            try
            {
                string navUrlDorf = ServerInfo.ServerUrl + "build.php?id=" + buildId;
                driver.Navigate().GoToUrl(navUrlDorf);
                IWebElement currLevel = driver.FindElement(By.ClassName("level"));
                IWebElement currUpgrade = driver.FindElement(By.ClassName("titleInHeader"));
                currentLevel = currLevel.Text;
                currentUpgrade = currUpgrade.Text;

                IWebElement travUpgrade = driver.FindElement(By.CssSelector("button[class='textButtonV1 green build']"));
                IWebElement nextTimer = driver.FindElement(By.CssSelector("div[class='inlineIcon duration'] > span"));
                string nextLevel = travUpgrade.GetAttribute("value");
                string nextTimerStr = nextTimer.Text;

                var time = TimeSpan.Parse(nextTimerStr);

                String[] str = currentLevel.Split(" ");
                int curLevel = 0;
                int.TryParse(str[1], out curLevel);

                Thread.Sleep(1500);

                if (curLevel >= 0)
                {
                    string nxtLvl = "Upgrade to level " + (curLevel + 1).ToString();
                    if (nextLevel == nxtLvl)
                    {
                        travUpgrade.Click();
                        int nextLevelCalc = (curLevel + 1);
                        int baseWait = 1000 + rand.Next(200, 1000);
                        baseWait += Convert.ToInt32(time.TotalSeconds) * 1000;

                        Tools.PrintOutput("Upgrading [" + currentUpgrade + "] to level " + nextLevelCalc.ToString() + " which take " + time.TotalSeconds.ToString() + " seconds");

                        Thread.Sleep(baseWait);
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                int baseWait = 1000 + rand.Next(200, 1000);
                Thread.Sleep(baseWait);
            }
            catch (Exception ex)
            {
                int baseWait = 1000 + rand.Next(200, 1000);
                Console.WriteLine(ex.Message);
            }

            Tools.PrintOutput("Reached max level for -> " + currentUpgrade);
            currentLevel = "";

            return DateTime.Now;
        }

    }
}
