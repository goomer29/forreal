using forreal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class LoginPageViewModel:ViewModel
    {
        #region Fields
        private string _userName;//שם משתמש
        private bool _showUserNameError;//האם להציג שדה שגיאת שם משתמש
        private string _userErrorMessage;//תאור שגיאת שם משתמש
        private string _password;//סיסמה

        private bool _showPasswordError;//האם להציג שגיאת סיסמה
        private string _passwordErrorMessage;
        private bool _showLoginError;//
        private string _loginErrorMessage;
        #endregion
        #region Service component
        private readonly ForrealService _service;
        #endregion
        #region פעולות עזר
        private bool ValidateUser()
        {
            return !(string.IsNullOrEmpty(_userName) || _userName.Length < 3||_userName.Length>15);
        }
        private bool ValidatePassWord()
        {
            //return !string.IsNullOrEmpty(Password);
            return true;
        }

        private bool ValidatePage()
        {
            return ValidateUser() && ValidatePassWord();
        }
        #endregion

    }
}
