using System;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
    public class VisualStateManagerThemeTest : BaseTestFixture
    {
        MockAppInfo mockAppInfo;
        Application app;

        public VisualStateManagerThemeTest()
        {
            AppInfo.SetCurrent(mockAppInfo = new MockAppInfo() { RequestedTheme = AppTheme.Light });
            Application.Current = app = new Application();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Application.Current = null;
            }
            base.Dispose(disposing);
        }

        [Fact]
        public void ThemeChangeInVisualStateSetterShouldUpdate()
        {
            // Arrange
            var customControl = new Button();
            var page = new ContentPage { Content = customControl };
            app.LoadPage(page);

            // Create a visual state group with theme-based setters
            var visualStateGroup = new VisualStateGroup { Name = "CommonStates" };
            var normalState = new VisualState { Name = "Normal" };
            var disabledState = new VisualState { Name = "Disabled" };

            // Add a setter with AppThemeBinding to the Normal state
            normalState.Setters.Add(new Setter
            {
                Property = Button.BackgroundColorProperty,
                Value = new AppThemeBinding { Light = Colors.White, Dark = Colors.Black }
            });

            // Add a setter with AppThemeBinding to the Disabled state
            disabledState.Setters.Add(new Setter
            {
                Property = Button.BackgroundColorProperty,
                Value = new AppThemeBinding { Light = Colors.LightGray, Dark = Colors.DarkGray }
            });

            visualStateGroup.States.Add(normalState);
            visualStateGroup.States.Add(disabledState);

            var visualStateGroups = new VisualStateGroupList();
            visualStateGroups.Add(visualStateGroup);
            VisualStateManager.SetVisualStateGroups(customControl, visualStateGroups);

            // Act & Assert - Initial state should be Normal with Light theme
            VisualStateManager.GoToState(customControl, "Normal");
            Assert.Equal(Colors.White, customControl.BackgroundColor);

            // Change to Dark theme - this should update the background color
            SetAppTheme(AppTheme.Dark);
            Assert.Equal(Colors.Black, customControl.BackgroundColor);

            // Change to Disabled state
            VisualStateManager.GoToState(customControl, "Disabled");
            Assert.Equal(Colors.DarkGray, customControl.BackgroundColor);

            // Change back to Light theme - this should update the background color
            SetAppTheme(AppTheme.Light);
            Assert.Equal(Colors.LightGray, customControl.BackgroundColor);

            // Change back to Normal state
            VisualStateManager.GoToState(customControl, "Normal");
            Assert.Equal(Colors.White, customControl.BackgroundColor);
        }

        void SetAppTheme(AppTheme theme)
        {
            mockAppInfo.RequestedTheme = theme;
            ((IApplication)app).ThemeChanged();
        }
    }
}