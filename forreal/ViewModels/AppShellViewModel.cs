using forreal.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class AppShellViewModel:ViewModel
    {
        private bool _isVisible;
        public bool Visible { get => _isVisible; set { if (_isVisible != value) _isVisible = value; OnPropertyChange(); } }
        public bool AntiVisible { get { if (_isVisible) return false;return true; } set { OnPropertyChange(); } }
        public AppShellViewModel()
        {
           Visible = false;
        }

    }
}
