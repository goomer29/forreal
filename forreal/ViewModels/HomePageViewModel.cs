using forreal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;
using forreal.Services;
using forreal.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace forreal.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        readonly IPopupService popupService;
        private string _userName;//שם משתמש
        public bool _showfriend;
        public static bool _showsubmit;
        public ICommand ChallangeCommand { get; protected set; }
        public ICommand CloseChallange { get; protected set; }
        public ICommand PostCommand { get; protected set; }
        public ICommand YesCommand { get; protected set; }
        public ICommand NoCommand { get; protected set; }
        public static ObservableCollection<Challange> statChallanges { get; set; }
        public ObservableCollection<Challange> Challanges { get; set;  }
        public Challange ChallengeSubmit { get => ChallangePageViewModel.challange_select; }
        public ImageSource ImageSubmit { get => ChallangePageViewModel.image_select; }
        public FileResult FileSubmit { get=>ChallangePageViewModel.file_select; }
        public ObservableCollection<string> UsersNameWant { get => MainPageViewModel.WantedUsers; }
        public ObservableCollection<string> UsersNameRequest { get => MainPageViewModel.RequestUsers; }
        public ObservableCollection<User> Users { get => MainPageViewModel.AllUsers; }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public string UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; OnPropertyChange(); } }
        }
        public bool ShowSubmit
        {
            get => _showsubmit;
            set { if (_showsubmit != value) { _showsubmit = value; OnPropertyChange(); } }
        }
        public bool ShowFriend
        {
            get => _showfriend;
            set { if (_showfriend != value) { _showfriend = value; OnPropertyChange(); } }
        }
        public HomePageViewModel(IPopupService _popupService, ForrealService service)
        {
            ShowFriend = false;
            ShowSubmit = false;
            Challanges = new ObservableCollection<Challange>();
            _service = service;
            popupService = _popupService;
            UserName = "Welcome to Homepage " + ((App)Application.Current).User.UserName;

           ShowFriend= Users.Any(u => !UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName));
            //foreach (User u in Users)
            //{
            //    if (!UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName))
            //    {
            //        ShowFriend = true;
            //        break;
            //    }              

            //}
            Task.Delay(5000);
            if(ShowFriend)
            {
                AppShell.Current.DisplayAlert("You've got a friend request!", "go to Search to see more info", "cancel");
            }
            ChallangeCommand = new Command(async () =>
            {                
                
                try
                {
                    if(Challanges.Count == 0)
                    {
                        var ch = await _service.GetChallange();
                        if (ch.Success)
                        {
                            for (int i = 1; i < 6; i++)
                            {
                                Challanges.Add(ch.ChallangesList[i]);
                            }
                        }
                    }                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await AppShell.Current.Navigation.PopModalAsync();
                }
                OnPropertyChange();
                statChallanges = Challanges;
                await popupService.ShowPopupAsync<ChallangePageViewModel>(onPresenting: vm => vm.SelectedImage += (s, e) => {
                    OnPropertyChange(nameof(ShowSubmit));
                    OnPropertyChange(nameof(ChallengeSubmit));
                    OnPropertyChange(nameof(ImageSubmit));
                    OnPropertyChange(nameof(FileSubmit));
                });

                OnPropertyChange(nameof(ShowSubmit));
                OnPropertyChange(nameof(ChallengeSubmit));
                OnPropertyChange(nameof(ImageSubmit));
                OnPropertyChange(nameof(FileSubmit));

            });

            CloseChallange = new Command(async () =>
            {
                await ChallangePage.ClosePopup();
                OnPropertyChange(nameof(ShowSubmit));
                OnPropertyChange(nameof(ChallengeSubmit));
                OnPropertyChange(nameof(ImageSubmit));
                OnPropertyChange(nameof(FileSubmit));
            });
            NoCommand = new Command(async () => 
            {
                ShowSubmit = false;
            });
            YesCommand = new Command(async () =>
            {
                try
                {
                    string username = ((App)Application.Current).User.UserName;
                    string challangename = ChallengeSubmit.Text;
                    PostDto post = new PostDto();
                    post.username = username; post.challengename = challangename;
                    var file = FileSubmit;
                    var response = await _service.UploadPost(post, file);
                    if (response == System.Net.HttpStatusCode.OK)
                    {
                        await AppShell.Current.DisplayAlert("All done!", "the post has submitted", "cancel");
                    }
                    ShowSubmit = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await AppShell.Current.Navigation.PopModalAsync();
                }
            });
        }     
    }
}
