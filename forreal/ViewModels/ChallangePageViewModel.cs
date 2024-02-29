using CommunityToolkit.Maui.Core;
using forreal.Models;
using forreal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class ChallangePageViewModel : ViewModel
    {
        #region Fields
        private TimeSpan time_remains;//זמן שנשאר
        private string hours;
        private string minutes;
        private string seconds;
        private string bgcolor;
        private string title;
        public ObservableCollection<Challange> Challanges{ get =>HomePageViewModel.statChallanges; }
        
        #endregion
        #region Proporties
        public TimeSpan Time_Reamins
        {
            get => time_remains;
            set { if (time_remains != value) { time_remains = value; OnPropertyChange(); } }
        }
        public Event Evt {  get; set; } 
        public string Hours
        {
            get => hours; set
            {
                if (hours != value)
                {
                    hours = value; OnPropertyChange();
                }
            }
        }
        public string Minutes
        {
            get => minutes; set
            {
                if (minutes != value)
                {
                    minutes = value; OnPropertyChange();
                }
            }
        }
        public string Seconds
        {
            get => seconds; set
            {
                if (seconds != value)
                {
                    seconds = value; OnPropertyChange();
                }
            }
        }
        public string BgColor
        {
            get => bgcolor; set
            {
                if (bgcolor != value)
                {
                    bgcolor = value; OnPropertyChange();
                }
            }
        }
        public string Title
        {
            get => title; set
            {
                if (title != value)
                {
                    title = value; OnPropertyChange();
                }
            }
        }
        public Event GetEvent()
        {
            return new Event { EventTitle = "Time Remains for TOday's challenges", BgColor = "#EB9999", Date = new DateTime(DateTime.Now.Ticks + new TimeSpan(0, 24, 0, 0).Ticks) };
            
        }
        #endregion
        #region Commands
        public ICommand ChallangesCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        #endregion
        [Obsolete]
        public ChallangePageViewModel()
        {
            Evt = GetEvent();
            BgColor = "#EB9999";
            Title = "Time Remains for Today's challenges";
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
                    Hours = Time_Reamins.Hours.ToString();
                    Minutes = Time_Reamins.Minutes.ToString();
                    Seconds = Time_Reamins.Seconds.ToString();
                });  
                return true;
            });

            PickFileCommand = new Command(async () =>
            {
                FileResult result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle="please select an image/video"
                });
                if (result.FileName.EndsWith("jpg",StringComparison.OrdinalIgnoreCase)|| result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)||result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    var stream = await result.OpenReadAsync();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Alert", "Invalid file selected", "Ok");
                }
            });
            
            
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
