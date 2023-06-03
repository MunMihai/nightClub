using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nightClub.Web.Models
{
    public class Bar
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Alcohol { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}