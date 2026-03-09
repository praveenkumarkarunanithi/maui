namespace Maui.Controls.Sample;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DefaultTheme : CustomThemeDictionary
{
    /// <summary>
    /// Initializes a new instance of the DefaultTheme class.
    /// </summary>
    public DefaultTheme() : base()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isDark"></param>
    public DefaultTheme(bool isDark = false) : base(isDark)
    {
        InitializeComponent();
    }

}