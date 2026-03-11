using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Controls.Sample
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomThemeResourceDictionary : ResourceDictionary
    {

        private ThemeVisuals visualTheme = ThemeVisuals.LightDefault;

        /// <summary>
        /// 
        /// </summary>
        public ThemeVisuals VisualTheme
        {
            get
            {
                return visualTheme;
            }
            set
            {
                visualTheme = value;

                this.UpdateVisualTheme();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomThemeResourceDictionary()
        {
            this.UpdateDefaultTheme();
        }

        private void UpdateVisualTheme()
        {
            this.MergedDictionaries.Clear();
            this.UpdateDefaultTheme();
            if (this.VisualTheme == ThemeVisuals.LightDefault)
            {
                this.UpdateDefaultTheme();
            }
            else if (this.VisualTheme == ThemeVisuals.DarkDefault)
            {
                this.UpdateDefaultTheme(true);
            }
        }

        private void UpdateDefaultTheme(bool isDark = false)
        {
            this.MergedDictionaries.Clear();
            this.MergedDictionaries.Add(new DefaultTheme(isDark));
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public enum ThemeVisuals
    {
        /// <summary>
        /// 
        /// </summary>
        LightDefault,

        /// <summary>
        /// 
        /// </summary>
        DarkDefault

    }
}
