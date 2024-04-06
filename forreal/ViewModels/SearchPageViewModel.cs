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
        private User user_select { get; set; }
        private ObservableCollection<User> red_users { get; set; }
        private ObservableCollection<User> green_users { get; set; }
        private ObservableCollection<User> wanted_users { get; set; }
        private ObservableCollection<User> request_users {  get; set; }
        public ObservableCollection<User> Users { get => MainPageViewModel.AllUsers; }
        public ICommand SelectCommand { get; protected set; }
        public User UserSelect
        {
            get => user_select; set
            {
                if (user_select != value)
                {
                    user_select = value; OnPropertyChange();
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
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public SearchPageViewModel(ForrealService service)
        {
            RedUsers= new ObservableCollection<User>();
            WantedUsers = MainPageViewModel.WantedUsers;
            RequestUsers = MainPageViewModel.RequestUsers;
            _service = service;
            try
            {
                foreach (User u in Users)
                {
                    if (!(WantedUsers.Contains(u) || RequestUsers.Contains(u)))
                        RedUsers.Add(u);
                }
                OnPropertyChange(nameof(RedUsers));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AppShell.Current.Navigation.PopModalAsync();
            }
            SelectCommand = new Command(async () =>
            {
                string myuser= ((App)Application.Current).User.UserName;
                string otheruser = UserSelect.UserName;
                var response=await _service.FriendRequest(myuser, otheruser);
                if (response == System.Net.HttpStatusCode.OK)
                {
                    await AppShell.Current.DisplayAlert("The Friend Request Submitted!", "wait to the other user to submit your request", "cancel");
                }
            });
        }
    }
}
