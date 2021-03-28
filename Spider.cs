using System;
using System.Net;
using Parser.Models;
using HtmlAgilityPack;



namespace Parser
{
    public class Spider
    {
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
            string xpathForUrls = "a";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);
            HtmlNodeCollection Tags =  doc.DocumentNode.SelectNodes("//a/@href");
            foreach (var elem in Tags)
            {
                Console.WriteLine(elem.Attributes["href"].Value );
            }
            
        } 
        private void FindGamesOnPage()
        {
            
        }
        
        
    }
}