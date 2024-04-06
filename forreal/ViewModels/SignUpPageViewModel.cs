using System;
using forreal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using forreal.Models;
using forreal.Views;
using System.Windows.Input;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace forreal.ViewModels
{
    public class SignUpPageViewModel:ViewModel
    {
        #region Fields
        private bool _showSignUpError;//האם להציג שגיאת התחברות
        private string _signupErrorMessage;//תאור שגיאת התחברות

        private string _userName;//שם משתמש
        private bool _showUserNameError;//האם להציג שדה שגיאת שם משתמש
        private string _userErrorMessage;//תאור שגיאת שם משתמש

        private string _password;//סיסמה
        private bool _showPasswordError;//האם להציג שגיאת סיסמה
        private string _passwordErrorMessage;//תאור שגיאת סיסמה

        private string _repassword;//שחזור סיסמה
        private bool _showRePasswordError;//האם להציג שגיאת שחזור סיסמה
        private string _repasswordErrorMessage;//תיאור שגירת שחזור סיסמה

        private string _email;//אימייל
        private bool _showEmailError;//האם להציג שגיאת אימייל
        private string _emailErrorMessage;//תאור שגיאת אימייל
        #endregion
        #region Service component
        private readonly ForrealService _service;
        #endregion
        #region Commands
        public ICommand SignInCommand { get; protected set; }
        public ICommand LogInCommand { get; protected set; }
        #endregion
        #region Proporties
        public bool ShowSignUpError { get => _showSignUpError; set { if (_showSignUpError != value) { _showSignUpError = value; OnPropertyChange(); } } }
        public string SignUpErrorMessage { get => _signupErrorMessage; set { if (_signupErrorMessage != value) { _signupErrorMessage = value; OnPropertyChange(); } } }
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
                    if (!ValidateRePassWord())
                    {
                        ShowRePasswordError = true;
                        RePasswordErrorMessage = ErrorMessages.INVALID_REPASSWORD;
                    }
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
        public string RePassword
        {
            get => _repassword;
            set
            {
                if (_repassword != value)
                {
                    _repassword = value; if (!ValidateRePassWord())
                    {
                        ShowRePasswordError = true;
                        RePasswordErrorMessage = ErrorMessages.INVALID_REPASSWORD;
                    }
                    else
                    {
                        RePasswordErrorMessage = string.Empty;
                        ShowRePasswordError = false;
                    };
                    OnPropertyChange();
                    OnPropertyChange(nameof(IsButtonEnabled));
                }
            }
        }
        public bool ShowRePasswordError
        {
            get => _showRePasswordError;
            set { if (_showRePasswordError != value) { _showRePasswordError = value; OnPropertyChange(); } }
        }
        public string RePasswordErrorMessage { get => _repasswordErrorMessage; set { if (_repasswordErrorMessage != value) { _repasswordErrorMessage = value; OnPropertyChange(); } } }
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value; if (!ValidateEmail())
                    {
                        ShowEmailError = true;
                       EmailErrorMessage = ErrorMessages.INVALID_EMAIL;
                    }
                    else
                    {
                        EmailErrorMessage = string.Empty;
                        ShowEmailError = false;
                    };
                    OnPropertyChange();
                    OnPropertyChange(nameof(IsButtonEnabled));
                }
            }
        }
        public bool ShowEmailError
        {
            get => _showEmailError;
            set { if (_showEmailError != value) { _showEmailError = value; OnPropertyChange(); } }
        }
        public string EmailErrorMessage { get => _emailErrorMessage; set { if (_emailErrorMessage != value) { _emailErrorMessage = value; OnPropertyChange(); } } }
        public bool IsButtonEnabled { get { return ValidatePage(); } }
        #endregion
        public SignUpPageViewModel(ForrealService service)
        {
            _service=service;
            UserName = string.Empty;
            Password=string.Empty;
            Email=string.Empty;

            SignInCommand = new Command(async () =>
            {
                ShowSignUpError = false;//הסתרת שגיאת התחברות
                try
                {
                    #region טעינת מסך ביניים
                    var lvm = new LoadingPageViewModel() { IsBusy = true };
                    await AppShell.Current.Navigation.PushModalAsync(new LoadingPage(lvm));
                    #endregion
                    var user = await _service.SignUpAsync(UserName, Password, Email);

                    lvm.IsBusy = false;
                    await Shell.Current.Navigation.PopModalAsync();
                    if (!user.Success)
                    {
                        ShowSignUpError = true;
                        SignUpErrorMessage = user.Message;
                        UserName = null; Password = null;
                    }
                    else
                    {
                        await AppShell.Current.DisplayAlert("You signed up!", "Click cancel to start", "cancel");
                        await SecureStorage.Default.SetAsync("SignedUser", JsonSerializer.Serialize(user.User));
                        ((App)(Application.Current)).ShowFlyouts = true;
                        ((App)(Application.Current)).ShowFlyouts2 = false;
                        #region Gets all users and info for search friends
                        var allusers = await _service.GetAllUsers();
                        var userim = allusers.UsersList;
                        ObservableCollection<User> users = new ObservableCollection<User>();
                        for (int i = 0; i < userim.Count; i++)
                        {
                            if (userim[i].UserName != ((App)(Application.Current)).User.UserName)
                            {
                                users.Add(userim[i]);
                            }

                        }
                        MainPageViewModel.AllUsers = users;
                        var wantedusers = await _service.GetWantedFriends(((App)(Application.Current)).User.UserName);
                        MainPageViewModel.WantedUsers = wantedusers.UsersList;
                        var requestusers = await _service.GetWRequestFriends(((App)(Application.Current)).User.UserName);
                        MainPageViewModel.RequestUsers = requestusers.UsersList;
                        #endregion
                        await AppShell.Current.GoToAsync("//HomePage");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    await AppShell.Current.Navigation.PopModalAsync();
                }
            });

            LogInCommand = new Command(async () =>
            {
                await AppShell.Current.GoToAsync("Login");
            });
        }
        #region פעולות עזר
        private bool ValidateUser()
        {
            return !(string.IsNullOrEmpty(_userName) || _userName.Length < 3 || _userName.Length > 15);
        }
        private bool ValidatePassWord()
        {
            return !string.IsNullOrEmpty(_password);
        }
        private bool ValidateRePassWord()
        {
            if(_repassword != null&&_password!=null)
            return _repassword.Equals(_password);
            return false;
        }
        private bool ValidateEmail()
        {
            if(_email!=null)
           return _email.Contains('@');
            return false;
        }
        private bool ValidatePage()
        {
            return ValidateUser() && ValidatePassWord() && ValidateRePassWord() && ValidateEmail();
        }
        #endregion
    }
}
