using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nightClub.Web.Models
{
    public class Review
    {
        [Required]
        public string Name { get; set; } //defacto o sa citeasca numele de utilizator.
        [Required]
        public string Email { get; set; }// datele utilizatorului logat 
        [Required]
        [StringLength(30, MinimumLength = 8)]
        public string Subject { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 30)]
        public string Message { get; set; }

    }
}