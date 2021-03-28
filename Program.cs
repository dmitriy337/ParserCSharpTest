using System;
using System.Net;
using HtmlAgilityPack;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestCommentFromDmtr
            //HEllo
            Spider spider = new Spider();
            spider.StartCrawl();
            
            Console.ReadKey();
        }
    }
}