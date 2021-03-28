using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Parser.Models
{
    public class UseDB
    {
        public class ApplicationContext : DbContext
        {
            public DbSet<Models.Game> Games { get; set; }

            public ApplicationContext()
            {
                Database.EnsureCreated();
                
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite($"Data Source={System.IO.Directory.GetCurrentDirectory()}\\OurDB.db");
            }
        }

        public void WriteToDB(string url,  string title, string image, string discriptoin, string[] screenshots, string urlToDownload )
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    Game game = new Game
                    {
                        Url = url,
                        Title = title,
                        Image = image,
                        Discription = discriptoin,
                        Screenshots = screenshots,
                        UrlToDownload = urlToDownload
                    };

                    db.Games.Add(game) ;
                }
            }
            catch
            {
                Console.WriteLine("Smth wrong!");
            }
        }
    }
}