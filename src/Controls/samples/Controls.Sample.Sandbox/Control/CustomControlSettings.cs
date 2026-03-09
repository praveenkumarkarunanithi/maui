#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.Controls.Sample.Control
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomControlSettings : Element, INotifyPropertyChanged
    {
        internal CustomControl customControl;

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty TrackBackgroundProperty =
BindableProperty.Create(nameof(TrackBackground), typeof(Color), typeof(CustomControlSettings), Colors.Red, propertyChanged: OnTrackBackgroundPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty =
           BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(CustomControlSettings), Colors.Green);

        /// <summary>
        /// 
        /// </summary>
        public Color TrackBackground
        {
            get => (Color)GetValue(TrackBackgroundProperty);
            set
            {
                SetValue(TrackBackgroundProperty, value);
                OnPropertyChanged(nameof(TrackBackground));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color StrokeColor
        {
            get => (Color)GetValue(StrokeColorProperty);
            set
            {
                SetValue(StrokeColorProperty, value);
                OnPropertyChanged(nameof(StrokeColor));
            }
        }

        internal CustomControl CustomControl
        {
            get
            {
                return customControl;
            }
            set
            {
                if (value != null)
                {
                    customControl = value;
                }
            }
        }

        private static void OnTrackBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
           System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Settings({bindable.GetHashCode()}).TrackBackground changed: {oldValue} → {newValue}, customControl={((CustomControlSettings)bindable).customControl?.GetHashCode()}");
           if (bindable is CustomControlSettings settings && settings.customControl is not null)
           {
               System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Settings → directly updating Control({settings.customControl.GetHashCode()})._box");
               settings.customControl._box.BackgroundColor = (Color)newValue;
           }
        }

        // NOTE: Do NOT declare a shadowed PropertyChanged event or OnPropertyChanged here.
        // CustomControlSettings inherits from Element (via BindableObject), which already
        // provides PropertyChanged and OnPropertyChanged. Declaring them here causes
        // subscriptions made on the shadowed event to never fire when the BindableProperty
        // system updates TrackBackground via DynamicResource (which calls base.OnPropertyChanged).

    }
}
