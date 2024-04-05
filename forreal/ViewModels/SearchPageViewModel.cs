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
        public ObservableCollection<User> Users { get => MainPageViewModel.Users; }
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
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public SearchPageViewModel(ForrealService service)
        {
            _service = service;
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
