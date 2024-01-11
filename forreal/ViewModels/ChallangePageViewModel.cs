using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class ChallangePageViewModel : ViewModel
    {
        private string time_remains;//זמן שנשאר
        public string Time_Reamins
        {
            get => time_remains;
            set { if (time_remains != value) { time_remains = value; OnPropertyChange(); } }
        }
        public ChallangePageViewModel()
        {
            var TimeNow= DateTime.Now;
            var TimeDay = new DateTime();
            
        }
    }
}
