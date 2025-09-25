using Microsoft.Maui.Graphics;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using SkiaSharp;
using System;
using SkiaSharp.Views.Maui.Controls;
using TestZone.Test.Models;
using TestZone.Test.Views.Models;


namespace TestZone.Test.Views
{
    public class MyCanvasView : SKCanvasView
    {
        #region Constructors
        private float lastXTouch = -1, lastYTouch = -1;
        private bool isPressed;
        public MyCanvasView()
        {
            BackgroundColor = Colors.Transparent;
            PaintSurface += OnPaintCanvas;
            EnableTouchEvents = true;
            this.drawer = new CanvasDrawer();
            Console.WriteLine("NEW CANVAS VIEW");
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;

        }

        public static readonly BindableProperty TitledProperty = BindableProperty.Create(nameof(Titled), typeof(ITitled), typeof(MyCanvasView), null, propertyChanged: OnTitledChanged);

        #endregion

        #region Fields
        
//        public ITitled _titled { get; set; }
        public ITitled Titled
        {

            get { return (ITitled)GetValue(TitledProperty); }
            set {

                Console.WriteLine("SET TITLED INVALIDATE SURFACE");
                SetValue(TitledProperty, value); 
                InvalidateSurface();
            }
        }

        public event EventHandler<SKPaintSurfaceEventArgs> ChartPainted;

        #endregion

        #region Static fields

        public static readonly BindableProperty DrawerProperty = BindableProperty.Create(nameof(Drawer), typeof(CanvasDrawer), typeof(MyCanvasView), null, propertyChanged: OnChartChanged);

        #endregion

        #region Fields

        private CanvasDrawer drawer;

        #endregion

        #region Properties

        public CanvasDrawer Drawer
        {
            get { return (CanvasDrawer)GetValue(DrawerProperty); }
            set { SetValue(DrawerProperty, value); }
        }

        #endregion

        #region Methods

        private static void OnChartChanged(BindableObject d, object oldValue, object value)
        {
            var view = d as MyCanvasView;
            view.drawer = value as CanvasDrawer;
            view.InvalidateSurface();

        }
        private static void OnTitledChanged(BindableObject d, object oldValue, object value)
        {
            var view = d as MyCanvasView;
            view.Titled = value as ITitled;
            view.InvalidateSurface();

        }
        private bool FirstPaint = true;
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (drawer != null)
            {
                Console.WriteLine("SET TITLED - ON PAINT CANVAS");

                drawer.titled = Titled;

                if (FirstPaint)
                {
                    FirstPaint = false;
                }
                drawer.Draw(e.Surface.Canvas);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }

            ChartPainted?.Invoke(sender, e);
        }


        #endregion
    }
}