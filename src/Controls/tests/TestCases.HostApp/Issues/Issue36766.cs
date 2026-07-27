using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 36766, "SwipeItem IconImageSource not visible on dark SwipeItem background (dark mode)", PlatformAffected.Android | PlatformAffected.iOS)]
public class Issue36766 : ContentPage
{
	readonly SwipeView _swipeView;

	public Issue36766()
	{
		if (Application.Current is not null)
			Application.Current.UserAppTheme = AppTheme.Dark;

		BackgroundColor = Colors.Black;

		var openBtn = new Button { AutomationId = "OpenSwipeButton", Text = "Open Swipe" };

		var swipeItem = new SwipeItem
		{
			BackgroundColor = Colors.Black,
			IconImageSource = "fruitsicon.png",
			IconColor = Colors.White,
			Text = "Back",
		};

		var swipeItems = new SwipeItems { swipeItem };
		swipeItems.Mode = SwipeMode.Execute;

		var items = new ObservableCollection<TestData>();
		for (int i = 0; i < 5; i++)
			items.Add(new TestData { Name = $"Item {i + 1}" });

		var collectionView = new CollectionView
		{
			ItemsSource = items,
			ItemTemplate = new DataTemplate(() =>
			{
				var label = new Label { Padding = new Thickness(10), TextColor = Colors.White };
				label.SetBinding(Label.TextProperty, "Name");
				return label;
			}),
		};

		_swipeView = new SwipeView
		{
			Threshold = 80,
			LeftItems = swipeItems,
			Content = collectionView,
		};

		openBtn.Clicked += (_, _) => _swipeView.Open(OpenSwipeItem.LeftItems, animated: false);

		var grid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star },
			},
		};
		grid.Add(openBtn, 0, 0);
		grid.Add(_swipeView, 0, 1);
		Content = grid;
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		if (Application.Current is not null)
			Application.Current.UserAppTheme = AppTheme.Unspecified;
	}

	class TestData
	{
		public string Name { get; set; }
	}
}
