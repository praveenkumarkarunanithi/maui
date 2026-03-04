#if ANDROID
using Android.Widget;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
#elif IOS || MACCATALYST
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
#elif WINDOWS
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using WinGrid = Microsoft.UI.Xaml.Controls.Grid;
#endif

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 34310, "Loaded event not called for MAUI View added to native View", PlatformAffected.All)]
public class Issue34310 : ContentPage
{
	bool _gridLoaded;
	bool _labelLoaded;
	bool _buttonLoaded;
	Label _statusLabel;

	public Issue34310()
	{
		_statusLabel = new Label
		{
			AutomationId = "LoadedStatus",
			Text = "Grid=False,Label=False,Button=False",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		// Build the inner MAUI view that will be hosted inside the native container
		var innerGrid = new Grid
		{
			BackgroundColor = Colors.LightSkyBlue,
			HeightRequest = 200,
			WidthRequest = 300
		};

		var innerLabel = new Label
		{
			Text = "Inner Label",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		var innerButton = new Microsoft.Maui.Controls.Button
		{
			Text = "Inner Button",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.End
		};

		innerGrid.Children.Add(innerLabel);
		innerGrid.Children.Add(innerButton);

		innerGrid.Loaded += (s, e) =>
		{
			_gridLoaded = true;
			UpdateStatus();
		};

		innerLabel.Loaded += (s, e) =>
		{
			_labelLoaded = true;
			UpdateStatus();
		};

		innerButton.Loaded += (s, e) =>
		{
			_buttonLoaded = true;
			UpdateStatus();
		};

		// Wrap in a NativeHostView34310 so it goes through the platform native handler
		var nativeHost = new NativeHostView34310
		{
			HostedView = innerGrid,
			HeightRequest = 200,
			WidthRequest = 300,
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		Content = new VerticalStackLayout
		{
			Spacing = 20,
			Padding = new Thickness(20),
			VerticalOptions = LayoutOptions.Center,
			Children =
			{
				new Label
				{
					Text = "Issue 34310 – Loaded event for MAUI View in native container",
					HorizontalOptions = LayoutOptions.Center,
					HorizontalTextAlignment = TextAlignment.Center
				},
				nativeHost,
				_statusLabel
			}
		};

		// Refresh status after a 2-second delay to allow events to propagate
		Loaded += async (s, e) =>
		{
			await Task.Delay(2000);
			UpdateStatus();
		};
	}

	void UpdateStatus()
	{
		_statusLabel.Text = $"Grid={_gridLoaded},Label={_labelLoaded},Button={_buttonLoaded}";
	}
}

// ─────────────────────────────────────────────────
// Cross-platform MAUI view that hosts another view
// ─────────────────────────────────────────────────
public class NativeHostView34310 : Microsoft.Maui.Controls.ContentView
{
	public static readonly BindableProperty HostedViewProperty =
		BindableProperty.Create(
			nameof(HostedView),
			typeof(View),
			typeof(NativeHostView34310),
			null,
			propertyChanged: OnHostedViewChanged);

	public View HostedView
	{
		get => (View)GetValue(HostedViewProperty);
		set => SetValue(HostedViewProperty, value);
	}

	static void OnHostedViewChanged(BindableObject bindable, object oldValue, object newValue)
	{
#if !ANDROID && !WINDOWS
		// On iOS/MacCatalyst the normal MAUI content pipeline fires Loaded correctly,
		// so we simply set Content and let the framework do the rest.
		if (bindable is NativeHostView34310 host)
			host.Content = newValue as View;
#endif
	}
}

// ─────────────────────────────────────────────────
// Platform-specific handlers
// ─────────────────────────────────────────────────
#if ANDROID
public class NativeHostViewHandler34310 : ViewHandler<NativeHostView34310, FrameLayout>
{
	public NativeHostViewHandler34310()
		: base(ViewHandler.ViewMapper, ViewHandler.ViewCommandMapper)
	{
	}

	protected override FrameLayout CreatePlatformView() =>
		new FrameLayout(Context);

	protected override void ConnectHandler(FrameLayout platformView)
	{
		base.ConnectHandler(platformView);
		UpdateHostedView();
	}

	void UpdateHostedView()
	{
		if (VirtualView?.HostedView is View hosted && MauiContext is not null)
		{
			PlatformView.RemoveAllViews();
			var native = hosted.ToPlatform(MauiContext);
			PlatformView.AddView(native);
		}
	}
}
#elif WINDOWS
public class NativeHostViewHandler34310 : ViewHandler<NativeHostView34310, WinGrid>
{
	public NativeHostViewHandler34310()
		: base(ViewHandler.ViewMapper, ViewHandler.ViewCommandMapper)
	{
	}

	protected override WinGrid CreatePlatformView() => new WinGrid();

	protected override void ConnectHandler(WinGrid platformView)
	{
		base.ConnectHandler(platformView);
		UpdateHostedView();
	}

	void UpdateHostedView()
	{
		if (VirtualView?.HostedView is View hosted && MauiContext is not null)
		{
			PlatformView.Children.Clear();
			var fe = hosted.ToPlatform(MauiContext);
			PlatformView.Children.Add(fe);
		}
	}
}
#endif

// ─────────────────────────────────────────────────
// Extension method for handler registration
// ─────────────────────────────────────────────────
public static class Issue34310Extensions
{
	public static MauiAppBuilder Issue34310AddMappers(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
#if ANDROID || WINDOWS
			handlers.AddHandler<NativeHostView34310, NativeHostViewHandler34310>();
#endif
		});
		return builder;
	}
}
