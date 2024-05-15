
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
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
        private Color bgcolor;
        private string title;
        private bool _showchallangeerror;
        public static Challange challange_select;
        public static ImageSource image_select;
        public static MediaSource video_select;
        public static FileResult file_select;
        public ObservableCollection<Challange> Challanges{ get =>HomePageViewModel.statChallanges; }
        public event EventHandler SelectedImage;
        #endregion
        #region Proporties
        public TimeSpan Time_Reamins
        {
            get => time_remains;
            set { if (time_remains != value) { time_remains = value; OnPropertyChange(); } }
        }
        private void OnSelectedImage()
        {
            SelectedImage?.Invoke(this, EventArgs.Empty);
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
        public Color BgColor
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
        public bool ShowChallangeError
        {
            get => _showchallangeerror; set
            {
                if (_showchallangeerror != value)
                {
                    _showchallangeerror = value; OnPropertyChange();
                }
            }
        }
        public Challange ChallangeSelect
        {
            get => challange_select; set
            {
                if (challange_select != value)
                {
                    challange_select = value; if (!ValidateChallange())
                    {
                        ShowChallangeError = true;
                    }
                    else
                        ShowChallangeError = false;
                    OnPropertyChange();
                    OnPropertyChange(nameof(IsButtonEnabled));
                }           
                
            }
        }
        public bool IsButtonEnabled { get { return ValidateChallange(); } }
        public Event GetEvent()
        {
            return new Event { EventTitle = "Time Remains for TOday's challenges", BgColor = Color.FromRgb(38, 127, 0) , Date = new DateTime(DateTime.Today.Ticks+ new TimeSpan(0, 24, 0, 0).Ticks) };
            
        }
        #endregion
        #region Commands
        public ICommand ChallangesCommand { get; protected set; }
        public ICommand PickFileCommand { get; protected set; }
        public ICommand PickVideoCommand { get; protected set; }
        #endregion
        [Obsolete]
        public ChallangePageViewModel()
        {
            ShowChallangeError = true;
            ChallangeSelect = null;
            Evt = GetEvent();
            BgColor = Color.FromRgb(38, 127, 0);
            Title = "Time Remains for Today's challenges";
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Time_Reamins = ((App)(Application.Current)).Timeleft;
                    Evt.Timespan =Evt.Date-DateTime.Now;
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
                challange_select = ChallangeSelect;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        FileResult result = await FilePicker.Default.PickAsync(new PickOptions
                        {
                             FileTypes = FilePickerFileType.Images,
                            PickerTitle = "please select an image"
                        });
                   
                try
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) || result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) || result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        HomePageViewModel._showsubmit = true;
                        HomePageViewModel._showvideosubmit = false;
                        var stream = await result.OpenReadAsync();
                        var image = ImageSource.FromStream(() => stream);
                            image_select = image;
                            file_select = result;
                              OnSelectedImage();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Alert", "Invalid file selected", "Ok");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                    });
            });
            PickVideoCommand = new Command(async () =>
            {
                challange_select = ChallangeSelect;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    FileResult result = await FilePicker.Default.PickAsync(new PickOptions
                    {
                        FileTypes = FilePickerFileType.Videos,
                        PickerTitle = "please select a video"
                    });

                    try
                    {
                          HomePageViewModel._showvideosubmit = true;
                           HomePageViewModel._showsubmit = false;
                            var stream = await result.OpenReadAsync();
                        var video = MediaSource.FromFile(result.FullPath);
                        video_select = video;
                            file_select = result;
                            OnSelectedImage();
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                });
            });
           
        }
        private bool ValidateChallange()
        {
            if(ChallangeSelect==null)
                return false;
            return !(string.IsNullOrEmpty(ChallangeSelect.Text));
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
        public Color BgColor { get; set; }
    }
}
