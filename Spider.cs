using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Collections;
using System.Linq;
using Parser.Models;
using HtmlAgilityPack;
using System.Threading.Tasks;



namespace Parser
{
    public class Spider
    {
        private HashSet<string> PassedUrls = new HashSet<string>();
        
        private string domain = "s1.torrents-igruha.org";
        
        private string startUrl = "https://s1.torrents-igruha.org/top10-online.html";

       
        
        
        public void StartCrawl()
        {
            
            FindUrlsOnPage(startUrl);
        }

        private void FindUrlsOnPage(string url)
        {
            try
            {
                WebClient Client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
                    //Uri uri = new Uri(url);
                string data = Client.DownloadString(url);

                string xpathForUrls = "//a/@href";

                List<string> NotAllowToParse = new List<string> { ".jpg", ".png", ".jpeg", ".webp", "download.php", "do=register", "#comment", ".gif" };

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(data);
                HtmlNodeCollection Tags = doc.DocumentNode.SelectNodes(xpathForUrls);

                foreach (var elem in Tags) 
                {
                    string ElemUrl = elem.Attributes["href"].Value;


                    if (PassedUrls.Contains(ElemUrl))
                    {
                        continue;
                    }
                    else
                    {
                        PassedUrls.Add(ElemUrl);

                        // CheckDomain
                        if (ElemUrl.Contains(domain) != true) { continue; }
                        // CheckFormat 
                        if (NotAllowToParse.Any(u => ElemUrl.Contains(u))) { continue; }

                        if (ElemUrl.Contains(".html"))
                        {
                            ParseGame(ElemUrl);
                            Task task = new Task(() => FindUrlsOnPage(ElemUrl));
                            //FindUrlsOnPage(ElemUrl);
                            task.Start();

                        }
                        else
                        {
                            Task task = new Task(() => FindUrlsOnPage(ElemUrl));
                            task.Start();
                        }

                    }
                    //Console.WriteLine();
                }
            }
            catch
            {
                
            }
            
        } 
        public void ParseGame(string url)
        {
            Encoding encoding = Encoding.GetEncoding(1251);

            WebClient client = new WebClient { Encoding = encoding };
            string data;
            HtmlDocument doc;
            
            data = client.DownloadString(url);
            doc = new HtmlDocument();
            doc.LoadHtml(data);
            //doc.Load(client.OpenRead(url), encoding: encoding);
            

            




            //Get title 
            string TitleXpath = "//div[@class=\"module-title\"]/h1";
            HtmlNode TitleNode = doc.DocumentNode.SelectSingleNode(TitleXpath);
            string Title = TitleNode.InnerText;
            Console.WriteLine(Title);


            //Get image
            string ImageXpath = "/html/body/div[5]/div[1]/div/div/div[2]/div[1]/img";
            HtmlNode ImageNode = doc.DocumentNode.SelectSingleNode(ImageXpath);
            string Image = ImageNode.Attributes["src"].Value;
            Console.WriteLine(Image);


            //Get description
            string DescriptionXpath = "//*[@id=\"dle-content\"]/div[6]/text()[1]";
            HtmlNodeCollection DescriptionNode = doc.DocumentNode.SelectNodes(DescriptionXpath);
            foreach (var descNode in DescriptionNode)
            {
                string Description = descNode.InnerText;
                Console.WriteLine(Description);

            }

            //Get screenshots
            string[] ScreenshotList;
            string ScreenshotXpath = "//div[@class=\"item-screenstop\"]/a/img";
            HtmlNodeCollection ScreenshonNode = doc.DocumentNode.SelectNodes(ScreenshotXpath);
            ScreenshotList = new string[ScreenshonNode.Count];
            for (int i = 0; i<ScreenshonNode.Count();i++)
            {
                string screenshot = ScreenshonNode[i].Attributes["src"].Value;
                ScreenshotList[i] = screenshot;
            }
            Console.WriteLine(ScreenshotList);
        }


    }
}