
using forreal.ViewModels;
namespace forreal.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call the PageAppearingCommand when the page appears
        if (BindingContext is ProfilePageViewModel viewModel && viewModel.PageAppearingCommand.CanExecute(null))
        {
            viewModel.PageAppearingCommand.Execute(null);
        }
    }
}