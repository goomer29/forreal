using forreal.Models;
using forreal.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class SearchPageViewModel: ViewModel
    {
        public ObservableCollection<User> Users { get => LoginPageViewModel.Users; }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public SearchPageViewModel(ForrealService service)
        {
            

        }
    }
}
