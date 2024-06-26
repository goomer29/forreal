﻿using forreal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using forreal.Views;
using forreal.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using Microsoft.Maui.Layouts;

namespace forreal.ViewModels
{
    public class LoginPageViewModel:ViewModel
    {
        #region Fields
        private string _userName;//שם משתמש
        private bool _showUserNameError;//האם להציג שדה שגיאת שם משתמש
        private string _userErrorMessage;//תאור שגיאת שם משתמש
        private string _password;//סיסמה
        private IServiceProvider provider;

        private bool _showPasswordError;//האם להציג שגיאת סיסמה
        private string _passwordErrorMessage;//תאור שגיאת סיסמה
        private bool _showLoginError;//האם להציג שגיאת התחברות
        private string _loginErrorMessage;//תאור שגיאת התחברות

        #endregion
        #region Service component
        private readonly ForrealService _service;
        #endregion
        #region Proporties
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
        public string UserErrorMessage { get => _userErrorMessage; 
            set { if (_userErrorMessage != value) { _userErrorMessage = value; OnPropertyChange(); } } }
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
        public bool ShowLoginError { get => _showLoginError; set { if (_showLoginError != value) { _showLoginError = value; OnPropertyChange(); } } }
        public string LoginErrorMessage { get => _loginErrorMessage; set { if (_loginErrorMessage != value) { _loginErrorMessage = value; OnPropertyChange(); } } }
        //האם כפתור התחבר יהיה זמין
        public bool IsButtonEnabled { get { return ValidatePage(); } }

        #endregion
        #region Commands
        public ICommand LogInCommand { get; protected set; }
        public ICommand ForgotPasswordCommand { get; protected set; }
        public ICommand SignUpCommand { get; protected set; }
        #endregion
        public LoginPageViewModel(ForrealService service)
        {
            _service = service;
            UserName = string.Empty;
            Password = string.Empty;

            LogInCommand = new Command(async () =>
            {
            ShowLoginError = false;//הסתרת שגיאת לוגין
            try
            {
                #region טעינת מסך ביניים
                var lvm = new LoadingPageViewModel() { IsBusy = true };
                await AppShell.Current.Navigation.PushModalAsync(new LoadingPage(lvm));
                #endregion
                var user = await _service.LogInAsync(UserName, Password);

               
                    if (!user.Success)
                    {
                        lvm.IsBusy = false;
                        await Shell.Current.Navigation.PopModalAsync();

                        ShowLoginError = true;
                        LoginErrorMessage = user.Message;
                        UserName = null; Password = null;
                    }
                    else
                    {

                        #region Gets all users for search friends
                        MainPageViewModel.UserWithID = await _service.GetUserNameWithID();
                        var usersWithID = MainPageViewModel.UserWithID;
                        MainPageViewModel.AllUsers = CreateUsersCollection(usersWithID, ((App)(Application.Current)).User.UserName);

                        var wantedusers = await _service.GetWantedFriends(((App)(Application.Current)).User.UserName);
                        MainPageViewModel.WantedUsers = wantedusers.UsersNameList;

                        var requestusers = await _service.GetWRequestFriends(((App)(Application.Current)).User.UserName);
                        MainPageViewModel.RequestUsers = requestusers.UsersNameList;
                        #endregion                      
                        UserName=null ; Password=null;
                        int Id = 0;
                        foreach(var user_with_id in usersWithID)
                        {
                            if (user_with_id.Text == ((App)(Application.Current)).User.UserName)
                                Id = user_with_id.Id;
                        }
                        MainPageViewModel.UserID = Id;
                        MainPageViewModel.Images=await _service.GetImages();
                        MainPageViewModel.ChallangeNames = await _service.GetAllChallanges();
                        

                        lvm.IsBusy = false;
                        await Shell.Current.Navigation.PopModalAsync();

                        ((App)(Application.Current)).ShowFlyouts = true;
                        ((App)(Application.Current)).ShowFlyouts2 = false;
                        await AppShell.Current.DisplayAlert("Succceful logged in!", "Enter cancel to start", "cancel");
                        await SecureStorage.Default.SetAsync("LoggedUser", JsonSerializer.Serialize(user.User));

                        await AppShell.Current.GoToAsync("//HomePage");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);

                    await AppShell.Current.Navigation.PopModalAsync();
                }


            });
           
            SignUpCommand= new Command(async () =>
            {
                await AppShell.Current.GoToAsync("SignUp");

            });
        }//work on it brb
        #region פעולות עזר
        private bool ValidateUser()
        {
            return !(string.IsNullOrEmpty(_userName) || _userName.Length < 3||_userName.Length>15);
        }

        private bool ValidatePassWord()
        {
            return !string.IsNullOrEmpty(_password);
        }

        private bool ValidatePage()
        {
            return ValidateUser() && ValidatePassWord();
        }

        private ObservableCollection<User> CreateUsersCollection(ObservableCollection<UserNameDto> userNames, string current_user)
        {
            ObservableCollection<User> userim= new ObservableCollection<User>();
            foreach (UserNameDto username in userNames)
            {
                User u=new User() { UserName=username.Text, Email=username.Email };
                userim.Add(u);
            }
            ObservableCollection<User> users = new ObservableCollection<User>();
            for (int i = 0; i < userim.Count; i++)
            {
                if (userim[i].UserName != current_user)
                {
                    users.Add(userim[i]);
                }
            }
            return users;
        }
        #endregion


    }
}
