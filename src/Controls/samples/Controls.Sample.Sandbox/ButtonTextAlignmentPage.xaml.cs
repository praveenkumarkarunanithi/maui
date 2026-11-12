using System.Diagnostics;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample;

public partial class ButtonTextAlignmentPage : ContentPage
{
	static readonly TextAlignment?[] AlignmentCycle = { null, TextAlignment.Start, TextAlignment.Center, TextAlignment.End };

	int _horizontalIndex;
	int _verticalIndex;
	int _clickCount;

	public ButtonTextAlignmentPage()
	{
		InitializeComponent();

		var defaultButton = new Button();
		Case1Values.Text = $"Actual getter values: H={defaultButton.HorizontalTextAlignment}, V={defaultButton.VerticalTextAlignment} (must be Center, Center)";

		ImageOnlyButton.ImageSource = CreateIcon();

		foreach (var position in new[]
		{
			Button.ButtonContentLayout.ImagePosition.Left,
			Button.ButtonContentLayout.ImagePosition.Top,
			Button.ButtonContentLayout.ImagePosition.Right,
			Button.ButtonContentLayout.ImagePosition.Bottom
		})
		{
			ImageCasesLayout.Children.Add(new Button
			{
				Style = (Style)Resources["CaseButton"],
				WidthRequest = 170,
				HeightRequest = 120,
				Margin = new Thickness(0, 0, 6, 6),
				Text = $"Img {position}",
				AutomationId = $"Case4Image{position}",
				ImageSource = CreateIcon(),
				ContentLayout = new Button.ButtonContentLayout(position, 6),
				HorizontalTextAlignment = TextAlignment.End,
				VerticalTextAlignment = TextAlignment.End
			});
		}

		UpdateRuntimeCase();
		ObserveButtons(this);
		Loaded += (_, _) =>
		{
			Log("Page loaded. Native snapshots are observations, not visual PASS results.");
			LogExpectations(this);
		};
	}

	static void Log(string message) =>
		Debug.WriteLine($"SANDBOX: ButtonAlignment [{DeviceInfo.Current.Platform}] {message}");

	void ObserveButtons(IVisualTreeElement element)
	{
		foreach (var child in element.GetVisualChildren())
		{
			if (child is Button button)
			{
				button.Loaded += (_, _) => LogButton(button, "Loaded");
				button.SizeChanged += (_, _) => LogButton(button, "SizeChanged");
				button.Clicked += (_, _) => LogButton(button, "Clicked");
			}

			ObserveButtons(child);
		}
	}

	static void LogExpectations(IVisualTreeElement element)
	{
		foreach (var child in element.GetVisualChildren())
		{
			if (child is Label label && label.Text?.StartsWith("EXPECTED:", StringComparison.Ordinal) == true)
				Log(label.Text);

			LogExpectations(child);
		}
	}

	static void LogButton(Button button, string reason)
	{
		var scenario = button.AutomationId switch
		{
			"RuntimeButton" or "CycleH" or "CycleV" or "ClearBoth" or "ToggleText" or "ToggleImage" or "ToggleRtl" => $"Case8/{button.AutomationId}",
			"TryJustify" => "Case9/Justify",
			"ClickableButton" => "Case10/ClickableButton",
			_ => button.AutomationId ?? "(unidentified)"
		};
		void Write(string message) => Log($"[{scenario}] {reason}: {message}");

		var text = button.Text?.Replace("\r", "\\r", StringComparison.Ordinal).Replace("\n", "\\n", StringComparison.Ordinal) ?? "(null)";
		Write($"id={button.AutomationId ?? "(none)"}, text=\"{text}\", " +
			$"H={button.HorizontalTextAlignment} (set={button.IsSet(Button.HorizontalTextAlignmentProperty)}), " +
			$"V={button.VerticalTextAlignment} (set={button.IsSet(Button.VerticalTextAlignmentProperty)}), " +
			$"flow={button.FlowDirection}, padding={button.Padding}, bounds={button.Bounds}, " +
			$"image={button.ImageSource is not null}, imagePosition={button.ContentLayout.Position}, " +
			$"spacing={button.ContentLayout.Spacing}, lineBreak={button.LineBreakMode}, enabled={button.IsEnabled}");

		// Native bounds use platform units; compare positions within a platform, not across platforms.
#if ANDROID
		if (button.Handler?.PlatformView is global::Android.Widget.Button native)
		{
			Write($"Native Android: gravity={native.Gravity}, textAlignment={native.TextAlignment}, " +
				$"layoutDirection={native.LayoutDirection}, size={native.Width}x{native.Height}, " +
				$"padding=({native.PaddingLeft},{native.PaddingTop},{native.PaddingRight},{native.PaddingBottom})");
			if (native.Layout is { } layout && layout.LineCount > 0)
				Write($"Native Android text: lines={layout.LineCount}, " +
					$"firstLine=({layout.GetLineLeft(0)},{layout.GetLineTop(0)},{layout.GetLineRight(0)},{layout.GetLineBottom(0)}), " +
					$"layoutSize={layout.Width}x{layout.Height}; line coordinates are relative to the text layout.");
		}
#elif IOS || MACCATALYST
		if (button.Handler?.PlatformView is UIKit.UIButton native)
		{
			Write($"Native Apple: H={native.HorizontalAlignment}, V={native.VerticalAlignment}, " +
				$"flow={native.EffectiveUserInterfaceLayoutDirection}, bounds={native.Bounds}, " +
				$"titleAlignment={native.TitleLabel?.TextAlignment}, titleFrame={native.TitleLabel?.Frame}, " +
				$"imageFrame={native.ImageView?.Frame}, hasImage={native.CurrentImage is not null}");
		}
#elif WINDOWS
		if (button.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.Button native)
		{
			Write($"Native Windows: H={native.HorizontalContentAlignment}, V={native.VerticalContentAlignment}, " +
				$"flow={native.FlowDirection}, size={native.ActualWidth}x{native.ActualHeight}, padding={native.Padding}");
			LogWindowsContent(native, native, Write);
		}
#endif
	}

#if WINDOWS
	static void LogWindowsContent(Microsoft.UI.Xaml.DependencyObject element, Microsoft.UI.Xaml.Controls.Button button, Action<string> write)
	{
		for (var i = 0; i < Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(element); i++)
		{
			var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(element, i);
			if (child is Microsoft.UI.Xaml.FrameworkElement view &&
				(child is Microsoft.UI.Xaml.Controls.TextBlock || child is Microsoft.UI.Xaml.Controls.Image))
			{
				var origin = view.TransformToVisual(button).TransformPoint(new global::Windows.Foundation.Point());
				var alignment = child is Microsoft.UI.Xaml.Controls.TextBlock text ? text.TextAlignment.ToString() : "(image)";
				write($"Native Windows {view.GetType().Name}: origin=({origin.X},{origin.Y}), " +
					$"size={view.ActualWidth}x{view.ActualHeight}, textAlignment={alignment}");
			}

			LogWindowsContent(child, button, write);
		}
	}
#endif

	static FontImageSource CreateIcon() => new() { Glyph = "+", Size = 24, Color = Colors.White };

	void OnCycleHorizontal(object? sender, EventArgs e)
	{
		_horizontalIndex = (_horizontalIndex + 1) % AlignmentCycle.Length;
		UpdateRuntimeCase();
	}

	void OnCycleVertical(object? sender, EventArgs e)
	{
		_verticalIndex = (_verticalIndex + 1) % AlignmentCycle.Length;
		UpdateRuntimeCase();
	}

	void OnClearAlignment(object? sender, EventArgs e)
	{
		_horizontalIndex = _verticalIndex = 0;
		UpdateRuntimeCase();
	}

	void OnToggleText(object? sender, EventArgs e)
	{
		RuntimeButton.Text = string.IsNullOrEmpty(RuntimeButton.Text) ? "Runtime" : null;
		UpdateRuntimeCase();
	}

	void OnToggleImage(object? sender, EventArgs e)
	{
		RuntimeButton.ImageSource = RuntimeButton.ImageSource is null ? CreateIcon() : null;
		UpdateRuntimeCase();
	}

	void OnToggleRtl(object? sender, EventArgs e)
	{
		RuntimeButton.FlowDirection = RuntimeButton.FlowDirection == FlowDirection.RightToLeft
			? FlowDirection.LeftToRight
			: FlowDirection.RightToLeft;
		UpdateRuntimeCase();
	}

	void UpdateRuntimeCase()
	{
		var horizontal = AlignmentCycle[_horizontalIndex];
		var vertical = AlignmentCycle[_verticalIndex];

		// ClearValue (not SetValue(Center)) is what "not set" means: native default rendering is restored.
		if (horizontal is { } h)
			RuntimeButton.HorizontalTextAlignment = h;
		else
			RuntimeButton.ClearValue(Button.HorizontalTextAlignmentProperty);

		if (vertical is { } v)
			RuntimeButton.VerticalTextAlignment = v;
		else
			RuntimeButton.ClearValue(Button.VerticalTextAlignmentProperty);

		var hasText = !string.IsNullOrEmpty(RuntimeButton.Text);
		var hasImage = RuntimeButton.ImageSource is not null;
		var rtl = RuntimeButton.FlowDirection == FlowDirection.RightToLeft;

		RuntimeState.Text = $"H={horizontal?.ToString() ?? "(not set)"}, V={vertical?.ToString() ?? "(not set)"}, " +
			$"text={(hasText ? "yes" : "no")}, image={(hasImage ? "yes" : "no")}, flow={(rtl ? "RTL" : "LTR")}";

		string content = hasText && hasImage ? "Image + text group" : hasText ? "Text" : hasImage ? "Image" : "Nothing";
		if (!hasText)
		{
			RuntimeExpected.Text = hasImage
				? "EXPECTED: The image is CENTERED. Alignment is ignored without text."
				: "EXPECTED: Empty Button (nothing to align).";
		}
		else
		{
			RuntimeExpected.Text = $"EXPECTED: {content} is {DescribeVertical(vertical)} and {DescribeHorizontal(horizontal, rtl)}." +
				(horizontal is null && vertical is null ? " (Same as a Button before this feature.)" : string.Empty);
		}

		Log($"[Case8/RuntimeButton] Runtime requested: {RuntimeState.Text} -> {RuntimeExpected.Text}");
		// Observe after synchronous mapper updates; Loaded/SizeChanged also capture initial layout.
		Dispatcher.Dispatch(() => LogButton(RuntimeButton, "Runtime dispatched snapshot"));
	}

	static string DescribeHorizontal(TextAlignment? alignment, bool rtl) => alignment switch
	{
		TextAlignment.Start => rtl ? "on the RIGHT (Start mirrors in RTL)" : "on the LEFT",
		TextAlignment.End => rtl ? "on the LEFT (End mirrors in RTL)" : "on the RIGHT",
		TextAlignment.Center => "horizontally CENTERED",
		_ => "horizontally centered (native default)"
	};

	static string DescribeVertical(TextAlignment? alignment) => alignment switch
	{
		TextAlignment.Start => "at the TOP",
		TextAlignment.End => "at the BOTTOM",
		TextAlignment.Center => "vertically CENTERED",
		_ => "vertically centered (native default)"
	};

	void OnTryJustify(object? sender, EventArgs e)
	{
		var probe = new Button { Text = "probe" };
		try
		{
			probe.HorizontalTextAlignment = TextAlignment.Justify;
			JustifyResult.Text = $"FAIL: no exception, value is now {probe.HorizontalTextAlignment}";
		}
		catch (ArgumentException)
		{
			JustifyResult.Text = $"PASS: ArgumentException thrown; value is still {probe.HorizontalTextAlignment}";
		}

		Log($"[Case9/Justify] Validation: {JustifyResult.Text}");
	}

	void OnClickableClicked(object? sender, EventArgs e)
	{
		ClickCount.Text = $"Taps: {++_clickCount}";
		Log($"[Case10/ClickableButton] {ClickCount.Text}");
	}
}
