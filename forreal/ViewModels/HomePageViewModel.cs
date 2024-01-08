using forreal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class HomePageViewModel:ViewModel
    {
        private string _userName;//שם משתמש
        public ICommand ChallangeCommand { get; protected set; }
        public string UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; OnPropertyChange(); } }
        }
        public HomePageViewModel() 
        {
            UserName = "Welcome to Homepage "+((App)Application.Current).User.UserName;
            ChallangeCommand = new Command(async () => 
            {
                

            });
        }
    }
}
