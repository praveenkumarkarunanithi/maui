#if ANDROID
using AndroidX.Core.View;
using AColor = Android.Graphics.Color;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 32987, "TabBar behavior inconsistent with API 36 and API 34 devices", PlatformAffected.Android)]
public class Issue32987 : ContentPage
{
	readonly Label _activityNavBarColor = new() { AutomationId = "Issue32987ActivityNavBarColorLabel", TextColor = Colors.Black };
	readonly Label _activityStatusBarColor = new() { AutomationId = "Issue32987ActivityStatusBarColorLabel", TextColor = Colors.Black };
	readonly Label _activityNavBarContrast = new() { AutomationId = "Issue32987ActivityNavBarContrastLabel", TextColor = Colors.Black };
	readonly Label _activityStatusBarContrast = new() { AutomationId = "Issue32987ActivityStatusBarContrastLabel", TextColor = Colors.Black };

	public Issue32987()
	{
		BackgroundColor = Colors.Orange;
		Content = new VerticalStackLayout
		{
			Padding = 20,
			Spacing = 8,
			Children =
			{
				_activityNavBarColor,
				_activityStatusBarColor,
				_activityNavBarContrast,
				_activityStatusBarContrast,
			},
		};
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		CaptureActivityWindowProperties();
	}

	void CaptureActivityWindowProperties()
	{
		var window = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity?.Window
			?? throw new InvalidOperationException("Issue32987: CurrentActivity.Window was null.");

		// Reproduce the bug scenario: edge-to-edge layout + transparent system bars.
		// Without the fix the OS draws a translucent contrast scrim over the page color.
		if (!OperatingSystem.IsAndroidVersionAtLeast(35))
		{
			WindowCompat.SetDecorFitsSystemWindows(window, false);
			window.SetNavigationBarColor(AColor.Transparent);
			window.SetStatusBarColor(AColor.Transparent);
			_activityNavBarColor.Text = $"ActivityNavBar=0x{window.NavigationBarColor:X8}";
			_activityStatusBarColor.Text = $"ActivityStatusBar=0x{window.StatusBarColor:X8}";

			if (OperatingSystem.IsAndroidVersionAtLeast(29))
			{
				_activityNavBarContrast.Text = $"ActivityNavBarContrast={window.NavigationBarContrastEnforced}";
				_activityStatusBarContrast.Text = $"ActivityStatusBarContrast={window.StatusBarContrastEnforced}";
			}
		}
	}
}
#endif
