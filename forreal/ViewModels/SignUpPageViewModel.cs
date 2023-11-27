using System;
using forreal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class SignUpPageViewModel:ViewModel
    {
        #region Fields
        private bool _showLoginError;//האם להציג שגיאת התחברות
        private string _loginErrorMessage;//תאור שגיאת התחברות
        #endregion
        #region Service component
        private readonly ForrealService _service;
        #endregion
        #region Proporties
        public bool ShowLoginError { get => _showLoginError; set { if (_showLoginError != value) { _showLoginError = value; OnPropertyChange(); } } }
        public string LoginErrorMessage { get => _loginErrorMessage; set { if (_loginErrorMessage != value) { _loginErrorMessage = value; OnPropertyChange(); } } }
        #endregion
    }
}
