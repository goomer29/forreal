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
        public Event Evt {  get; set; } 
        private Event GetEvent()
        {
            return new Event { EventTitle = "Challange", BgColor = "#EB9999", Date = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 24, 0, 0).Ticks) };
            
        }

        [Obsolete]
        public ChallangePageViewModel()
        {
            Evt = GetEvent();
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Time_Reamins = ((App)(Application.Current)).Timeleft;
                    Evt.Timespan = Evt.Date - DateTime.Now;
                    ((App)(Application.Current)).Timeleft = Evt.Timespan;
                    if (((App)(Application.Current)).StartTimer)
                    {
                        ((App)(Application.Current)).StartTimer = false;
                    }
                });      
                return true;
            });
            OnPropertyChange(nameof(Time_Reamins));
            OnPropertyChange(nameof(Evt));
        }
    }
    public class Event
    {
        public DateTime Date { get; set; }
        public string EventTitle { get; set; }
        public TimeSpan Timespan { get; set; }
        public string Days => Timespan.Days.ToString("00");
        public string Hours { get => Timespan.Hours.ToString(); }
        public string Minutes { get => Timespan.Minutes.ToString(); }
        public string Seconds { get => Timespan.Seconds.ToString(); }
        public string BgColor { get; set; }
    }
}
