using forreal.ViewModels;

namespace forreal
{
    public partial class App : Application
    {
        private bool showFlyout;
        private AppShellViewModel shellViewModel;
        public bool ShowFlyouts { get => showFlyout; set { showFlyout = value; shellViewModel.Visible = value; } }
        public bool ShowFlyouts2 { get { if (showFlyout)return false; return true; } set { shellViewModel.AntiVisible = value; } }
        public App(AppShellViewModel vm)
        {
            InitializeComponent();
            shellViewModel = vm;

            MainPage = new AppShell(vm);
        }
    }
}