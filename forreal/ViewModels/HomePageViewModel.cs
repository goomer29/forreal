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
        private string _userName;//שם משתמש
        public bool _showfriend;
        public static bool _showsubmit;
        public static bool _showvideosubmit;
        public bool _showchallanges;
        public string _showChallangeText;
        public bool _ishomepagelogin;
        public Post _postselect;
        public static Post PostSelecting;
        public ObservableCollection<Post> _posts { get; set; }
        public ObservableCollection<User> friend_users { get; set; }
        public ObservableCollection<UserNameDto> users_with_id { get; set; }
        public ICommand ChallangeCommand { get; protected set; }
        public ICommand CloseChallange { get; protected set; }
        public ICommand PostCommand { get; protected set; }
        public ICommand YesCommand { get; protected set; }
        public ICommand NoCommand { get; protected set; }
        public ICommand ChatCommand { get; protected set; }
        public ICommand CloseChat { get; protected set; }

        // Command to handle the page appearing
        public ICommand PageAppearingCommand { get; private set; }
        public static ObservableCollection<Challange> statChallanges { get; set; }
        public static ObservableCollection<ChatDto> post_chats { get; set; }
        public ObservableCollection<Challange> Challanges { get; set; }
        public ObservableCollection<string> ImagesName { get => MainPageViewModel.Images; }
        public Challange ChallengeSubmit { get => ChallangePageViewModel.challange_select; }
        public ImageSource ImageSubmit { get => ChallangePageViewModel.image_select; }
        public MediaSource VideoSubmit { get => ChallangePageViewModel.video_select; }
        public FileResult FileSubmit { get => ChallangePageViewModel.file_select; }
        public ObservableCollection<string> UsersNameWant { get => MainPageViewModel.WantedUsers; }
        public ObservableCollection<string> UsersNameRequest { get => MainPageViewModel.RequestUsers; }
        public ObservableCollection<ChallangeNameDto> ChallangesNames { get => MainPageViewModel.ChallangeNames; }
        public ObservableCollection<UserNameDto> UsersNames { get => MainPageViewModel.UserWithID; }
        public ObservableCollection<User> Users { get => MainPageViewModel.AllUsers; }
        
        #region Service component
        private readonly ForrealService _service;
        readonly IPopupService popupService;
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
        public bool ShowVideoSubmit
        {
            get => _showvideosubmit;
            set { if (_showvideosubmit != value) { _showvideosubmit = value; OnPropertyChange(); } }
        }
        public bool ShowChallanges
        {
            get => _showchallanges;
            set { if (_showchallanges != value) { _showchallanges = value; OnPropertyChange(); } }
        }
        public bool ShowFriend
        {
            get => _showfriend;
            set { if (_showfriend != value) { _showfriend = value; OnPropertyChange(); } }
        }
        public string ShowChallangeText
        {
            get => _showChallangeText;
            set { if (_showChallangeText != value) { _showChallangeText = value; OnPropertyChange(); } }
        }
        public Post PostSelect
        {
            get => _postselect; set
            {
                if (_postselect != value)
                {
                    _postselect = value; OnPropertyChange();
                }
            }
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
        public bool IsHomePageLogIn
        {
            get => _ishomepagelogin; set
            {
                if (_ishomepagelogin != value)
                {
                    _ishomepagelogin = value; OnPropertyChange();
                }
            }
        }
        public HomePageViewModel(IPopupService _popupService, ForrealService service)
        {
            var time = DateTime.Now;
            string day = time.Day.ToString(); var month = time.Month.ToString(); var year = time.Year.ToString();
            post_chats = new ObservableCollection<ChatDto>();
            UsersWithID = new ObservableCollection<UserNameDto>();
            Posts = new ObservableCollection<Post>();
            FriendUsers = new ObservableCollection<User>();
            IsHomePageLogIn = true;
            ShowFriend = false;
            ShowSubmit = false;
            ShowVideoSubmit = false;
            ShowChallanges = true;
            Challanges = new ObservableCollection<Challange>();
            _service = service;
            popupService = _popupService;
            UserName = "Welcome to Homepage " + ((App)Application.Current).User.UserName;
            ShowChallangeText = "To see friends posts do a Challange!";
            PageAppearingCommand = new Command(OnPageAppearing);

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
            if (ShowFriend)
            {
                AppShell.Current.DisplayAlert("You've got a friend request!", "go to Search to see more info", "cancel");
            }
            // checks if the user already did a challenge for the day, if so adds the posts
            foreach (var name in ImagesName)
            {
               PostData data=CreatePostData(name);

                    if (MainPageViewModel.UserID == data.UserId && data.Date == DateTime.Now.Date)
                    {
                        ShowChallanges = false;
                    var posty = new Post() { is_image = false, is_video = false };
                        if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType == "png")
                        {
                            foreach (var ch in ChallangesNames)
                            {
                                if (ch.Id == data.ChallengeId)
                                    posty = new Post { username = ((App)(Application.Current)).User.UserName, challengename = ch.Text, TaskDate=data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image = true, is_video = false };
                            }

                        }

                        else if (data.FileType == "mp4" || data.FileType == "mp3")
                        {
                            foreach (var ch in ChallangesNames)
                            {
                                if (ch.Id == data.ChallengeId)
                                    posty = new Post { username = ((App)(Application.Current)).User.UserName, challengename = ch.Text, TaskDate=data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}", is_image = false, is_video = true };
                            }
                        }
                        if (posty.is_image || posty.is_video)
                            Posts.Add(posty);
                    }
            }
            bool have_no_friends = true;
            //now adds friends posts
            if (!ShowChallanges)
            {
                foreach (var u in Users)
                {
                    if (UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName))
                        FriendUsers.Add(u);
                }
                UsersWithID = UsersNames;
                foreach (var name in ImagesName)
                {
                    
                    PostData data=CreatePostData(name);
                        foreach (var user in UsersWithID)
                        {
                            bool IsFriend = FriendUsers.Any(friend => friend.UserName == user.Text);
                            if (IsFriend && data.UserId == user.Id && data.Date==DateTime.Now.Date)
                            {
                            string text = null;
                                foreach (var ch_name in ChallangesNames)
                                {
                                    if (ch_name.Id == data.ChallengeId)
                                        text = ch_name.Text;
                                }
                            var posty = new Post() { is_image = false, is_video = false };
                                if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType == "png")
                                    posty = new Post { username = user.Text, challengename = text, TaskDate = data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image = true, is_video = false };
                                else if (data.FileType == "mp4" || data.FileType == "mp3")
                                    posty = new Post { username = user.Text, challengename = text, TaskDate = data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}", is_image = false, is_video = true };
                                    if (posty.is_image || posty.is_video)
                                    {
                                        Posts.Add(posty);
                                        have_no_friends = false;

                                    }  
                            }

                        }                   
                }
                if (have_no_friends)
                {
                    ShowChallanges=true;
                    ShowChallangeText = "It's very quiet here... add som friends!";
                }
            }
            ChallangeCommand = new Command(async () =>
            {

                try
                {
                    if (Challanges.Count == 0)
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
                await popupService.ShowPopupAsync<ChallangePageViewModel>(onPresenting: vm => vm.SelectedImage += (s, e) =>
                {
                    SaveFileChages();
                });

                SaveFileChages();

            });

            CloseChallange = new Command(async () =>
            {
                await ChallangePage.ClosePopup();
                SaveFileChages();
            });
            NoCommand = new Command(async () =>
            {
                ShowSubmit = false;
                ShowVideoSubmit = false;
            });
            YesCommand = new Command(async () => await DisplayPosts());
            ChatCommand = new Command(async () =>
            {
                var postchats = await _service.GetPostComments(PostSelect.username, PostSelect.challengename,PostSelect.TaskDate);
                post_chats = postchats.chats;
                PostSelecting = PostSelect;
            await popupService.ShowPopupAsync<ChatPageViewModel>();
            });
            CloseChat = new Command(async () =>
            {
                await ChatPage.ClosePopup();
            });
        }
        // Method to handle the page appearing
        private void OnPageAppearing()
        {
            if(((App)Application.Current).IsLogIn == false)
            {
                IsHomePageLogIn = false;
            }
        }
        private async Task DisplayPosts()
        {
            try
            {
                Posts = new ObservableCollection<Post>();
                string username = ((App)(Application.Current)).User.UserName;
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
                    ShowChallanges = false;
                    ShowSubmit = false;
                    ShowVideoSubmit = false;
                    #region adds challanges the user did today
                    foreach (var name in ImagesName)
                    {
                        PostData data=CreatePostData(name);
                            if (MainPageViewModel.UserID == data.UserId && data.Date==DateTime.Now.Date)
                            {
                                ShowChallanges = false;
                            var posty = new Post() { is_image = false, is_video = false };
                                if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType == "png")
                                {
                                    foreach (var ch in ChallangesNames)
                                    {
                                        if (ch.Id == data.ChallengeId)
                                            posty = new Post { username = ((App)(Application.Current)).User.UserName, challengename = ch.Text, TaskDate = data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image = true, is_video = false };
                                    }

                                }

                                else if (data.FileType == "mp4" || data.FileType == "mp3")
                                {
                                    foreach (var ch in ChallangesNames)
                                    {
                                        if (ch.Id == data.ChallengeId)
                                            posty = new Post { username = ((App)(Application.Current)).User.UserName, challengename = ch.Text, TaskDate = data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}", is_image = false, is_video = true };
                                    }

                                }
                                if (posty.is_image || posty.is_video)
                                    Posts.Add(posty);
                            }
                    }
                    #endregion
                    #region adds the challange which submitted right now
                    var postt = new Post();
                    string namee = file.FileName;
                    string path = file.FullPath;
                    var infoes = namee.Split('.');
                    var data1 = new PostData { UserId = MainPageViewModel.UserID, ChallengeId = (await _service.GetChallangeID(challangename)).Id, Date = DateTime.Now, FileType = infoes[1] };
                    if (data1.FileType == "jpg" || data1.FileType == "jpeg" || data1.FileType == "png")
                        postt = new Post { username = post.username, challengename = post.challengename, TaskDate=data1.Date, image = file.FullPath, is_image = true, is_video = false };
                    else if (data1.FileType == "mp4" || data1.FileType == "mp3")
                        postt = new Post { username = post.username, challengename = post.challengename, TaskDate = data1.Date, image = file.FullPath, is_image = false, is_video = true };
                    if (postt.is_image || postt.is_video)
                        Posts.Add(postt);
                    #endregion
                    #region adds friends posts in the current day
                    bool have_no_friends = true;
                    foreach (var u in Users)
                    {
                        if (UsersNameWant.Contains(u.UserName) && UsersNameRequest.Contains(u.UserName))
                            FriendUsers.Add(u);
                    }
                    UsersWithID = UsersNames;
                    foreach (var name in ImagesName)
                    {

                        PostData data = CreatePostData(name);
                        foreach (var user in UsersWithID)
                        {
                            bool IsFriend = FriendUsers.Any(friend => friend.UserName == user.Text);
                            if (IsFriend && data.UserId == user.Id && data.Date == DateTime.Now.Date)
                            {
                                string text = null;
                                foreach (var ch_name in ChallangesNames)
                                {
                                    if (ch_name.Id == data.ChallengeId)
                                        text = ch_name.Text;
                                }
                                var posty = new Post() { is_image = false, is_video = false };
                                if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType == "png")
                                    posty = new Post { username = user.Text, challengename = text, TaskDate = data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image = true, is_video = false };
                                else if (data.FileType == "mp4" || data.FileType == "mp3")
                                    posty = new Post { username = user.Text, challengename = text, TaskDate = data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}", is_image = false, is_video = true };
                                if (posty.is_image || posty.is_video)
                                {
                                    Posts.Add(posty);
                                    have_no_friends = false;
                                }
                                   
                            }

                        }
                    }
                    if (have_no_friends)
                    {
                        ShowChallanges = true;
                        ShowChallangeText = "It's very quiet here... add som friends! ";
                    }                    
                    #endregion
                }

                await AppShell.Current.DisplayAlert("All done!", "the post has submitted", "cancel");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await AppShell.Current.Navigation.PopModalAsync();

            }
        }
        



        private PostData CreatePostData(string name)
        {
            PostData postData = new PostData();
            var infoes = name.Split('_');
            string[] infofoes = null;
            if (infoes.Length > 3)
            {
                infofoes = infoes[4].Split(".");

                postData.UserId = Int32.Parse(infoes[0]);
                postData.Date = new DateTime(Int32.Parse(infofoes[0]), Int32.Parse(infoes[3]), Int32.Parse(infoes[2]));
                postData.ChallengeId = Int32.Parse(infoes[1]);
                postData.FileType = infofoes[1];

            }
            return postData;
        }
        private void SaveFileChages()
        {
            OnPropertyChange(nameof(ShowSubmit));
            OnPropertyChange(nameof(ShowVideoSubmit));
            OnPropertyChange(nameof(ChallengeSubmit));
            OnPropertyChange(nameof(ImageSubmit));
            OnPropertyChange(nameof(VideoSubmit));
            OnPropertyChange(nameof(FileSubmit));
        }

    }
}
