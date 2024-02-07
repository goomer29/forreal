using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.Models
{
    public class ChallangesDto
    {
        public ObservableCollection<Challange> ChallangesList { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
