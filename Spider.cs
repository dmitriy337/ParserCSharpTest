using System;
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
        
        private string startUrl = "https://s1.torrents-igruha.org/newgames/page/";

        private WebClient Client = new WebClient {Encoding = System.Text.Encoding.UTF8 };
        
        
        public void StartCrawl()
        {
            
            FindUrlsOnPage(startUrl);
        }

        private void FindUrlsOnPage(string url)
        {
            try
            {
                string data = Client.DownloadString(url);

                string xpathForUrls = "//a/@href";

                List<string> NotAllowToParse = new List<string> { ".jpg", ".png", ".jpeg", ".webp", "download.php" };

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
                        // CheckComment
                        if (ElemUrl.Contains("#comment")) { continue; }
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
                Console.WriteLine("Ошииибка!!!!!!!!!!!!!!!!!");
            }
            
        } 
        private void ParseGame(string url)
        {
            Console.WriteLine(url);
            string TitleXpath = "//*[@id=\"dle - content\"]/div[1]/h1";



            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(data);
            //HtmlNodeCollection Tags = doc.DocumentNode.SelectNodes(GameXpath);
            //foreach (var element in Tags)
            //{
            //    Console.WriteLine(element.);
            //}
        }
        
        
    }
}