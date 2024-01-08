using forreal.ViewModels;

namespace forreal.Views;

public partial class ChallangePage : ContentPage
{
	public ChallangePage(ChallangePageViewModel vm)
	{
		this.BindingContext=vm;
		InitializeComponent();
	}
}