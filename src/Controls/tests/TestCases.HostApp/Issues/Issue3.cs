using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.Github, 3, "[Bug] Shell.Current.GoToAsync doesn't display a back button on Windows after navigating to another Shell page",
        PlatformAffected.Windows)]
    public class Issue3 : TestShell
    {
        protected override void Init()
        {
            // Create main page with navigation to another shell
            var mainPage = new ContentPage
            {
                Title = "Main Page",
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label 
                        { 
                            Text = "Main Shell Page - Click button to navigate to another Shell",
                            AutomationId = "MainPageLabel"
                        },
                        new Button
                        {
                            Text = "Navigate to Another Shell",
                            AutomationId = "NavigateButton",
                            Command = new Command(async () => await NavigateToAnotherShell())
                        }
                    }
                }
            };

            // Register route for another shell
            Routing.RegisterRoute("AnotherShell", typeof(AnotherShell));
            
            Items.Add(new ShellContent { Content = mainPage });
        }

        private async Task NavigateToAnotherShell()
        {
            // This navigation should preserve back button visibility on Windows
            await Shell.Current.GoToAsync("AnotherShell");
        }
    }

    public class AnotherShell : Shell
    {
        public AnotherShell()
        {
            var anotherPage = new ContentPage
            {
                Title = "Another Shell Page",
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label 
                        { 
                            Text = "Another Shell - Back button should be visible",
                            AutomationId = "AnotherShellLabel"
                        },
                        new Button
                        {
                            Text = "Navigate to Detail Page",
                            AutomationId = "DetailButton", 
                            Command = new Command(async () => await GoToAsync("DetailPage"))
                        }
                    }
                }
            };

            // Register route for detail page within this shell
            Routing.RegisterRoute("DetailPage", typeof(DetailPageInAnotherShell));

            Items.Add(new ShellContent { Content = anotherPage });
        }
    }

    public class DetailPageInAnotherShell : ContentPage
    {
        public DetailPageInAnotherShell()
        {
            Title = "Detail Page";
            Content = new StackLayout
            {
                Children =
                {
                    new Label 
                    { 
                        Text = "Detail Page in Another Shell - Back button should be visible",
                        AutomationId = "DetailPageLabel"
                    }
                }
            };
        }
    }
}