using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class ChatDto
    {
        public string username { get; set; }
        public string text { get; set; }
        public DateTime? time { get; set; }
    }
}
