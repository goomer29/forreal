using forreal.ViewModels;

namespace forreal.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingPageViewModel vm)
    { 
            this.BindingContext = vm;
            InitializeComponent();      
    }
}