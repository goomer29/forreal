using forreal.ViewModels;

namespace forreal.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}