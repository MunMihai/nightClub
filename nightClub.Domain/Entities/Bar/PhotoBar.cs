using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nightClub.Domain.Entities.Bar
{
    public class PhotoBar
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int Alcohol { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
