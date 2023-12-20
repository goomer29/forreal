using forreal.ViewModels;

namespace forreal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            this.BindingContext= vm;
            InitializeComponent();
        }
    }
}