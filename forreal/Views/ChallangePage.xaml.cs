using CommunityToolkit.Maui.Views;
using forreal.ViewModels;
using System.Runtime.CompilerServices;

namespace forreal.Views;

public partial class ChallangePage : Popup
{
	public static ChallangePage Instance;

	public ChallangePage(ChallangePageViewModel vm)
	{
		this.BindingContext=vm;
		InitializeComponent();
		Instance = this;
	}

	public async static Task CloseInstance()
	{
		await Instance.CloseAsync();
		Instance = null;
	}
}