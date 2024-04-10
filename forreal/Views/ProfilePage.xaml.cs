
using forreal.ViewModels;
namespace forreal.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfilePageViewModel vm)
	{
        this.BindingContext = vm;
        InitializeComponent();
    }
}