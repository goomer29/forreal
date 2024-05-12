using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class Chat
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime? Time { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
