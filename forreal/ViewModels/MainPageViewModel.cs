using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class MainPageViewModel: ViewModel
    {
        public ICommand LogInCommand { get; protected set; }
        public ICommand SignUpCommand { get; protected set; }
        public MainPageViewModel()
        {
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
