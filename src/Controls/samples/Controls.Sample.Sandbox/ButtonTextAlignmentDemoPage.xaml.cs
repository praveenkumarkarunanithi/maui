using System.Diagnostics;
using Microsoft.Maui.Devices;

namespace Maui.Controls.Sample;

public partial class ButtonTextAlignmentDemoPage : ContentPage
{
	static readonly TextAlignment?[] Alignments =
	{
		null, TextAlignment.Start, TextAlignment.Center, TextAlignment.End
	};

	int _clickCount;

	public ButtonTextAlignmentDemoPage()
	{
		InitializeComponent();

		HorizontalPicker.ItemsSource = new[] { "Native default (unset)", "Start", "Center", "End" };
		VerticalPicker.ItemsSource = new[] { "Native default (unset)", "Start (top)", "Center", "End (bottom)" };
		ImagePositionPicker.ItemsSource = new[]
		{
			Button.ButtonContentLayout.ImagePosition.Left,
			Button.ButtonContentLayout.ImagePosition.Top,
			Button.ButtonContentLayout.ImagePosition.Right,
			Button.ButtonContentLayout.ImagePosition.Bottom
		};
		HorizontalPicker.SelectedIndex = VerticalPicker.SelectedIndex = 1;
		ImagePositionPicker.SelectedIndex = 0;

		TextImageButton.ImageSource = CreateIcon();
		ImageOnlyReference.ImageSource = CreateIcon();
		ImageOnlyButton.ImageSource = CreateIcon();

		HorizontalPicker.SelectedIndexChanged += OnSettingsChanged;
		VerticalPicker.SelectedIndexChanged += OnSettingsChanged;
		ImagePositionPicker.SelectedIndexChanged += OnSettingsChanged;
		RtlSwitch.Toggled += OnSettingsChanged;
		MultilineSwitch.Toggled += OnSettingsChanged;

		UpdatePreviews();
	}

	static FileImageSource CreateIcon() => new()
	{
		File = "shopping_cart.png"
	};

	void OnSettingsChanged(object? sender, EventArgs e) => UpdatePreviews();

	void UpdatePreviews()
	{
		if (HorizontalPicker.SelectedIndex < 0 || VerticalPicker.SelectedIndex < 0 ||
			ImagePositionPicker.SelectedItem is not Button.ButtonContentLayout.ImagePosition position)
			return;

		var horizontal = Alignments[HorizontalPicker.SelectedIndex];
		var vertical = Alignments[VerticalPicker.SelectedIndex];
		var direction = RtlSwitch.IsToggled ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
		var text = MultilineSwitch.IsToggled ? "Save changes\nTo current document" : "Save changes";

		TextOnlyButton.Text = TextImageButton.Text = text;
		TextImageButton.ContentLayout = new Button.ButtonContentLayout(position, 8);
		ImageOnlyReference.FlowDirection = direction;
		ApplyAlignment(TextOnlyButton, horizontal, vertical, direction);
		ApplyAlignment(TextImageButton, horizontal, vertical, direction);
		ApplyAlignment(ImageOnlyButton, horizontal, vertical, direction);

		AlignmentSummary.Text = $"H: {horizontal?.ToString() ?? "Native default"} | " +
			$"V: {vertical?.ToString() ?? "Native default"} | {(RtlSwitch.IsToggled ? "RTL" : "LTR")}";
		ImageOnlySummary.Text = $"Right button property values: H={ImageOnlyButton.HorizontalTextAlignment}, " +
			$"V={ImageOnlyButton.VerticalTextAlignment}. No text is present, so alignment is not applied.";

		Debug.WriteLine($"SANDBOX: ButtonAlignmentDemo [{DeviceInfo.Current.Platform}] {AlignmentSummary.Text}; " +
			$"imagePosition={position}; multiline={MultilineSwitch.IsToggled}; " +
			"previews=DemoTextOnly,DemoTextImage,DemoImageOnly; image-only reference remains unset.");
	}

	static void ApplyAlignment(Button button, TextAlignment? horizontal, TextAlignment? vertical, FlowDirection direction)
	{
		button.FlowDirection = direction;
		if (horizontal is { } h)
			button.HorizontalTextAlignment = h;
		else
			button.ClearValue(Button.HorizontalTextAlignmentProperty);

		if (vertical is { } v)
			button.VerticalTextAlignment = v;
		else
			button.ClearValue(Button.VerticalTextAlignmentProperty);
	}

	void OnRestoreNative(object? sender, EventArgs e)
	{
		HorizontalPicker.SelectedIndex = VerticalPicker.SelectedIndex = 0;
	}

	void OnDemoClicked(object? sender, EventArgs e)
	{
		if (sender is Button button)
		{
			ClickStatus.Text = $"Clicks: {++_clickCount} | Last: {button.AutomationId}";
			Debug.WriteLine($"SANDBOX: ButtonAlignmentDemo [{DeviceInfo.Current.Platform}] {ClickStatus.Text}");
		}
	}

	async void OnOpenVerification(object? sender, EventArgs e) =>
		await Navigation.PushAsync(new ButtonTextAlignmentPage());
}
