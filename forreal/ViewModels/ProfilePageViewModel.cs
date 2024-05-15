using CommunityToolkit.Maui.Core;
using forreal.Models;
using forreal.Views;
using forreal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui;

namespace forreal.ViewModels
{
    public class ProfilePageViewModel:ViewModel
    {
        public bool _isprofilepagelogin;
        public User User { get; set; }
        public int Id { get => MainPageViewModel.UserID; }
        public ObservableCollection<string> ImagesName { get => MainPageViewModel.Images; }
        public ObservableCollection<ChallangeNameDto> ChallangeNames { get=>MainPageViewModel.ChallangeNames; }
        public ObservableCollection<Post> _posts { get; set; }
        public Post _postselect;
        public ICommand ChatCommand { get; protected set; }
        public ICommand CloseChat { get; protected set; }
        public ICommand PageAppearingCommand { get; private set; } // Command to handle the page appearing
        public static bool ProfilePageLogOut { get; set; }
        #region Service component
        private readonly ForrealService _service;
        readonly IPopupService popupService;
        #endregion
        public  ObservableCollection<Post> Posts
        {
            get => _posts; set
            {
                if (_posts != value)
                {
                    _posts = value; OnPropertyChange();
                }
            }
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
        public bool IsProfilePageLogIn
        {
            get => _isprofilepagelogin; set
            {
                if (_isprofilepagelogin != value)
                {
                    _isprofilepagelogin = value; OnPropertyChange();
                }
            }
        }
        public ProfilePageViewModel(IPopupService _popupService, ForrealService service)
        {
            IsProfilePageLogIn = true;
            ProfilePageLogOut = false;
            PageAppearingCommand = new Command(OnPageAppearing);
            Posts = new ObservableCollection<Post>();
            _service = service;
            popupService = _popupService;
            User = ((App)Application.Current).User;
            foreach (var name in ImagesName)
            {
                PostData data=CreatePostData(name);
                string text = null;
                if (data.UserId == Id)
                {
                    foreach(var ch_name in ChallangeNames)
                    {
                        if (ch_name.Id == data.ChallengeId)
                        {
                           text=ch_name.Text; break;
                        }
                    }
                    var post = new Post();
                    post.is_image = false; post.is_video = false;
                    if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType=="png")
                    post = new Post { username = User.UserName, challengename = text, TaskDate=data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image=true , is_video=false};
                    else if (data.FileType == "mp4" || data.FileType=="mp3")
                      post = new Post { username = User.UserName, challengename = text, TaskDate=data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}",is_image=false, is_video = true};
                    if(post.is_image||post.is_video)
                    Posts.Add(post);

                }
            }
            OnPropertyChange(nameof(Posts));

            ChatCommand = new Command(async () =>
            {
                var postchats = await _service.GetPostComments(PostSelect.username, PostSelect.challengename, PostSelect.TaskDate);
                HomePageViewModel.post_chats = postchats.chats;
                HomePageViewModel.PostSelecting = PostSelect;
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
            if (((App)Application.Current).IsLogIn == false)
            {
                IsProfilePageLogIn = false;
            }
            if(((App)Application.Current).User != null && ProfilePageLogOut)
            {
                #region runs the constructor
                IsProfilePageLogIn = true;
                PageAppearingCommand = new Command(OnPageAppearing);
                Posts = new ObservableCollection<Post>();
                User = ((App)Application.Current).User;
                foreach (var name in ImagesName)
                {
                    PostData data = CreatePostData(name);
                    string text = null;
                    if (data.UserId == Id)
                    {
                        foreach (var ch_name in ChallangeNames)
                        {
                            if (ch_name.Id == data.ChallengeId)
                            {
                                text = ch_name.Text; break;
                            }
                        }
                        var post = new Post();
                        post.is_image = false; post.is_video = false;
                        if (data.FileType == "jpg" || data.FileType == "jpeg" || data.FileType == "png")
                            post = new Post { username = User.UserName, challengename = text, TaskDate = data.Date, image = $"{ForrealService.WwwRoot}/Images/{name}", is_image = true, is_video = false };
                        else if (data.FileType == "mp4" || data.FileType == "mp3")
                            post = new Post { username = User.UserName, challengename = text, TaskDate = data.Date, video = $"{ForrealService.WwwRoot}/Images/{name}", is_image = false, is_video = true };
                        if (post.is_image || post.is_video)
                            Posts.Add(post);

                    }
                }
                OnPropertyChange(nameof(Posts));
                #endregion
                ProfilePageLogOut = false;
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
    }
}
