using forreal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace forreal.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        readonly IPopupService popupService;
        private string _userName;//שם משתמש
        public ICommand ChallangeCommand { get; protected set; }
        public ICommand CloseChallange { get; protected set; }
        public string UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; OnPropertyChange(); } }
        }
        public HomePageViewModel(IPopupService _popupService)
        {
            popupService = _popupService;

            UserName = "Welcome to Homepage " + ((App)Application.Current).User.UserName;
            ChallangeCommand = new Command(async () =>
            {
                await popupService.ShowPopupAsync<ChallangePageViewModel>();
            });

            CloseChallange = new Command(async () =>
            {
                await ChallangePage.CloseInstance();
            });
        }
    }
}
