using forreal.ViewModels;
using Microsoft.Maui.Controls;

namespace forreal.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call the PageAppearingCommand when the page appears
        if (BindingContext is HomePageViewModel viewModel && viewModel.PageAppearingCommand.CanExecute(null))
        {
            viewModel.PageAppearingCommand.Execute(null);
        }
    }
}