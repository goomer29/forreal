
using forreal.ViewModels;

namespace forreal.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }
}