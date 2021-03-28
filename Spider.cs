using System;
using System.Collections.Generic;
using System.Net;
using Parser.Models;
using HtmlAgilityPack;



namespace Parser
{
    public class Spider
    {
        private HashSet<string> PassedUrls = new HashSet<string>();
        
        private string domain = "s1.torrents-igruha.org";
        
        private string startUrl = "https://s1.torrents-igruha.org/newgames/page/";
        
        private WebClient Client = new WebClient();

        public void StartCrawl()
        {
            string data  = Client.DownloadString(startUrl);
            FindUrlsOnPage(data);
        }

        private void FindUrlsOnPage(string data)
        {
            string xpathForUrls = "//a/@href";

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);
            HtmlNodeCollection Tags =  doc.DocumentNode.SelectNodes(xpathForUrls);
            foreach (var elem in Tags)
            {
                string ElemUrl = elem.Attributes["href"].Value;
                
                //Chech for unique urls
                if (PassedUrls.Contains(ElemUrl))
                {
                    continue;
                }
                else
                {
                    PassedUrls.Add(ElemUrl);
                    Console.WriteLine(ElemUrl);
                }
                //Console.WriteLine();
            }
            
        } 
        private void FindGamesOnPage()
        {
            
        }
        
        
    }
}