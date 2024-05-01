using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class Post
    {
        public string username { get; set; }
        public string challengename { get; set; }

        //Tal
        public DateTime TaskDate { get; set; }
        public string date { get; set; }
        public string image { get; set; }
        public string video { get; set; }
        public bool is_image { get; set; }
        public bool is_video { get; set;}
    }
}
