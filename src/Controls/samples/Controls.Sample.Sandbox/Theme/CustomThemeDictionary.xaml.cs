namespace Maui.Controls.Sample;


[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CustomThemeDictionary : ResourceDictionary
{
	public CustomThemeDictionary()
	{
		InitializeComponent();
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isDark"></param>
    public CustomThemeDictionary(bool isDark = false)
    {
        if (isDark)
        {
            this.MergedDictionaries.Add(new DarkThemeColors());
        }
        else
        {
            this.MergedDictionaries.Add(new LightThemeColors());
        }

        this.InitializeElement();
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitializeElement()
    {
        InitializeComponent();
        ThemeElement.AddStyleDictionary(this);
    }
}