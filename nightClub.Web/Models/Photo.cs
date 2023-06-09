﻿using System;
using System.ComponentModel.DataAnnotations;

namespace nightClub.Web.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}