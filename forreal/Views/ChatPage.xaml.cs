using forreal.ViewModels;

namespace forreal.Views;

public partial class ChatPage : ContentPage
{
	public ChatPage(ChatPageViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}