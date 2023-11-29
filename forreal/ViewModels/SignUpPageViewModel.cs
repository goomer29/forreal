﻿using System;
using forreal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using forreal.Models;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class SignUpPageViewModel:ViewModel
    {
        #region Fields
        private bool _showLoginError;//האם להציג שגיאת התחברות
        private string _loginErrorMessage;//תאור שגיאת התחברות

        private string _userName;//שם משתמש
        private bool _showUserNameError;//האם להציג שדה שגיאת שם משתמש
        private string _userErrorMessage;//תאור שגיאת שם משתמש

        private string _password;//סיסמה
        private bool _showPasswordError;//האם להציג שגיאת סיסמה
        private string _passwordErrorMessage;//תאור שגיאת סיסמה

        #endregion
        #region Service component
        private readonly ForrealService _service;
        #endregion
        #region Commands
        public ICommand SignInCommand { get; protected set; }
        #endregion
        #region Proporties
        public bool ShowLoginError { get => _showLoginError; set { if (_showLoginError != value) { _showLoginError = value; OnPropertyChange(); } } }
        public string LoginErrorMessage { get => _loginErrorMessage; set { if (_loginErrorMessage != value) { _loginErrorMessage = value; OnPropertyChange(); } } }
        public string UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; if (!ValidateUser()) { ShowUserNameError = true; UserErrorMessage = ErrorMessages.INVALID_USERNAME; } else { ShowUserNameError = true; UserErrorMessage = string.Empty; } OnPropertyChange(); OnPropertyChange(nameof(IsButtonEnabled)); } }
        }
        public bool ShowUserNameError
        {
            get => _showUserNameError; set
            {
                if (_showUserNameError != value)
                {
                    _showUserNameError = value; OnPropertyChange();
                }
            }
        }
        public string UserErrorMessage
        {
            get => _userErrorMessage;
            set { if (_userErrorMessage != value) { _userErrorMessage = value; OnPropertyChange(); } }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value; if (!ValidatePassWord())
                    {
                        ShowPasswordError = true;
                        PasswordErrorMessage = ErrorMessages.INVALID_PASSWORD;
                    }
                    else
                    {
                        PasswordErrorMessage = string.Empty;
                        ShowPasswordError = false;
                    };
                    OnPropertyChange();
                    OnPropertyChange(nameof(IsButtonEnabled));
                }
            }
        }
        public bool ShowPasswordError
        {
            get => _showPasswordError;
            set { if (_showPasswordError != value) { _showPasswordError = value; OnPropertyChange(); } }
        }
        public string PasswordErrorMessage { get => _passwordErrorMessage; set { if (_passwordErrorMessage != value) { _passwordErrorMessage = value; OnPropertyChange(); } } }

        public bool IsButtonEnabled { get { return ValidatePage(); } }
        #endregion
        #region פעולות עזר
        private bool ValidateUser()
        {
            return !(string.IsNullOrEmpty(_userName) || _userName.Length < 3 || _userName.Length > 15);
        }
        private bool ValidatePassWord()
        {
            return !string.IsNullOrEmpty(_password);
        }
        private bool ValidatePage()
        {
            return ValidateUser() && ValidatePassWord();
        }
        #endregion
    }
}
