using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var themeName = this.VisualTheme == ThemeVisuals.DarkDefault ? "DARK" : "LIGHT";
            Debug.WriteLine($"[THEME-DBG] UpdateVisualTheme START → {themeName}");

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
            Debug.WriteLine($"[THEME-DBG] UpdateVisualTheme END → {themeName}");
        }

        private void UpdateDefaultTheme(bool isDark = false)
        {
            Debug.WriteLine($"[THEME-DBG] UpdateDefaultTheme isDark={isDark}");
            this.MergedDictionaries.Clear();
            var newTheme = new DefaultTheme(isDark);
            Debug.WriteLine($"[THEME-DBG] Adding DefaultTheme(isDark={isDark}), inner keys: {string.Join(", ", newTheme.Select(kv => kv.Key))}");
            this.MergedDictionaries.Add(newTheme);
            Debug.WriteLine($"[THEME-DBG] MergedDictionaries.Add done");
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
