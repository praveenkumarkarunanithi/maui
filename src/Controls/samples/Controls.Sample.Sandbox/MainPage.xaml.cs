using Maui.Controls.Sample.ViewModels;

namespace Maui.Controls.Sample;

/// <summary>
/// MainPage demonstrates WebView sizing behavior in a chat-like interface.
///
/// This page showcases how WebView renders HTML content and highlights
/// the sizing inconsistencies that can occur.
/// </summary>
public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		// Set the ViewModel as the BindingContext
		BindingContext = new ChatViewModel();
	}

	private void UserWebView_SizeChanged(object sender, EventArgs e)
	{

	}
}