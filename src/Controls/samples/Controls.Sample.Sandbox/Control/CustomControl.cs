#nullable disable
using Maui.Controls.Sample.ControlStyling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maui.Controls.Sample;

namespace Maui.Controls.Sample.Control
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomControl:ContentView,IParentThemeElement
    {

        private const string OffStateName = "Off";

        private string currentState = OffStateName;

        private bool isHovered;

        private bool isPressed;

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty IsOnProperty =
            BindableProperty.Create(nameof(IsOn), typeof(bool?), typeof(CustomControl), false, BindingMode.TwoWay, null, OnIsOnPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty CustomControlSettingsProperty =
                BindableProperty.Create(nameof(CustomControlSettings), typeof(CustomControlSettings), typeof(CustomControl), null, BindingMode.TwoWay, propertyChanged: OnCustomControlSettingsPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public bool? IsOn
        {
            get { return (bool?)this.GetValue(IsOnProperty); }
            set { this.SetValue(IsOnProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomControlSettings CustomControlSettings
        {
            get { return (CustomControlSettings)this.GetValue(CustomControlSettingsProperty); }
            set { this.SetValue(CustomControlSettingsProperty, value); }
        }

        private static void OnIsOnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as CustomControl)?.ChangeVisualState();

        }

        private static void OnCustomControlSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ctrl = bindable as CustomControl;
            System.Diagnostics.Debug.WriteLine($"[THEME-DBG] CustomControl({ctrl?.GetHashCode()}).CustomControlSettings changed: old={oldValue?.GetHashCode()} new={newValue?.GetHashCode()}");

            // Unsubscribe from the OLD settings (not the new one — prior code had this reversed)
            if (oldValue is CustomControlSettings previousSetting)
            {
                previousSetting.PropertyChanged -= ctrl.OnDefaultCustomControlSettings_PropertyChanged;
                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Disconnecting old Settings({previousSetting.GetHashCode()}) from Control({ctrl?.GetHashCode()}), setting Parent=null");
                previousSetting.customControl = null;
                previousSetting.BindingContext = null;
                SetInheritedBindingContext(previousSetting, null);
                previousSetting.Parent = null;
            }

            if (newValue is CustomControlSettings currentSetting && bindable is CustomControl customControl)
            {
                // Subscribe to the NEW settings
                currentSetting.PropertyChanged += customControl.OnDefaultCustomControlSettings_PropertyChanged;
                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] CustomControl({customControl.GetHashCode()}) subscribed PropertyChanged to Settings({currentSetting.GetHashCode()})");

                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Connecting Settings({currentSetting.GetHashCode()}) to Control({customControl.GetHashCode()})");
                currentSetting.Parent = currentSetting.CustomControl = customControl;
                customControl.UpdateCurrentStyle();
                SetInheritedBindingContext(currentSetting, customControl.BindingContext);
                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Settings({currentSetting.GetHashCode()}) parented to Control({customControl.GetHashCode()}). TrackBackground={currentSetting.TrackBackground}");
            }
        }

        void OnDefaultCustomControlSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var senderSettings = sender as CustomControlSettings;
            System.Diagnostics.Debug.WriteLine(
                $"[THEME-DBG] PropChanged on Settings({senderSettings?.GetHashCode()}): PropertyName='{e.PropertyName}', control={GetHashCode()}, mySettings={CustomControlSettings?.GetHashCode()}");

            // Only act if the firing settings is our CURRENT settings.
            // Stale subscriptions from previous states must not apply wrong interim colors.
            if (!ReferenceEquals(sender, CustomControlSettings))
            {
                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Skipping PropChanged — sender != current settings, no-op");
                return;
            }

            if (e.PropertyName == nameof(CustomControlSettings.TrackBackground) || e.PropertyName == null)
            {
                var currentSettings = CustomControlSettings;
                System.Diagnostics.Debug.WriteLine(
                    $"[THEME-DBG] PropertyChanged FIRED: control={GetHashCode()}, " +
                    $"sender={senderSettings?.GetHashCode()}, newColor={senderSettings?.TrackBackground}, " +
                    $"mySettings={currentSettings?.GetHashCode()}, myColor={currentSettings?.TrackBackground}");
                UpdateCurrentStyle();
            }
        }

        internal Border _box;

        /// <summary>
        /// 
        /// </summary>
        public CustomControl()
        {
            _box = new Border
            {

                WidthRequest = 100,
                HeightRequest = 100
            };
            ThemeElement.InitializeThemeResources(this, "CustomControlTheme");

            PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();

            // Attach event handlers
            pointerGestureRecognizer.PointerEntered += OnPointerEntered;
            pointerGestureRecognizer.PointerExited += OnPointerExited;
            pointerGestureRecognizer.PointerMoved += OnPointerMoved;
            pointerGestureRecognizer.PointerPressed += PointerGestureRecognizer_PointerPressed;
            pointerGestureRecognizer.PointerReleased += PointerGestureRecognizer_PointerReleased;

            GestureRecognizers.Add(pointerGestureRecognizer);

            Content = _box;
        }

        /// <summary>
        /// 
        /// </summary>
        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public void UpdateCurrentStyle()
        {
            if (_box is not null && CustomControlSettings is not null)
            {
                var newColor = CustomControlSettings.TrackBackground;
                System.Diagnostics.Debug.WriteLine($"[THEME-DBG] Control({GetHashCode()}).UpdateCurrentStyle: Settings({CustomControlSettings.GetHashCode()}).TrackBackground={newColor} → _box was {_box.BackgroundColor}");
                _box.BackgroundColor = newColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ChangeVisualState()
        {
            if (IsOn == true)
            {
                if (isPressed && isHovered)
                {
                    currentState = "OnPressed";
                }
                else if (isHovered)
                {
                    currentState = "OnHovered";
                }
                else
                {
                    currentState = "On";
                }
            }
            else if (IsOn == false)
            {
                if (isPressed && isHovered)
                {
                    currentState = "OffPressed";
                }
                else if (isHovered)
                {
                    currentState = "OffHovered";
                }
                else
                {
                    currentState = "Off";
                }
            }
            else
            {
                if (isPressed && isHovered)
                {
                    currentState = "IndeterminatePressed";
                }
                else if (isHovered)
                {
                    currentState = "IndeterminateHovered";
                }
                else
                {
                    currentState = "Indeterminate";
                }
            }
            VisualStateManager.GoToState(this, currentState);
            UpdateCurrentStyle();
        }

        /// <summary>
        /// Workaround to refresh DynamicResource values in VisualStates after theme changes.
        /// </summary>
        public void RefreshThemeColors()
        {
            var tempState = currentState != "Off" ? "Off" : "On";
            VisualStateManager.GoToState(this, tempState);
            VisualStateManager.GoToState(this, currentState);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.CustomControlSettings != null)
            {
				SetInheritedBindingContext(this.CustomControlSettings, this.BindingContext);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            UpdateCurrentStyle();
        }

        private void OnPointerEntered(object sender, PointerEventArgs e)
        {
            isHovered = true;
            ChangeVisualState();
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            isHovered = false;
            ChangeVisualState();
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            ChangeVisualState();
        }

        private void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
        {
            isPressed = false;
            ChangeVisualState();
        }

        private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
        {
            isPressed = true;
            ChangeVisualState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ResourceDictionary GetThemeDictionary()
        {
            return new CustomControlStyling();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldTheme"></param>
        /// <param name="newTheme"></param>
        public void OnCommonThemeChanged(string oldTheme, string newTheme)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldTheme"></param>
        /// <param name="newTheme"></param>
        public void OnControlThemeChanged(string oldTheme, string newTheme)
        {
            
        }
    }
}
