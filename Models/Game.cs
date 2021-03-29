using System;
using System.ComponentModel.DataAnnotations;

namespace Parser.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Url { get; set; } // 
        public string Title { get; set; } //
        public string Image { get; set; } //
        public string Discription { get; set; } //
        public string Screenshots { get; set; } //
        
    }
}