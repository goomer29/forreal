using CommunityToolkit.Maui.Views;
using forreal.ViewModels;

namespace forreal.Views;

public partial class ChatPage : Popup
{
    public static ChatPage Instance;

    public ChatPage(ChatPageViewModel vm)
	{
        this.BindingContext = vm;
        InitializeComponent();
        Instance = this;
    }
    public async static Task ClosePopup()
    {
        await Instance.CloseAsync();
    }
    public async static Task CloseInstance()
    {
        await Instance.CloseAsync();
        Instance = null;
    }
}