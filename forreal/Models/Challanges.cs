using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class Challanges
    {
        public ObservableCollection<Challange> ChallangesList { get; set; }
        public List<string> ImagesDifficult { get; set; }

    }
}
