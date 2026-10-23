using System.Collections.ObjectModel;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 34965,
	"[iOS 26] CarouselView PreviousPosition/PreviousItem incorrect during animated ScrollTo in CarouselViewHandler2",
	PlatformAffected.iOS | PlatformAffected.macOS)]
public class Issue34965 : ContentPage
{
	readonly CarouselView _carouselView;
	readonly ObservableCollection<string> _items;
	readonly Label _previousItemLabel;
	readonly Label _previousPositionLabel;
	readonly Label _currentItemLabel;
	readonly Label _currentPositionLabel;

	public Issue34965()
	{
		_items = new ObservableCollection<string>
		{
			"Item 1",
			"Item 2",
			"Item 3",
			"Item 4",
			"Item 5",
			"Item 6",
			"Item 7",
			"Item 8",
			"Item 9",
			"Item 10"
		};

		_currentItemLabel = new Label
		{
			AutomationId = "CurrentItemLabel",
			Text = "Current Item: Item 1"
		};

		_previousItemLabel = new Label
		{
			AutomationId = "PreviousItemLabel",
			Text = "Previous Item: none"
		};

		_currentPositionLabel = new Label
		{
			AutomationId = "CurrentPositionLabel",
			Text = "Current Position: 0"
		};

		_previousPositionLabel = new Label
		{
			AutomationId = "PreviousPositionLabel",
			Text = "Previous Position: none"
		};

		_carouselView = new CarouselView2
		{
			AutomationId = "CarouselView",
			Loop = false,
			ItemsSource = _items,
			ItemTemplate = new DataTemplate(() =>
			{
				var label = new Label
				{
					FontSize = 24,
					BackgroundColor = Colors.LightGray,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Center,
					HorizontalTextAlignment = TextAlignment.Center
				};
				label.SetBinding(Label.TextProperty, ".");
				return label;
			})
		};

		_carouselView.CurrentItemChanged += OnCurrentItemChanged;
		_carouselView.PositionChanged += OnPositionChanged;

		var loopOffButton = new Button
		{
			AutomationId = "LoopOffButton",
			Text = "Loop Off"
		};
		loopOffButton.Clicked += (s, e) =>
		{
			_carouselView.Loop = false;
		};

		var loopOnButton = new Button
		{
			AutomationId = "LoopOnButton",
			Text = "Loop On"
		};
		loopOnButton.Clicked += (s, e) =>
		{
			_carouselView.Loop = true;
		};

		var animateOffButton = new Button
		{
			AutomationId = "AnimateOffButton",
			Text = "Animate Off"
		};
		animateOffButton.Clicked += (s, e) =>
		{
			_carouselView.IsScrollAnimated = false;
		};

		var animateOnButton = new Button
		{
			AutomationId = "AnimateOnButton",
			Text = "Animate On"
		};
		animateOnButton.Clicked += (s, e) =>
		{
			_carouselView.IsScrollAnimated = true;
		};

		var scrollTo3Button = new Button
		{
			AutomationId = "ScrollTo3Button",
			Text = "Scroll To 3"
		};
		scrollTo3Button.Clicked += (s, e) =>
		{
			var animate = _carouselView.IsScrollAnimated;
			_carouselView.ScrollTo(3, animate: animate);
		};

		var scrollTo1Button = new Button
		{
			AutomationId = "ScrollTo1Button",
			Text = "Scroll To 1"
		};
		scrollTo1Button.Clicked += (s, e) =>
		{
			var animate = _carouselView.IsScrollAnimated;
			_carouselView.ScrollTo(1, animate: animate);
		};

		var setPosition3Button = new Button
		{
			AutomationId = "SetPosition3Button",
			Text = "Set Position 3"
		};
		setPosition3Button.Clicked += (s, e) =>
		{
			_carouselView.Position = 3;
		};

		var setPosition0Button = new Button
		{
			AutomationId = "SetPosition0Button",
			Text = "Set Position 0"
		};
		setPosition0Button.Clicked += (s, e) =>
		{
			_carouselView.Position = 0;
		};

		var scrollToLastButton = new Button
		{
			AutomationId = "ScrollToLastButton",
			Text = "Scroll To Last"
		};
		scrollToLastButton.Clicked += (s, e) =>
		{
			var animate = _carouselView.IsScrollAnimated;
			_carouselView.ScrollTo(_items.Count - 1, animate: animate);
		};

		var scrollToElementLastButton = new Button
		{
			AutomationId = "ScrollToElementLastButton",
			Text = "Scroll To Last Item"
		};
		scrollToElementLastButton.Clicked += (s, e) =>
		{
			var animate = _carouselView.IsScrollAnimated;
			_carouselView.ScrollTo(_items[_items.Count - 1], animate: animate);
		};

		var scrollToElement1Button = new Button
		{
			AutomationId = "ScrollToElement1Button",
			Text = "Scroll To Item 2"
		};
		scrollToElement1Button.Clicked += (s, e) =>
		{
			var animate = _carouselView.IsScrollAnimated;
			_carouselView.ScrollTo(_items[1], animate: animate);
		};

		var setPosition1Button = new Button
		{
			AutomationId = "SetPosition1Button",
			Text = "Set Position 1"
		};
		setPosition1Button.Clicked += (s, e) =>
		{
			_carouselView.Position = 1;
		};

		var setCurrentItemLastButton = new Button
		{
			AutomationId = "SetCurrentItemLastButton",
			Text = "Set CurrentItem Last"
		};
		setCurrentItemLastButton.Clicked += (s, e) =>
		{
			_carouselView.CurrentItem = _items[_items.Count - 1];
		};

		var setCurrentItem1Button = new Button
		{
			AutomationId = "SetCurrentItem1Button",
			Text = "Set CurrentItem 2"
		};
		setCurrentItem1Button.Clicked += (s, e) =>
		{
			_carouselView.CurrentItem = _items[1];
		};

		Content = new Grid
		{
			Padding = new Thickness(10),
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star }
			},
			Children =
			{
				new VerticalStackLayout
				{
					Spacing = 6,
					Children =
					{
						_currentItemLabel,
						_previousItemLabel,
						_currentPositionLabel,
						_previousPositionLabel,
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { loopOffButton, loopOnButton }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { animateOffButton, animateOnButton }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { scrollTo3Button, scrollTo1Button }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { scrollToLastButton, scrollToElementLastButton }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { scrollToElement1Button, setPosition3Button }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { setPosition1Button, setCurrentItemLastButton }
						},
						new HorizontalStackLayout
						{
							Spacing = 8,
							Children = { setCurrentItem1Button, setPosition0Button }
						}
					}
				},
				_carouselView
			}
		};

		Grid.SetRow(_carouselView, 1);
	}

	void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
	{
		_currentItemLabel.Text = $"Current Item: {e.CurrentItem}";
		_previousItemLabel.Text = $"Previous Item: {e.PreviousItem ?? "none"}";
	}

	void OnPositionChanged(object sender, PositionChangedEventArgs e)
	{
		_currentPositionLabel.Text = $"Current Position: {e.CurrentPosition}";
		_previousPositionLabel.Text = $"Previous Position: {e.PreviousPosition}";
	}
}
