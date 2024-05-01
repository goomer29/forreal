using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class PostData
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string ChallengeId { get; set; }
        public string FileType { get; set;}
    }
}
