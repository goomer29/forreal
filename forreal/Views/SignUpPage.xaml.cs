using forreal.ViewModels;

namespace forreal.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}