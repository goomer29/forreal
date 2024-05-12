using forreal.Models;
using forreal.Services;
using forreal.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    public class ChatPageViewModel:ViewModel
    {
        private string _message;
        private ObservableCollection<Chat> _chats;
        public Post PostSelect { get => HomePageViewModel.PostSelecting; }
        public ObservableCollection<ChatDto> ChatsDto { get=>HomePageViewModel.post_chats; }
        public ICommand SendMessage { get;protected set; }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public ObservableCollection<Chat> Chats
        {
            get => _chats; set
            {
                if (_chats != value)
                {
                    _chats = value; OnPropertyChange();
                }
            }
        }
        public string Message
        {
            get => _message; set
            {
                if (_message != value)
                {
                    _message = value; OnPropertyChange();
                }
            }
        }
        public ChatPageViewModel(ForrealService service)
        {
            _service = service;
            Chats = new ObservableCollection<Chat>();
            foreach (var chatdto in ChatsDto)
            {
                Chat chat = new Chat() { Username = chatdto.username, Text = chatdto.text, Time = chatdto.time };
                if (chat.Username == ((App)(Application.Current)).User.UserName)
                {
                    chat.IsCurrentUser = true;
                }
                else
                    chat.IsCurrentUser = false;
                Chats.Add(chat);
            }
            SendMessage = new Command(async () =>
            {
                var username = PostSelect.username;
                string challangename = PostSelect.challengename;
                var usersent = ((App)(Application.Current)).User.UserName;

                var response = await _service.AddMessage(username,challangename,usersent,Message);
                if (response == System.Net.HttpStatusCode.OK)
                {
                    Chat chat = new Chat() { Username = ((App)(Application.Current)).User.UserName, Text = Message, Time = DateTime.Now, IsCurrentUser = true };
                    Chats.Add(chat);
                
                }
                    
                    
            });
        }
    }
}
