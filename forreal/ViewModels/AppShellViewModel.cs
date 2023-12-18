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
        private bool _isVisible { get; set; }
        public bool Visible { get => _isVisible; set { if (_isVisible != value) _isVisible = value; OnPropertyChange(); } }
        public AppShellViewModel()
        {
            //if (AppShell.Current?.CurrentPage==HomePage)
            //{

            //}
        }

    }
}
