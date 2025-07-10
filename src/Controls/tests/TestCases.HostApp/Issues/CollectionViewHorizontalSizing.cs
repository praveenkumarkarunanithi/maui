using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 99999, "CollectionView horizontal layouts should size to content height on iOS", PlatformAffected.iOS)]
public class CollectionViewHorizontalSizing : ContentPage
{
	public CollectionViewHorizontalSizing()
	{
		Title = "CollectionView Horizontal Sizing Test";

		var items = new[]
		{
			new { Text = "Short" },
			new { Text = "Medium Height Text" },
			new { Text = "Very Long Text That Should Make This Cell Taller Than The Others" },
			new { Text = "Another" },
			new { Text = "Item" }
		};

		// Test 1: Horizontal LinearItemsLayout
		var horizontalLinearCollectionView = new CollectionView
		{
			ItemsSource = items,
			ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
			{
				ItemSpacing = 10
			},
			ItemTemplate = new DataTemplate(() =>
			{
				var label = new Label
				{
					BackgroundColor = Colors.LightBlue,
					Padding = 10,
					TextColor = Colors.Black
				};
				label.SetBinding(Label.TextProperty, "Text");
				return label;
			}),
			BackgroundColor = Colors.Yellow,
			AutomationId = "HorizontalLinearCollectionView"
		};

		// Test 2: Horizontal GridItemsLayout
		var horizontalGridCollectionView = new CollectionView
		{
			ItemsSource = items,
			ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Horizontal)
			{
				HorizontalItemSpacing = 10,
				VerticalItemSpacing = 5
			},
			ItemTemplate = new DataTemplate(() =>
			{
				var label = new Label
				{
					BackgroundColor = Colors.LightGreen,
					Padding = 10,
					TextColor = Colors.Black
				};
				label.SetBinding(Label.TextProperty, "Text");
				return label;
			}),
			BackgroundColor = Colors.Orange,
			AutomationId = "HorizontalGridCollectionView"
		};

		Content = new StackLayout
		{
			Padding = 20,
			Spacing = 20,
			Children =
			{
				new Label
				{
					Text = "Horizontal CollectionViews should size to their content height, not fill container:",
					FontAttributes = FontAttributes.Bold
				},
				new Label
				{
					Text = "Linear Layout (should be height of tallest cell):",
					FontAttributes = FontAttributes.Italic
				},
				horizontalLinearCollectionView,
				new Label
				{
					Text = "Grid Layout with span=2 (should be height of 2 rows):",
					FontAttributes = FontAttributes.Italic
				},
				horizontalGridCollectionView,
				new Label
				{
					Text = "✓ If CollectionViews above size to content, the test passes",
					TextColor = Colors.Green
				},
				new Label
				{
					Text = "✗ If CollectionViews above fill remaining container height, the test fails",
					TextColor = Colors.Red
				}
			}
		};
	}
}