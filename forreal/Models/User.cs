using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace forreal.Models
{
    public class User
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserPswd { get; set; }
        public List<Challange> Challanges { get; set; }
    }
}
