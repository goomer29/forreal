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
        public int Id { get => MainPageViewModel.UserID; }
        public ObservableCollection<string> ImagesName { get => MainPageViewModel.Images; }
        public ObservableCollection<ChallangeNameDto> ChallangeNames { get=>MainPageViewModel.ChallangeNames; }
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
            foreach (var name in ImagesName)
            {
                var infoes = name.Split('_');
                string text = null;
                if (infoes[0] == Id.ToString())
                {
                    foreach(var ch_name in ChallangeNames)
                    {
                        if (ch_name.Id == Int32.Parse(infoes[1]))
                        {
                           text=ch_name.Text; break;
                        }
                    }            
                    var post = new Post { username = User.UserName, challengename = text, date = infoes[2] + "/" + infoes[3] + "/" + infoes[4], image = $"{ForrealService.WwwRoot}/Images/{name}" };
                    Posts.Add(post);

                }
            }
            
        }

    }
}
