using forreal.ViewModels;

namespace forreal.Views;

public partial class ExitPage : ContentPage
{
	public ExitPage(ExitPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}