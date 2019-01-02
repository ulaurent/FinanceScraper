using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Data;
using System.Data.SqlClient;

namespace FinanceScraper.Scrape
{
    public class Scraping
    {
            public List<Stock> myList;

            public List<Stock> Scrapington()
            {
                ChromeOptions option = new ChromeOptions();
                option.AddArgument("--headless");
                IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory);

                WebDriverWait waitForElement = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                driver.Navigate().GoToUrl("https://finance.yahoo.com/");

                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(".//a[@id = 'uh-signedin']")));
                driver.FindElement(By.XPath(".//a[@id = 'uh-signedin']")).Click();


                // Console.WriteLine("Email:");
                var userName = "ulaurent12@gmail.com";

                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(".//input[@name = 'username']")));
                driver.FindElement(By.XPath(".//input[@name = 'username']")).SendKeys(userName);
                driver.FindElement(By.XPath(".//input[@id= 'login-signin']")).Click();

                //Console.WriteLine("Password:");
                var passWord = "davidbabo16";

                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(".//input[@id = 'login-passwd']")));
                driver.FindElement(By.XPath(".//input[@id = 'login-passwd']")).SendKeys(passWord);
                driver.FindElement(By.XPath(".//button[@id = 'login-signin']")).Click();

                // Navigate to My portfolio page
                driver.FindElement(By.XPath(".//a[@title = 'My Portfolio']")).Click();

                // Wit for pop up, then click 'x' to exit pop up
                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//dialog[@id = '__dialog']/section/button")));
                driver.FindElement(By.XPath("//dialog[@id = '__dialog']/section/button")).Click();

            // Click on watch list under Portfolio
            //waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[@class='Fz(m) Fw(b) IbBox Td(n) C($c-fuji-blue-1-b) Ell']")));
            //driver.FindElement(By.XPath("//a[@class='Fz(m) Fw(b) IbBox Td(n) C($c-fuji-blue-1-b) Ell']")).Click();
            //var elementExist = driver.FindElement(By.XPath("//*[@id=\"main\"]/section/section/div[2]/table/tbody/tr[1]/td[1]/a"));
            //if (elementExist)
                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"main\"]/section/section/div[2]/table/tbody/tr[1]/td[1]/a")));
                driver.FindElement(By.XPath("//*[@id=\"main\"]/section/section/div[2]/table/tbody/tr[1]/td[1]/a")).Click();


                // Ready to scrape data to console
                waitForElement.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html[1]/body[1]/div[2]/div[3]/section[1]/section[2]/div[2]/table[1]/tbody[1]")));
                var table = driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[3]/section[1]/section[2]/div[2]/table[1]/tbody[1]"));
                var children = table.FindElements(By.XPath(".//*"));


                List<IWebElement> elements = new List<IWebElement>();
                elements = driver.FindElements(By.XPath("//tbody/tr")).ToList<IWebElement>();


                List<Stock> PortStocks = new List<Stock>();

                foreach (var stock in elements)
                {
                    var newStock = Convert.ToString(stock.Text);
                    string[] anotherStock = newStock.Split(' ');

                    PortStocks.Add(new Stock()
                    {
                        StockSymbol = anotherStock[0],
                        LastPrice = anotherStock[1],
                        MarketTime = "0" + anotherStock[5] + ":00",
                        PriceChange = anotherStock[2],
                        PercentChange = anotherStock[3]
                    });
                }

                // Now set up connection to database & Inset data
                SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-CS7KDGHP\BABOSQL;Initial Catalog=Stock1;Integrated Security=True");

                connection.Open();

                foreach (var stock in PortStocks)
                {
                    string data = string.Format("INSERT INTO Practice (SYMBOL, LASTPRICE, CHANGE, DATETIME) VALUES ('{0}','{1}',{2},'{3}')", stock.StockSymbol, stock.LastPrice, stock.PriceChange, stock.MarketTime);
                    SqlCommand cmd = new SqlCommand(data, connection);

                    cmd.ExecuteNonQuery();
                }

                connection.Close();


                return this.myList = PortStocks;
            }

        }
    }
