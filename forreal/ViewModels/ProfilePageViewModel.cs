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
    public class ProfilePageViewModel:ViewModel
    {
        public User User { get; set; }
        public int Id { get; set; }
        public ObservableCollection<Post> _posts { get; set; }
        #region Service component
        private readonly ForrealService _service;
        #endregion
        public ObservableCollection<Post> Posts
        {
            get => _posts; set
            {
                if (_posts != value)
                {
                    _posts = value; OnPropertyChange();
                }
            }
        }
        public ProfilePageViewModel(ForrealService service)
        {
            Posts= new ObservableCollection<Post>();
            _service = service;
            User= ((App)Application.Current).User;
            var id = _service.GetUserID(User.UserName);
            Id = id.Result;
            var ImagesName = (_service.GetImages()).Result;
            foreach(var name in ImagesName)
            {
                var infoes = name.Split('_');
                if (infoes[0] == Id.ToString())
                {
                    var post = new Post { username = User.UserName, challengename = infoes[1], date = infoes[2] + "/" + infoes[3] + "/" + infoes[4], image = $"{ForrealService.WwwRoot}/Images/{name}" };
                    Posts.Add(post);

                }
            }
            
        }

    }
}
