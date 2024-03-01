using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OfficeOpenXml;

namespace Program
{
    class Program
    {
        private static IWebDriver driver;

        static void Main(string[] args)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.tradingpost.com.au/search-results/?q=&filter-location-text=&filter-location-dist=25&cat=appliances%20and%20electrical");

            var collections = FindElements(By.XPath("//div[@class='result-sec']"));

            SaveToExcel(collections, "output.xlsx");

            //driver.Quit();
        }

        static List<(string Title, string Description, string Price, string URL)> FindElements(By by)
        {
            List<(string Title, string Description, string Price, string URL)> elementList = new List<(string Title, string Description, string Price, string URL)>();

            while (true)
            {
                var elements = driver.FindElements(by);

                if (elements != null && elements.Count > 0)
                {
                    foreach (var element in elements)
                    {
                        string title = element.FindElement(By.XPath(".//h3")).Text.Trim();
                        string description = element.FindElement(By.XPath(".//p")).Text.Trim();
                        string price = element.FindElement(By.XPath(".//h2")).Text.Trim();
                        string url = element.FindElement(By.XPath(".//a")).GetAttribute("href");

                        elementList.Add((title, description, price, url));
                    }
                    return elementList;
                }

                Thread.Sleep(100);
            }
        }

        static void SaveToExcel(List<(string Title, string Description, string Price, string URL)> elements, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                worksheet.Cells.LoadFromCollection(elements, true);

                FileInfo fileInfo = new FileInfo(filePath);
                package.SaveAs(fileInfo);
            }

            Console.WriteLine("Data has been saved to " + filePath);
        }
    }
}
