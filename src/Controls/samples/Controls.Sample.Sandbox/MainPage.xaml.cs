#nullable disable
using Maui.Controls.Sample.Control;

namespace Maui.Controls.Sample
{
    public partial class MainPage : ContentPage
    {
        DarkThemeColors darkTheme = new DarkThemeColors();
        LightThemeColors lightTheme = new LightThemeColors();
        List<string> stateList = new List<string> { "On", "Off", "Indeterminate" };
        ICollection<ResourceDictionary> mergedDictionaries;
        public MainPage()
        {
            InitializeComponent();
            //Application.Current.UserAppTheme = AppTheme.Dark;
            mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            customPicker1.ItemsSource = stateList;
            customPicker1.SelectedIndex = 2;
            customPicker2.ItemsSource = stateList;
            customPicker2.SelectedIndex = 1;
            customPicker3.ItemsSource = stateList;
            customPicker3.SelectedIndex = 0;
        }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        if(Application.Current is not null)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var theme = mergedDictionaries.OfType<CustomThemeResourceDictionary>().FirstOrDefault();
                if (theme != null)
                {
                    if (themeSwitch.IsToggled)
                    {
                        theme.VisualTheme = ThemeVisuals.DarkDefault;
                        Application.Current.UserAppTheme = AppTheme.Dark;
                    }
                    else
                    {
                        theme.VisualTheme = ThemeVisuals.LightDefault;
                        Application.Current.UserAppTheme = AppTheme.Light;
                    }
                }
            }
        }
    }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                if (Application.Current?.UserAppTheme == AppTheme.Light || Application.Current?.UserAppTheme == AppTheme.Unspecified)
                {
                    Application.Current.UserAppTheme = AppTheme.Dark;
                }
                else if (Application.Current?.UserAppTheme == AppTheme.Dark)
                {
                    Application.Current.UserAppTheme = AppTheme.Light;
                }

                if (mergedDictionaries != null)
                {
                    if (Application.Current.UserAppTheme is AppTheme.Light)
                    {
                        if (darkTheme != null)
                        {
                            mergedDictionaries.Remove(darkTheme);
                        }
                        mergedDictionaries.Add(lightTheme);
                    }
                    else
                    {
                        if (lightTheme != null)
                        {
                            mergedDictionaries.Remove(lightTheme);
                        }
                        mergedDictionaries.Add(darkTheme);
                    }
                }
            }
        }

        private void customPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker != null && picker == this.FindByName<Picker>("customPicker1"))
            {
                if (picker.SelectedItem.ToString() == "On")
                {
                    customControl1.IsOn = true;
                }
                else if (picker.SelectedItem.ToString() == "Off")
                {
                    customControl1.IsOn = false;
                }
                else
                {
                    customControl1.IsOn = null;
                }
            }
            else if (picker != null && picker == this.FindByName<Picker>("customPicker2"))
            {
                if (picker.SelectedItem.ToString() == "On")
                {
                    customControl2.IsOn = true;
                }
                else if (picker.SelectedItem.ToString() == "Off")
                {
                    customControl2.IsOn = false;
                }
                else
                {
                    customControl2.IsOn = null;
                }
            }
            else if (picker != null && picker == this.FindByName<Picker>("customPicker3"))
            {
                if (picker.SelectedItem.ToString() == "On")
                {
                    customControl3.IsOn = true;
                }
                else if (picker.SelectedItem.ToString() == "Off")
                {
                    customControl3.IsOn = false;
                }
                else
                {
                    customControl3.IsOn = null;
                }
            }
        }
    }

}
