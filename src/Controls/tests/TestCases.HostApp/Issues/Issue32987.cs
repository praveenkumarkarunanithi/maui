#if ANDROID
using AndroidX.Fragment.App;
using Microsoft.Maui.Controls.Platform;
using AndroidWindow = Android.Views.Window;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 32987, "TabBar behavior inconsistent with API 36 and API 34 devices", PlatformAffected.Android)]
public class Issue32987 : ContentPage
{
	public Issue32987()
	{
		var button = new Button { Text = "Show Modal", AutomationId = "ShowModalButton" };
		button.Clicked += async (_, _) =>
		{
			var modal = new Issue32987ModalPage();
			await Navigation.PushModalAsync(modal);
			modal.CaptureModalDialogWindowProperties();
		};
		Content = new VerticalStackLayout { Padding = 20, Children = { button } };
	}
}

internal sealed class Issue32987ModalPage : ContentPage
{
	readonly Label _navBarColor = new() { AutomationId = "Issue32987NavBarColorLabel" };
	readonly Label _statusBarColor = new() { AutomationId = "Issue32987StatusBarColorLabel" };
	readonly Label _navBarContrast = new() { AutomationId = "Issue32987NavBarContrastLabel" };
	readonly Label _statusBarContrast = new() { AutomationId = "Issue32987StatusBarContrastLabel" };

	public Issue32987ModalPage()
	{
		BackgroundColor = Colors.White;
		Content = new VerticalStackLayout
		{
			Padding = 20,
			Spacing = 8,
			Children = { _navBarColor, _statusBarColor, _navBarContrast, _statusBarContrast },
		};
	}

	internal void CaptureModalDialogWindowProperties()
	{
		// Bug only exists on API 30-34 (Android 11-14). On API 35+ Android already applies
		// edge-to-edge to dialogs and the getters below are obsolete there, so this method
		// no-ops outside the affected range. The NUnit test skips itself on those API levels.
		if (!OperatingSystem.IsAndroidVersionAtLeast(30) || OperatingSystem.IsAndroidVersionAtLeast(35))
			return;

		var window = FindModalDialogWindow();
		_navBarColor.Text = $"NavBar=0x{window.NavigationBarColor:X8}";
		_statusBarColor.Text = $"StatusBar=0x{window.StatusBarColor:X8}";
		_navBarContrast.Text = $"NavBarContrast={window.NavigationBarContrastEnforced}";
		_statusBarContrast.Text = $"StatusBarContrast={window.StatusBarContrastEnforced}";
	}

	static AndroidWindow FindModalDialogWindow()
	{
		var activity = (FragmentActivity)Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!;
		foreach (var f in activity.SupportFragmentManager.Fragments)
		{
			if (f is ModalNavigationManager.ModalFragment mf && mf.Dialog?.IsShowing == true)
			{
				return mf.Dialog.Window!;
			}
		}

		throw new InvalidOperationException(
			"Issue32987: ModalFragment with a showing Dialog was not found in SupportFragmentManager.Fragments.");
	}
}
#endif
