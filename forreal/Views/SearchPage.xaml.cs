using forreal.ViewModels;

namespace forreal.Views;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel vm)
    {
        this.BindingContext = vm;
        InitializeComponent();
    }
}