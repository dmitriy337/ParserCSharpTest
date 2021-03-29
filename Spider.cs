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

        public int CountPages = 118;

        private UseDB useDB = new UseDB();
        
        
        public void StartCrawl()
        {
            for (int i = 1; i<CountPages;i++)
            {
                FindUrlsOnPage($"https://s1.torrents-igruha.org/newgames/page/{i}/");
            }
            
            
        }

        private void FindUrlsOnPage(string url)
        {
            try
            {
                using (WebClient Client = new WebClient { Encoding = System.Text.Encoding.UTF8 })
                {


                    string data = Client.DownloadString(url);

                    string xpathForUrls = "//a/@href";

                    List<string> NotAllowToParse = new List<string> { ".jpg", ".png", ".jpeg", ".webp", "download.php", "do=register", "#comment", ".gif" };

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(data);
                    HtmlNodeCollection Tags = doc.DocumentNode.SelectNodes(xpathForUrls);

                    foreach (var elem in Tags)
                    {
                        try
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
                                    Task task = new Task(() => ParseGame(ElemUrl));
                                    task.Start();
                                    FindUrlsOnPage(ElemUrl);

                                }
                                else
                                {
                                    FindUrlsOnPage(ElemUrl);


                                }


                            }
                        }
                        catch
                        {

                        }
                        //Console.WriteLine();
                    }
                }
            }
            catch
            {
                
            }
            
        } 
        public void ParseGame(string url)
        {
            try
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



                //Get description
                string DescriptionXpath = "//*[@id=\"dle-content\"]/div[6]/text()[1]";
                HtmlNode DescriptionNode = doc.DocumentNode.SelectSingleNode(DescriptionXpath);
                string Description = DescriptionNode.InnerText;


                //Get screenshots
                string ScreenshotList = "";
                string ScreenshotXpath = "//div[@class=\"item-screenstop\"]/a/img";
                HtmlNodeCollection ScreenshonNode = doc.DocumentNode.SelectNodes(ScreenshotXpath);

                for (int i = 0; i < ScreenshonNode.Count(); i++)
                {
                    string screenshot = ScreenshonNode[i].Attributes["src"].Value;
                    ScreenshotList = ScreenshotList + screenshot + ';';
                }


                useDB.WriteToDB(url: url,
                    image: Image,
                    title: Title,
                    descriptoin: Description,
                    screenshots: ScreenshotList);
                useDB.SaveChanges();
            }
            catch
            {

            }

        }


    }
}