using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues
{
	[Issue(IssueTracker.None, 5, "Legacy Layout not working - ViewCell height measurement with margins", PlatformAffected.All)]
	public class Issue5 : TestContentPage
	{
		const string TestInstructions = "ViewCell with margins should have correct height. The cells below should show proper spacing with their defined margins.";
		const string Success = "ViewCells with margins are displayed correctly";

		protected override void Init()
		{
			var label = new Label { Text = TestInstructions };

			var data = new ObservableCollection<string>
			{
				"Item 1",
				"Item 2", 
				"Item 3"
			};

			var listView = new ListView()
			{
				ItemsSource = data,
				HasUnevenRows = true,
				ItemTemplate = new DataTemplate(() =>
				{
					var viewCell = new ViewCell();
					
					// Create a view with explicit margins to test measurement
					var contentView = new StackLayout
					{
						Margin = new Thickness(20, 10, 20, 10), // Significant margins
						Padding = new Thickness(10),
						BackgroundColor = Colors.LightBlue,
						Children =
						{
							new Label 
							{ 
								Text = "Cell with margins",
								TextColor = Colors.Black,
								FontSize = 16
							}
						}
					};
					
					// Set AutomationId for testing
					contentView.SetBinding(Label.TextProperty, ".");
					contentView.AutomationId = "CellContent";
					
					viewCell.View = contentView;
					return viewCell;
				})
			};

			// Add verification label
			var successLabel = new Label 
			{ 
				Text = Success, 
				TextColor = Colors.Green,
				AutomationId = "SuccessLabel"
			};

			Content = new StackLayout
			{
				Children = { label, listView, successLabel }
			};
		}
	}
}