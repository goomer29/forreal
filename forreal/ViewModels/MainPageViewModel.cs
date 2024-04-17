using forreal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class MainPageViewModel: ViewModel
    {
        public static ObservableCollection<User> AllUsers { get; set; }
        public static ObservableCollection<string>WantedUsers { get; set; }
        public static ObservableCollection<string> RequestUsers { get; set; }
        public static int UserID { get; set; }
        public static ObservableCollection<string> Images { get; set; }
        public static ObservableCollection<ChallangeNameDto> ChallangeNames { get; set; }
        public ICommand LogInCommand { get; protected set; }
        public ICommand SignUpCommand { get; protected set; }
        public MainPageViewModel()
        {
            WantedUsers = new ObservableCollection<string>();
            RequestUsers = new ObservableCollection<string>();
            ((App)(Application.Current)).ShowFlyouts = false;
            ((App)(Application.Current)).ShowFlyouts2 = true;
            LogInCommand = new Command(async () =>
            {
                await AppShell.Current.GoToAsync("Login");
            });
            SignUpCommand = new Command(async () =>
            {
                await AppShell.Current.GoToAsync("SignUp");
            });
        }

    }
}
