using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using forreal.ViewModels;
using forreal.Views;
using System.Threading.Tasks;
using System.Windows.Input;

namespace forreal.ViewModels
{
    
   public class ExitPageViewModel:ViewModel
    {
        public ICommand ExitCommand {  get; protected set; }
        public ExitPageViewModel()
        {       
            ExitCommand = new Command(async () =>
            {
                ((App)Application.Current).IsLogIn = false;               
                await AppShell.Current.GoToAsync("MainPage");
                
            });
        }
    }
}
