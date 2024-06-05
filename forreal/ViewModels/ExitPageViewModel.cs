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
        private IServiceProvider provider;
        public ICommand ExitCommand {  get; protected set; }
        public ExitPageViewModel(IServiceProvider provider)
        {
            this.provider = provider;
            ExitCommand = new Command(async () =>
            {
                ((App)Application.Current).User = null;
                SearchPageViewModel.SearchPageLogOut = true;
                ProfilePageViewModel.ProfilePageLogOut = true;
               ((App)Application.Current).IsLogIn = false;               
                App.Current.MainPage=provider.GetService<LoginPage>();
                
            });
        }
   }
}
