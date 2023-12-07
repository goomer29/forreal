using forreal.Views;

namespace forreal
{
    public partial class AppShell : Shell
    {
        private bool _isVisible  { get; set; }
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("SignUp", typeof(SignUpPage));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
            BindingContext = this;
            IsVisible = false;
        }
        public bool Visible{ get=>_isVisible; set { if(_isVisible!=value) _isVisible = value; OnPropertyChanged(); } }
    }
}