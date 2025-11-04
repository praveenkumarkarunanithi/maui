namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 32279, "TapGestureRecognizer not working on ContentView without background on Windows", PlatformAffected.UWP)]
public class Issue32279 : ContentPage
{
	public Issue32279()
	{
		var resultLabel = new Label
		{
			AutomationId = "Result",
			Text = "Not tapped yet",
			Margin = new Thickness(10)
		};

		var contentView = new ContentView
		{
			AutomationId = "TestContentView",
			HeightRequest = 100,
			WidthRequest = 200,
			Content = new Label
			{
				Text = "Tap me (no background)",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			}
		};

		var tapGesture = new TapGestureRecognizer();
		tapGesture.Tapped += (s, e) =>
		{
			resultLabel.Text = "Tapped!";
		};
		contentView.GestureRecognizers.Add(tapGesture);

		Content = new VerticalStackLayout
		{
			Padding = 10,
			Spacing = 10,
			Children = { contentView, resultLabel }
		};
	}
}
