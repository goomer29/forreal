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
    public class SearchPageViewModel: ViewModel
    {
        private int red_count { get; set; }//the remove makes the function to go agian so to not enter it again
        private int yellow_count { get; set; }//""
        private int green_count { get; set; }//""
        private int blue_count { get; set; }//""
        private User user_red_select { get; set; }//not friends and dont want to
        private User user_yellow_select { get; set; }//want to be friends with
        private User user_green_select { get; set; }//friends
        private User user_blue_select { get; set; }//users that wants to be friend
        private ObservableCollection<User> red_users { get; set; }
        private ObservableCollection<User> yellow_users { get; set; }
        private ObservableCollection<User> green_users { get; set; }
        private ObservableCollection<User> blue_users { get; set; }
        private ObservableCollection<User> wanted_users { get; set; }
        private ObservableCollection<User> request_users {  get; set; }
        public ObservableCollection<User> Users { get => MainPageViewModel.AllUsers; }
        public ObservableCollection<string> UsersNameWant { get => MainPageViewModel.WantedUsers; }
        public ObservableCollection<string> UsersNameRequest { get => MainPageViewModel.RequestUsers; }
        public ICommand SelectRedCommand { get; protected set; }//making a friend request
        public ICommand SelectYellowCommand { get; protected set; }//deleting a friend request
        public ICommand SelectGreenCommand { get; protected set; }//deleting friend request which lead to no more being friends
        public ICommand SelectBlueCommand { get; protected set; }//making a friend request which leads to friends
        public User UserRedSelect
        {
            get => user_red_select; set
            {
                if (user_red_select != value)
                {
                    user_red_select = value; OnPropertyChange();
                }
            }
        }
        public User UserYellowSelect
        {
            get => user_yellow_select; set
            {
                if (user_yellow_select != value)
                {
                    user_yellow_select = value; OnPropertyChange();
                }
            }
        }
        public User UserGreenSelect
        {
            get => user_green_select; set
            {
                if (user_green_select != value)
                {
                    user_green_select = value; OnPropertyChange();
                }
            }
        }
        public User UserBlueSelect
        {
            get => user_blue_select; set
            {
                if (user_blue_select != value)
                {
                    user_blue_select = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> WantedUsers
        {
            get => wanted_users; set
            {
                if (wanted_users != value)
                {
                    wanted_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> RequestUsers
        {
            get => request_users; set
            {
                if (request_users != value)
                {
                    request_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> RedUsers
        {
            get => red_users; set
            {
                if (red_users != value)
                {
                    red_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> YellowUsers
        {
            get => yellow_users; set
            {
                if (yellow_users != value)
                {
                    yellow_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> GreenUsers
        {
            get => green_users; set
            {
                if (green_users != value)
                {
                    green_users = value; OnPropertyChange();
                }
            }
        }
        public ObservableCollection<User> BlueUsers
        {
            get => blue_users; set
            {
                if (blue_users != value)
                {
                    blue_users = value; OnPropertyChange();
                }
            }
        }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public SearchPageViewModel(ForrealService service)
        {
            red_count = 1;
            yellow_count = 1;
            green_count = 1;
            blue_count = 1;
            WantedUsers = new ObservableCollection<User>();
            RequestUsers = new ObservableCollection<User>();
            RedUsers = new ObservableCollection<User>();
            YellowUsers = new ObservableCollection<User>();
            GreenUsers = new ObservableCollection<User>();
            BlueUsers = new ObservableCollection<User>();
            foreach (User u in Users)
            {
                if (UsersNameWant.Contains(u.UserName))
                    WantedUsers.Add(u);
                if (UsersNameRequest.Contains(u.UserName))
                    RequestUsers.Add(u);
            }
            _service = service;
            try
            {
                foreach (User u in Users)
                {
                    if (!(WantedUsers.Contains(u) || RequestUsers.Contains(u)))
                        RedUsers.Add(u);
                    else if (WantedUsers.Contains(u) && !RequestUsers.Contains(u))
                        YellowUsers.Add(u);
                    else if (WantedUsers.Contains(u) && RequestUsers.Contains(u)) 
                        GreenUsers.Add(u);
                    else
                        BlueUsers.Add(u);
                }
                OnPropertyChange(nameof(RedUsers));
                OnPropertyChange(nameof(YellowUsers));
                OnPropertyChange(nameof(GreenUsers));
                OnPropertyChange(nameof(BlueUsers));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AppShell.Current.Navigation.PopModalAsync();
            }
            SelectRedCommand = new Command(async () =>
            {
                red_count++;
                if (red_count % 2 == 0)
                {
                    string myuser = ((App)Application.Current).User.UserName;
                    string otheruser = UserRedSelect.UserName;
                    var response = await _service.FriendRequest(myuser, otheruser);
                    if (response == System.Net.HttpStatusCode.OK)
                    {
                        await AppShell.Current.DisplayAlert("The Friend Request Submitted!", "wait to the other user to submit your request", "cancel");
                        YellowUsers.Add(UserRedSelect);
                        RedUsers.Remove(UserRedSelect);
                    }
                }              
                    OnPropertyChange(nameof(RedUsers));
                    OnPropertyChange(nameof(YellowUsers));
            });
            SelectYellowCommand = new Command(async () =>
            {
                yellow_count++;
                if(yellow_count % 2 == 0)
                {
                    string myuser = ((App)Application.Current).User.UserName;
                    string otheruser = UserYellowSelect.UserName;
                    var response = await _service.EnemyRequest(myuser, otheruser);
                    if (response == System.Net.HttpStatusCode.OK)
                    {
                        await AppShell.Current.DisplayAlert("The Friend request Deleted", "the friend request deleted successfully", "cancel");
                        RedUsers.Add(UserYellowSelect);
                        YellowUsers.Remove(UserYellowSelect);
                    }
                }           
                    OnPropertyChange(nameof(RedUsers));
                    OnPropertyChange(nameof(YellowUsers));
                
            });
            SelectGreenCommand = new Command(async () =>
            {
                green_count++;
                if(green_count % 2 == 0)
                {
                    string myuser = ((App)Application.Current).User.UserName;
                    string otheruser = UserGreenSelect.UserName;
                    var response1 = await _service.EnemyRequest(myuser, otheruser);
                    var response2 = await _service.EnemyRequest(otheruser, myuser);
                    if (response1 == System.Net.HttpStatusCode.OK && response2 == System.Net.HttpStatusCode.OK)
                    {
                        await AppShell.Current.DisplayAlert("The Friend request Deleted", "you are now no more friends", "cancel");
                        RedUsers.Add(UserGreenSelect);
                        GreenUsers.Remove(UserGreenSelect);
                    }
                }            
                    OnPropertyChange(nameof(RedUsers));
                    OnPropertyChange(nameof(GreenUsers));             
            });
            SelectBlueCommand = new Command(async () =>
            {
                blue_count++;
                if(blue_count % 2 == 0)
                {
                    string myuser = ((App)Application.Current).User.UserName;
                    string otheruser = UserBlueSelect.UserName;
                    var response = await _service.FriendRequest(myuser, otheruser);
                    if (response == System.Net.HttpStatusCode.OK)
                    {
                        await AppShell.Current.DisplayAlert("The Friend Request Submitted!", "you are now friends", "cancel");
                        GreenUsers.Add(UserBlueSelect);
                        BlueUsers.Remove(UserBlueSelect);
                    }
                }          
                    OnPropertyChange(nameof(BlueUsers));
                    OnPropertyChange(nameof(GreenUsers));              
            });
        }
    }
}
