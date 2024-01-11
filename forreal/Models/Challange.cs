using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class Challange
    {
        public string ChallangeText { get; set; }
        public int Diffcult { get; set; }
        public ObservableCollection<User> Users { get; set; }

    }
}
