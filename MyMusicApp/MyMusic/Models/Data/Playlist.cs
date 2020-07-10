using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyMusic.Models.Data
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //admin user
        public String ApplicationUserId { get; set; }
        public string SongIds { get; set; }

    }
}
