namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 32603, "NET10 - Content in WebView on iOS too small now", PlatformAffected.iOS)]
public class Issue32603 : ContentPage
{
	// HTML without viewport meta tag — this is what triggers the bug on iOS .NET 10.
	// iOS defaults to a 980px viewport when no viewport meta is present, making
	// font-size: 36px content appear ~1.5x smaller than a native Label at FontSize 36.
	const string HtmlContent =
		"<html>" +
		"<head>" +
		"<style>html { font-size: 36px; } body { margin: 0px; }</style>" +
		"</head>" +
		"<body><p>Html Text (font-size: 36px)</p></body>" +
		"</html>";

	public Issue32603()
	{
		var statusLabel = new Label
		{
			AutomationId = "ViewportStatusLabel",
			Text = "Loading..."
		};

		var webView = new WebView
		{
			AutomationId = "TestWebView",
			HeightRequest = 80,
			Source = new HtmlWebViewSource { Html = HtmlContent }
		};

		webView.Navigated += async (s, e) =>
		{
			// Measure the WebView's CSS viewport width via JavaScript.
			// Correct behavior (device-width viewport): innerWidth ≈ 390–430px on a typical iPhone.
			// Bug (default 980px desktop viewport): innerWidth ≈ 980px, content appears scaled down.
			var result = await webView.EvaluateJavaScriptAsync("window.innerWidth.toString()");
			statusLabel.Text = int.TryParse(result, out int viewportWidth) && viewportWidth < 500
				? "ViewportCorrect"
				: "ViewportTooWide";
		};

		Content = new VerticalStackLayout
		{
			Padding = 10,
			Spacing = 10,
			Children = { webView, statusLabel }
		};
	}
}
