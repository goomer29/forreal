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
