namespace Maui.Controls.Sample.Issues;
[Issue(IssueTracker.Github, 29428, "Shell flyout navigation fires NavigatedTo before Loaded event", PlatformAffected.iOS | PlatformAffected.Android)]
public class Issue29428 : Shell
{
    public Issue29428()
    {
        System.Diagnostics.Debug.WriteLine($"[Sample Debug] Issue29428 Shell constructor started");
        
        var mainPage = new ContentPage
        {
            Title = "Main Page",
            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Children =
                {
                    new Label
                    {
                        Text = "This is the main page. Use the flyout to navigate to Page 2.",
                        AutomationId = "MainPageLabel"
                    }
                }
            }
        };

        var secondPage = new EventOrderTestPage29428();
        Items.Add(new FlyoutItem
        {
            Title = "Main Page",
            Items =
            {
                new ShellSection
                {
                    Items = { new ShellContent { Content = mainPage } }
                }
            }
        });

        Items.Add(new FlyoutItem
        {
            Title = "Page 2",
            Items =
            {
                new ShellSection
                {
                    Items = { new ShellContent { Content = secondPage } }
                }
            }
        });
        
        System.Diagnostics.Debug.WriteLine($"[Sample Debug] Issue29428 Shell constructor completed");
    }

    public class EventOrderTestPage29428 : ContentPage
    {
        private readonly Label _eventOrderLabel;

        public EventOrderTestPage29428()
        {
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428 constructor started");
            
            Title = "SecondPage";
            AutomationId = "SecondPage";
            _eventOrderLabel = new Label
            {
                AutomationId = "EventOrderLabel",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Children =
                {
                    new Label
                    {
                        Text = "Page 2 - Event Order Test",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    _eventOrderLabel
                }
            };

            Loaded += OnLoaded;
            NavigatedTo += OnNavigatedTo;
            
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428 constructor completed");
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428.OnLoaded fired");
            _eventOrderLabel.Text = string.IsNullOrEmpty(_eventOrderLabel.Text)
                ? "Loaded called first"
                : $"{_eventOrderLabel.Text} then Loaded called";
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428 label text: {_eventOrderLabel.Text}");
        }

        private void OnNavigatedTo(object sender, NavigatedToEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428.OnNavigatedTo fired");
            _eventOrderLabel.Text = string.IsNullOrEmpty(_eventOrderLabel.Text)
                ? "NavigatedTo called first"
                : $"{_eventOrderLabel.Text} then NavigatedTo called";
            System.Diagnostics.Debug.WriteLine($"[Sample Debug] EventOrderTestPage29428 label text: {_eventOrderLabel.Text}");
        }

        protected override void OnDisappearing()
        {
            _eventOrderLabel.Text = string.Empty;
            base.OnDisappearing();
        }
    }
}
