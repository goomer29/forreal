using forreal.ViewModels;

namespace forreal
{
    public partial class App : Application
    {
        private bool showFlyout;
        private AppShellViewModel shellViewModel;
        public bool ShowFlyouts { get => showFlyout; set { showFlyout = value; shellViewModel.Visible = value; } }
        public App(AppShellViewModel vm)
        {
            InitializeComponent();
            shellViewModel = vm;

            MainPage = new AppShell(vm);
        }
    }
}