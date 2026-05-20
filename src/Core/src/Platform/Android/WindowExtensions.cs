using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;
using AColor = Android.Graphics.Color;

namespace Microsoft.Maui
{
	public static partial class WindowExtensions
	{
		internal static void UpdateTitle(this Activity platformWindow, IWindow window)
		{
			if (string.IsNullOrEmpty(window.Title))
				platformWindow.Title = ApplicationModel.AppInfo.Current.Name;
			else
				platformWindow.Title = window.Title;
		}

		internal static DisplayOrientation GetOrientation(this IWindow? window)
		{
			if (window == null)
				return DeviceDisplay.Current.MainDisplayInfo.Orientation;

			return window.Handler?.MauiContext?.GetPlatformWindow()?.Resources?.Configuration?.Orientation switch
			{
				Orientation.Landscape => DisplayOrientation.Landscape,
				Orientation.Portrait => DisplayOrientation.Portrait,
				Orientation.Square => DisplayOrientation.Portrait,
				_ => DisplayOrientation.Unknown
			};
		}

		internal static void UpdateWindowSoftInputModeAdjust(this IWindow platformView, SoftInput inputMode)
		{
			var activity = platformView?.Handler?.PlatformView as Activity ??
							platformView?.Handler?.MauiContext?.GetPlatformWindow();

			activity?
				.Window?
				.SetSoftInputMode(inputMode);
		}

		//TODO : Make it public in NET 11.
		internal static void ConfigureTranslucentSystemBars(this Window? window, Activity activity)
		{
			if (window is null)
			{
				return;
			}

			// Set appropriate system bar appearance for readability using API 30+ methods
			var windowInsetsController = WindowCompat.GetInsetsController(window, window.DecorView);
			if (windowInsetsController is not null)
			{
				// Automatically adjust icon/text colors based on app theme
				var configuration = activity.Resources?.Configuration;
				var isLightTheme = configuration is null ||
					(configuration.UiMode & UiMode.NightMask) != UiMode.NightYes;

				windowInsetsController.AppearanceLightStatusBars = isLightTheme;
				windowInsetsController.AppearanceLightNavigationBars = isLightTheme;
			}
		}

		// API 30-34 dialog windows aren't edge-to-edge by default (API 35+ are), so the host
		// activity's system-bar chrome leaks through modals. Emulate the API 35+ default to
		// keep modal presentation consistent across Android versions.
		internal static void EnableLegacyDialogEdgeToEdge(this Window window)
		{
			if (!OperatingSystem.IsAndroidVersionAtLeast(30) ||
				OperatingSystem.IsAndroidVersionAtLeast(35))
			{
				return;
			}

			WindowCompat.SetDecorFitsSystemWindows(window, false);
			window.SetStatusBarColor(AColor.Transparent);
			window.SetNavigationBarColor(AColor.Transparent);
			window.StatusBarContrastEnforced = false;
			window.NavigationBarContrastEnforced = false;
		}
	}
}
