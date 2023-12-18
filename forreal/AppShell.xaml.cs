using forreal.ViewModels;
using forreal.Views;

namespace forreal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            this.BindingContext = new AppShellViewModel();
            InitializeComponent();
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
        }
    }
}