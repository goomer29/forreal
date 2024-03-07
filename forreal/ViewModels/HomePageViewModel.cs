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

namespace forreal.ViewModels
{
    public class HomePageViewModel : ViewModel
    {
        readonly IPopupService popupService;
        private string _userName;//שם משתמש
        public ICommand ChallangeCommand { get; protected set; }
        public ICommand CloseChallange { get; protected set; }
        public static ObservableCollection<Challange> statChallanges { get; set; }
        public ObservableCollection<Challange> Challanges { get; set;  }
        public string ChallengeName { get => ChallangePageViewModel.challange_select.Text; }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public string UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; OnPropertyChange(); } }
        }
        public HomePageViewModel(IPopupService _popupService, ForrealService service)
        {
            Challanges = new ObservableCollection<Challange>();
            _service = service;
            popupService = _popupService;

            UserName = "Welcome to Homepage " + ((App)Application.Current).User.UserName;
            ChallangeCommand = new Command(async () =>
            {            
                try
                {
                    if(Challanges.Count == 0)
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
                await popupService.ShowPopupAsync<ChallangePageViewModel>();
            });

            CloseChallange = new Command(async () =>
            {
                await ChallangePage.ClosePopup();
            });
        }
    }
}
