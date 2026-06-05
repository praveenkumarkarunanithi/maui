namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 35771, "Android SIGSEGV crash with multiple auto-sizing WebViews in ScrollView on navigated page", PlatformAffected.Android)]
public class Issue35771 : NavigationPage
{
	public Issue35771() : base(new Issue35771HomePage()) { }

	class Issue35771HomePage : ContentPage
	{
		public Issue35771HomePage()
		{
			Content = new VerticalStackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Children =
				{
					new Button
					{
						AutomationId = "Issue35771NavigateButton",
						Text = "Open repro page",
						Command = new Command(async () => await Navigation.PushAsync(new Issue35771ReproPage()))
					}
				}
			};
		}
	}

	class Issue35771ReproPage : ContentPage
	{
		public Issue35771ReproPage()
		{
			var stack = new VerticalStackLayout
			{
				Children =
				{
					new Label
					{
						AutomationId = "Issue35771Ready",
						Text = "Page loaded — no crash"
					}
				}
			};

			for (int i = 1; i <= 6; i++)
			{
				stack.Children.Add(new WebView
				{
					Source = new HtmlWebViewSource { Html = $"<html><body><p>WebView {i}</p></body></html>" }
				});
			}

			Content = new ScrollView { Content = stack };
		}
	}
}
