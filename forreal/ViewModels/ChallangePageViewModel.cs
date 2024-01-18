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
        private TimeSpan time_remains;//זמן שנשאר
        public TimeSpan Time_Reamins
        {
            get => time_remains;
            set { if (time_remains != value) { time_remains = value; OnPropertyChange(); } }
        }
        private Event GetEvent()
        {
            return new Event { EventTitle = "Challange", BgColor = "#EB9999", Date = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 24, 0, 0).Ticks) };
            
        }

        [Obsolete]
        public ChallangePageViewModel()
        {
            var evt = GetEvent();
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                    Time_Reamins = evt.Date - DateTime.Now;
                evt.Timespan = Time_Reamins;
                return true;
            });

        }
    }
    public class Event
    {
        public DateTime Date { get; set; }
        public string EventTitle { get; set; }
        public TimeSpan Timespan { get; set; }
        public string Days => Timespan.Days.ToString("00");
        public string Hours => Timespan.Hours.ToString("00");
        public string Minutes => Timespan.Minutes.ToString("00");
        public string Seconds => Timespan.Seconds.ToString("00");
        public string BgColor { get; set; }
    }
}
