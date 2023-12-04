using forreal.Views;

namespace forreal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
        }
    }
}