using forreal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class ChatPageViewModel:ViewModel
    {
        public Post PostSelect { get => HomePageViewModel.PostSelecting; }
    }
}
