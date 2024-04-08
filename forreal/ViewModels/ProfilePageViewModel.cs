using forreal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class ProfilePageViewMode:ViewModel
    {
        public ObservableCollection<Post> _posts { get; set; }
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
    }
}
