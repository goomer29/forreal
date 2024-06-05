using forreal.ViewModels;
using forreal.Views;

namespace forreal
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            this.BindingContext = vm;

            InitializeComponent();
           
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));


        }
    }
}