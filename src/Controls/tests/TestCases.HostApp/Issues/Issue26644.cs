namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 26644, "[iOS] Label with a fixed WidthRequest has wrong height", PlatformAffected.iOS | PlatformAffected.macOS)]
public class Issue26644 : ContentPage
{
	const string LongText =
		"This is a much longer label text string to test layout! " +
		"It must wrap to at least three lines at width 200pt.";

	readonly Label _testLabelWithBg;
	readonly Label _testLabelNoBg;
	readonly Grid _gridWithBg;
	readonly Grid _gridNoBg;

	public Issue26644()
	{
		var referenceWithBg = new Label
		{
			Text = LongText,
			AutomationId = "Issue26644ReferenceWithBg",
			FontSize = 16,
			MaximumWidthRequest = 200,
			LineBreakMode = LineBreakMode.WordWrap,
			BackgroundColor = Colors.LightYellow,
		};

		var referenceNoBg = new Label
		{
			Text = LongText,
			AutomationId = "Issue26644ReferenceNoBg",
			FontSize = 16,
			MaximumWidthRequest = 200,
			LineBreakMode = LineBreakMode.WordWrap,
		};

		_testLabelWithBg = new Label
		{
			Text = "Short",
			AutomationId = "Issue26644TestLabelWithBg",
			FontSize = 16,
			WidthRequest = 200,
			LineBreakMode = LineBreakMode.WordWrap,
			BackgroundColor = Colors.LightYellow,
		};

		_testLabelNoBg = new Label
		{
			Text = "Short",
			AutomationId = "Issue26644TestLabelNoBg",
			FontSize = 16,
			WidthRequest = 200,
			LineBreakMode = LineBreakMode.WordWrap,
		};

		_gridWithBg = new Grid
		{
			BackgroundColor = Colors.LightGray,
			ColumnDefinitions = { new ColumnDefinition(10), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(10) },
			RowDefinitions = { new RowDefinition(10), new RowDefinition(GridLength.Auto), new RowDefinition(10) },
		};
		Grid.SetRow(_testLabelWithBg, 1);
		Grid.SetColumn(_testLabelWithBg, 1);
		_gridWithBg.Add(_testLabelWithBg);

		_gridNoBg = new Grid
		{
			BackgroundColor = Colors.LightGray,
			ColumnDefinitions = { new ColumnDefinition(10), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(10) },
			RowDefinitions = { new RowDefinition(10), new RowDefinition(GridLength.Auto), new RowDefinition(10) },
		};
		Grid.SetRow(_testLabelNoBg, 1);
		Grid.SetColumn(_testLabelNoBg, 1);
		_gridNoBg.Add(_testLabelNoBg);

		var refGridWithBg = new Grid
		{
			BackgroundColor = Colors.LightGray,
			ColumnDefinitions = { new ColumnDefinition(10), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(10) },
			RowDefinitions = { new RowDefinition(10), new RowDefinition(GridLength.Auto), new RowDefinition(10) },
		};
		Grid.SetRow(referenceWithBg, 1);
		Grid.SetColumn(referenceWithBg, 1);
		refGridWithBg.Add(referenceWithBg);

		var refGridNoBg = new Grid
		{
			BackgroundColor = Colors.LightGray,
			ColumnDefinitions = { new ColumnDefinition(10), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(10) },
			RowDefinitions = { new RowDefinition(10), new RowDefinition(GridLength.Auto), new RowDefinition(10) },
		};
		Grid.SetRow(referenceNoBg, 1);
		Grid.SetColumn(referenceNoBg, 1);
		refGridNoBg.Add(referenceNoBg);

		var buttonWithBg = new Button
		{
			Text = "Update With Background",
			AutomationId = "Issue26644ButtonWithBg",
		};
		buttonWithBg.Clicked += (s, e) =>
		{
			_testLabelWithBg.Text = LongText;
			_gridWithBg.Handler?.VirtualView?.InvalidateMeasure();
		};

		var buttonNoBg = new Button
		{
			Text = "Update No Background",
			AutomationId = "Issue26644ButtonNoBg",
		};
		buttonNoBg.Clicked += (s, e) =>
		{
			_testLabelNoBg.Text = LongText;
			_gridNoBg.Handler?.VirtualView?.InvalidateMeasure();
		};

		Content = new ScrollView
		{
			Content = new VerticalStackLayout
			{
				Spacing = 20,
				Padding = new Thickness(20),
				Children =
				{
					refGridWithBg,
					refGridNoBg,
					_gridWithBg,
					buttonWithBg,
					_gridNoBg,
					buttonNoBg,
				}
			}
		};
	}
}
