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
        private ApplicationContext db = new ApplicationContext();

        public void WriteToDB(string url,  string title, string image, string descriptoin, string screenshots )
        {
                    Game game = new Game
                    {
                        Url = url,
                        Title = title,
                        Image = image,
                        Discription = descriptoin,
                        Screenshots = screenshots
                    };
                    db.Games.Add(game);
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}