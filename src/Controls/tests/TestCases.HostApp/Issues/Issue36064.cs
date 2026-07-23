using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 36064, "WebView does not measure and size correctly across platforms", PlatformAffected.iOS | PlatformAffected.UWP | PlatformAffected.Android | PlatformAffected.macOS)]
public class Issue36064 : ContentPage
{
	public Issue36064()
	{
		Title = "WebView Sizing Demo - Chatbot UI";

		var header = new VerticalStackLayout
		{
			Padding = new Thickness(16, 12),
			BackgroundColor = Color.FromArgb("#1976D2"),
			Spacing = 4,
			Children =
			{
				new Label { Text = "WebView Sizing Issue Demo", TextColor = Colors.White, FontSize = 18, FontAttributes = FontAttributes.Bold },
				new Label { Text = "Chat interface with HTML content rendering", TextColor = Color.FromArgb("#E0E0E0"), FontSize = 12 },
			}
		};

		var collectionView = new CollectionView
		{
			AutomationId = "Issue36064CollectionView",
			ItemsSource = BuildMessages(),
			SelectionMode = SelectionMode.None,
			ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 12 },
			ItemTemplate = new DataTemplate(BuildBubbleTemplate),
		};

		var footer = new VerticalStackLayout
		{
			Padding = 16,
			BackgroundColor = Color.FromArgb("#F5F5F5"),
			Spacing = 8,
			Children =
			{
				new Label { Text = "⚠️ WebView Sizing Behavior:", FontAttributes = FontAttributes.Bold, FontSize = 12 },
				new Label { Text = "• WebView may not calculate correct height from HTML content\n• Content may appear clipped or have extra space\n• This is independent of CollectionView – the layout container", FontSize = 11, LineBreakMode = LineBreakMode.WordWrap },
			}
		};

		Content = new Grid
		{
			RowDefinitions =
			[
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
				new RowDefinition { Height = GridLength.Auto },
			],
			Children = { header.Row(0), collectionView.Row(1), footer.Row(2) },
		};
	}

	static View BuildBubbleTemplate()
	{
		var webView = new WebView { VerticalOptions = LayoutOptions.Fill };
		webView.SetBinding(WebView.SourceProperty, new Binding(nameof(ChatMessage.HtmlContent), converter: new HtmlSourceConverter()));
		webView.SetBinding(View.HorizontalOptionsProperty, new Binding(nameof(ChatMessage.WebViewAlignment)));
		webView.SetBinding(Element.AutomationIdProperty, new Binding(nameof(ChatMessage.AutomationId)));

		var bubble = new Grid
		{
			Padding = 12,
			Children = { webView },
		};
		bubble.SetBinding(View.BackgroundColorProperty, new Binding(nameof(ChatMessage.BubbleColor)));
		bubble.SetBinding(View.HorizontalOptionsProperty, new Binding(nameof(ChatMessage.BubbleAlignment)));

		return new Grid
		{
			Padding = new Thickness(12, 0),
			HorizontalOptions = LayoutOptions.Fill,
			Children = { bubble },
		};
	}

	static ObservableCollection<ChatMessage> BuildMessages() => new()
	{
		new(1, true,  Html("<p>Hello! How are you?</p>")),
		new(2, false, Html("<p>I'm doing great, thanks for asking!<br/>This is a multi-line response.<br/>It contains multiple lines of text.</p>")),
		new(3, true,  Html("<p>Can you help me understand WebView sizing issues?<br/>I'm trying to render dynamic HTML content.</p>")),
		new(4, false, Html(
			"<p><strong>Absolutely!</strong> WebView sizing in MAUI can be tricky.</p>"
			+ "<p>Common issues include:</p>"
			+ "<p>• Incorrect height calculation<br/>• Clipped content<br/>• Extra empty space<br/>• Layout not expanding as expected</p>",
			"strong { color: #1976d2; } p { margin: 0 0 6px 0; line-height: 1.4; }")),
		new(5, true,  Html("<p>Why does this happen?</p>")),
		new(6, false, Html(
			"<p><strong>Several factors contribute:</strong></p>"
			+ "<div style='margin-left:8px'>"
			+ "<p><strong>1. Layout Measurement:</strong> WebView must report its desired size to the parent layout, but HTML content size is not always known immediately.</p>"
			+ "<p><strong>2. Async Rendering:</strong> WebView rendering happens asynchronously. The layout is calculated before content is fully rendered.</p>"
			+ "<p><strong>3. Platform Differences:</strong> Android, iOS, and Windows handle WebView measurement differently.</p>"
			+ "</div>",
			"strong { color: #1976d2; } p { margin: 0 0 6px 0; line-height: 1.4; }")),
	};

	// [#36064 — sample-side workaround] WKWebView on iOS defaults to a 980-px desktop
	// viewport when the HTML lacks a viewport meta tag, which makes fragments render tiny
	// and produces a scrollHeight that doesn't match the on-screen frame. This is
	// documented WKWebView behavior — the standard fix, per the MAUI team's guidance on
	// issue #32603, is for the HTML author to include a viewport meta. Android and Windows
	// default to device-width without needing this, so the meta only affects iOS.
	static string Html(string body, string extraCss = "p { margin: 0; line-height: 1.4; }") =>
		"<html><head>"
		+ "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"
		+ "<style>body{font-family:Arial,sans-serif;margin:0;padding:8px;font-size:14px;color:#333;}"
		+ extraCss + "</style></head><body>" + body + "</body></html>";

	class ChatMessage
	{
		public ChatMessage(int id, bool isUser, string html)
		{
			Id = id;
			IsUser = isUser;
			HtmlContent = html;
			AutomationId = $"Issue36064WebView{id}";
		}

		public int Id { get; }
		public bool IsUser { get; }
		public string HtmlContent { get; }
		public string AutomationId { get; }
		public Color BubbleColor => IsUser ? Color.FromArgb("#E3F2FD") : Color.FromArgb("#F5F5F5");
		public LayoutOptions BubbleAlignment => IsUser ? LayoutOptions.End : LayoutOptions.Start;
		public LayoutOptions WebViewAlignment => IsUser ? LayoutOptions.End : LayoutOptions.Start;
	}

	class HtmlSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is string s ? new HtmlWebViewSource { Html = s } : null;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotSupportedException();
	}
}
