namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 35767, "SearchHandler.ShowsResults does not work dynamically", PlatformAffected.All)]
public class Issue35767 : Shell
{
	public Issue35767()
	{
		Items.Add(new ShellContent
		{
			Title = "Animals",
			Content = new _35767Page()
		});
	}

	class _35767Page : ContentPage
	{
		readonly _35767SearchHandler _searchHandler;

		public _35767Page()
		{
			Title = "Animals";
			var statusLabel = new Label
			{
				Text = "None",
				AutomationId = "Issue35767StatusLabel"
			};

			_searchHandler = new _35767SearchHandler(statusLabel)
			{
				Placeholder = "Search animals...",
				ShowsResults = false,
				ItemTemplate = new DataTemplate(() =>
				{
					var label = new Label { AutomationId = "Issue35767ResultItem" };
					label.SetBinding(Label.TextProperty, ".");
					return label;
				})
			};

			Shell.SetSearchHandler(this, _searchHandler);

			Content = new VerticalStackLayout
			{
				Padding = 20,
				Spacing = 10,
				Children =
				{
					statusLabel,
					new Button
					{
						Text = "ShowsResults = True",
						AutomationId = "Issue35767TrueButton",
						Command = new Command(() => _searchHandler.ShowsResults = true)
					},
					new Button
					{
						Text = "ShowsResults = False",
						AutomationId = "Issue35767FalseButton",
						Command = new Command(() => _searchHandler.ShowsResults = false)
					},
					new Button
					{
						Text = "Reset",
						AutomationId = "Issue35767ResetButton",
						Command = new Command(() =>
						{
							_searchHandler.ShowsResults = false;
							_searchHandler.Query = string.Empty;
							statusLabel.Text = "None";
						})
					}
				}
			};
		}
	}

	class _35767SearchHandler : SearchHandler
	{
		readonly Label _statusLabel;

		static readonly List<string> Animals = new List<string>
		{
			"Beagle", "Bulldog", "Bengal Cat"
		};

		public _35767SearchHandler(Label statusLabel)
		{
			_statusLabel = statusLabel;
		}

		protected override void OnQueryChanged(string oldValue, string newValue)
		{
			base.OnQueryChanged(oldValue, newValue);
			var results = string.IsNullOrWhiteSpace(newValue)
				? null
				: Animals.FindAll(a => a.IndexOf(newValue, StringComparison.OrdinalIgnoreCase) >= 0);
			ItemsSource = results;
		}

		protected override void OnItemSelected(object item)
		{
			base.OnItemSelected(item);
			if (item is string animal)
				_statusLabel.Text = animal;
		}
	}
}
