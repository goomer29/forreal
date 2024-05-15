using forreal.ViewModels;
using Microsoft.Maui.Controls;
namespace forreal.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call the PageAppearingCommand when the page appears
        if (BindingContext is SearchPageViewModel viewModel && viewModel.PageAppearingCommand.CanExecute(null))
        {
            viewModel.PageAppearingCommand.Execute(null);
        }
    }
}