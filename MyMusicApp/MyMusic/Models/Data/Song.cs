using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Models.Data
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public String ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Genre { get; set; }
        public string CoverUrl { get; set; }
        public string Description { get; set; }
        public bool ForSale { get; set; }
        public string ImageFileName { get; set; }
        public string AudioFileName { get; set; }
        public bool ActiveSong { get; set; }
    }
}
