using Gehtsoft.PDFFlow.Builder;
using Logic.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Webscraper;




namespace Logic
{
    public class MainScraperFlow
    {
        Scraper scraper;
        string address;
        public async Task GetDefaultDrawings(string adress, int zoomLvl)
        {
            List<string> filePaths = new();
            //Maybe top bottom parameters to put large borders into view. Or allow user is perfect the view, then press save in program. But then, perhaps run browser in program

            string startUrl = "https://arealinformation.miljoeportal.dk/";
            scraper = new();
            address = adress;

            try
            {
                scraper.Driver.Navigate().GoToUrl(startUrl);
                bool present = false;
                while (!present)
                {
                    present = IsElementPresent(By.Id("gcx_search"));
                }
                Thread.Sleep(2000);
                IWebElement searchField = scraper.Driver.FindElement(By.Id("gcx_search"));
                searchField.Clear();
                searchField.SendKeys(address);
                IWebElement searchBtn = scraper.Driver.FindElement(By.ClassName("search-button"));
                searchBtn.Click();
                Thread.Sleep(2000);
                IWebElement clickableResult = scraper.Driver.FindElement(By.ClassName("feature-label"));
                clickableResult.Click();
                Thread.Sleep(2000);
                IWebElement JegVilGerneBtn = scraper.Driver.FindElement(By.XPath("//button[@class='wtm-button bound-visible']"));
                JegVilGerneBtn.Click();
                IWebElement optionKortlag = scraper.Driver.FindElement(By.XPath("//strong[text()='Administrer hvilke kortlag der er synlige']"));
                optionKortlag.Click();
                Thread.Sleep(2000);
                IWebElement ejerlavsGrænser = scraper.Driver.FindElement(By.CssSelector("input[aria-label='Ejerlavsgrænser']"));
                ejerlavsGrænser.Click();
                IWebElement jordStykker = scraper.Driver.FindElement(By.CssSelector("input[aria-label='Jordstykker (Matrikel)']"));
                jordStykker.Click();
                Thread.Sleep(1000);
                IWebElement zoomIn = scraper.Driver.FindElement(By.CssSelector("button[title='Zoomer et niveau ind.']"));
                for (int i = 0; i < zoomLvl; i++)
                {
                    zoomIn.Click();
                    Thread.Sleep(2000);
                }
                IWebElement tools = scraper.Driver.FindElement(By.CssSelector("button[title='Åben værktøjslinjen']"));
                tools.Click();
                Thread.Sleep(1000);
                IWebElement otherTools = scraper.Driver.FindElement(By.XPath("//button[text()='Andre værktøjer']"));
                otherTools.Click();
                Thread.Sleep(1000);
                //Save default picture (zoom lvl 2)
                filePaths.Add(await SavePicture(1));

                //Toggle airial foto
                IWebElement luftFotoNyeste = scraper.Driver.FindElement(By.CssSelector("input[aria-label='Ortofoto forår (nyeste)']"));
                luftFotoNyeste.Click();
                Thread.Sleep(1000);

                //Save arial foto normal zoom lvl
                filePaths.Add(await SavePicture(2));

                //zoom one more lvl
                zoomIn.Click();
                Thread.Sleep(1000);

                //Save arial photo zoomed
                filePaths.Add(await SavePicture(3));

                //Disable airial view
                luftFotoNyeste.Click();
                Thread.Sleep(1000);

                //Save zoomed normal picture.
                filePaths.Add(await SavePicture(4));


                SaveAsPdf(filePaths, address);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public async Task<string> SavePicture(int nrInSequennce)
        {
            IWebElement saveAsPic = scraper.Driver.FindElement(By.CssSelector("button[title='Gem som billede']"));
            saveAsPic.Click();
            Thread.Sleep(1000);
            IWebElement save = scraper.Driver.FindElement(By.XPath("//button[text()='Opret billede']"));
            save.Click();
            Thread.Sleep(5000);

            //Wait until Se Billede Appears
            bool pictureFinished = false;
            while (!pictureFinished)
            {
                pictureFinished = IsElementPresent(By.XPath("//button[text()='Se billede']"));
            }
            Thread.Sleep(1000);
            IWebElement seePic = scraper.Driver.FindElement(By.XPath("//button[text()='Se billede']"));
            seePic.Click();

            //Wait until picture is available
            bool pictureAvailable = false;
            while (!pictureAvailable)
            {
                pictureAvailable = IsElementPresent(By.TagName("img"));
            }

            scraper.Driver.SwitchTo().Window(scraper.Driver.WindowHandles[1]);
            Thread.Sleep(1000);
            string pngUrl = scraper.Driver.FindElement(By.TagName("img")).GetAttribute("src");
            Console.WriteLine(pngUrl);

            PictureDownloader downloader = new();
            string pathToFile = await downloader.InitiateDownload(pngUrl, address, nrInSequennce.ToString());
            scraper.Driver.Close();
            scraper.Driver.SwitchTo().Window(scraper.Driver.WindowHandles[0]);
            Thread.Sleep(1000);
            IWebElement closeBtn = scraper.Driver.FindElement(By.CssSelector("button[title='Luk Eksporter et kortbillede']"));
            closeBtn.Click();
            Thread.Sleep(1000);

            return pathToFile;
        }

        public bool IsElementPresent(By locatorKey)
        {
            try
            {
                IWebElement searchField = scraper.Driver.FindElement(By.Id("gcx_search"));
                Thread.Sleep(1000);
                return true;

            }
            catch (Exception e)
            {
                Thread.Sleep(2000);
                return false;
            }
        }


        public void SaveAsPdf(List<string> imgPaths, string pdfName)
        {
            //PictureDownloader downloader = new();
            //await downloader.InitiateDownload("https://arealinformation.miljoeportal.dk/Geocortex/Essentials/REST/TempFiles/Export.png?guid=563797f7-a4e0-42a7-9bd8-8a3a352bbcff&contentType=image%2Fpng"
            //    , "poopoo Street 51");


            //Docs - https://docs.gehtsoftusa.com/pdfflow/web-content.html#CreatingDocument.html
            var section = DocumentBuilder.New().AddSection();

            for (int i = 0; i < imgPaths.Count; i++)
            {
                section.AddImage(imgPaths[i]).ToSection().InsertPageBreak();
               
            }
            section.ToDocument().Build($@"C:\TegningHenter\{pdfName}.pdf");


            //Delete pictures
            for (int i = 0; i < imgPaths.Count; i++)
            {
                File.Delete(imgPaths[i]);
            }

        }
    }
}
