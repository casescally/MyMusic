using MyMusic.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Models.Data
{
    public class SongImage
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
        public string ImagePath { get; set; }
        public bool Active { get; set; }
    }
}
