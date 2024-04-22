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
using System.Xml.Linq;

namespace forreal.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        readonly IPopupService popupService;
        private string _userName;//שם משתמש
        public bool _showfriend;
        public static bool _showsubmit;
        public ObservableCollection<Post> _posts { get; set; }
        public ObservableCollection<User> friend_users { get; set; }
        public ObservableCollection<UserNameDto> users_with_id {get;set;}
        public ICommand ChallangeCommand { get; protected set; }
        public ICommand CloseChallange { get; protected set; }
        public ICommand PostCommand { get; protected set; }
        public ICommand YesCommand { get; protected set; }
        public ICommand NoCommand { get; protected set; }
        public static ObservableCollection<Challange> statChallanges { get; set; }
        public ObservableCollection<Challange> Challanges { get; set;  }
        public ObservableCollection<string> ImagesName { get => MainPageViewModel.Images; }
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
        public ObservableCollection<Post> Posts
        {
            get => _posts; set
            {
                if (_posts != value)
                {
                    _posts = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> FriendUsers
        {
            get => friend_users; set
            {
                if (friend_users != value)
                {
                    friend_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<UserNameDto> UsersWithID
        {
            get => users_with_id; set
            {
                if (users_with_id != value)
                {
                    users_with_id = value; OnPropertyChange();
                }
            }
        }
        public HomePageViewModel(IPopupService _popupService, ForrealService service)
        {
            UsersWithID = new ObservableCollection<UserNameDto>();
            Posts= new ObservableCollection<Post>();
            FriendUsers= new ObservableCollection<User>();
            ShowFriend = false;
            ShowSubmit = false;
            Challanges = new ObservableCollection<Challange>();
            _service = service;
            popupService = _popupService;
            UserName = "Welcome to Homepage " + ((App)Application.Current).User.UserName;

            ShowFriend = Users.Any(u => !UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName));
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
                        //var response1 = await _service.GetChallangeID(challangename); if i want updated posts in ProfilePage
                        //if (response1.Success)
                        //{
                        //    DateTime time = DateTime.Now;
                        //    string day = time.Day.ToString(); string month = time.Month.ToString(); string year = time.Year.ToString();
                        //    string date = day + "_" + month + "_" + year;
                        //    var imagename = $"{MainPageViewModel.UserID}_{response1.Id}_{date}{Path.GetExtension(file.FileName)}";
                        //    MainPageViewModel.Images.Add(imagename);
                        //    OnPropertyChange(nameof(ProfilePageViewModel.Posts));
                        //}
                        await AppShell.Current.DisplayAlert("All done!", "the post has submitted", "cancel");
                    }
                    ShowSubmit = false;
                    //only see friends posts in the current day
                    foreach (var u in Users)
                    {
                        if (UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName))
                            FriendUsers.Add(u);
                    }
                    UsersWithID = await _service.GetUserNameWithID();
                    var time= DateTime.Now;
                    string day = time.Day.ToString(); var month= time.Month.ToString(); var year= time.Year.ToString();
                    foreach (var name in ImagesName)
                    {
                       var infoes = name.Split('_');
                       string[] infofoes = null;
                       if (infoes.Length > 3)
                        {
                            infofoes = infoes[4].Split(".");
                            string text = null;
                            int id = Int32.Parse(infoes[0]);
                            foreach (var user in UsersWithID)
                            {
                                bool IsFriend = FriendUsers.Any(friend => friend.UserName == user.Text);
                                if (IsFriend && id == user.Id && infoes[2] == day && infoes[3] == month && infofoes[0] == year)
                                {
                                    var ch_id = Int32.Parse(infoes[1]);
                                    text = await _service.GetChallangeName(ch_id);
                                    var posty = new Post { username = user.Text, challengename = text, date = infoes[2] + "/" + infoes[3] + "/" + infofoes[0], image = $"{ForrealService.WwwRoot}/Images/{name}" };
                                    Posts.Add(posty);
                                }

                            }
                        }
                    }                    
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
