using System;
using System.Net;
using HtmlAgilityPack;
using System.Text;
namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Spider spider = new Spider();
            //spider.StartCrawl();
            spider.ParseGame("https://s1.torrents-igruha.org/24-16-gta-5-onlayn-versiya-na-kompyuter-2-1.html");
            
            Console.ReadKey();
        }
    }
}