using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Brush = Microsoft.Maui.Controls.Brush;
using System.Collections.ObjectModel;

namespace TestZone.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScrollRepere : ContentView
    {
        private float _lineWidth { get; set; }
        public float LineWidth { get { return _lineWidth; } set { _lineWidth = value; OnPropertyChanged(); } }

        private bool ShowedExtremities = false;
        private float _pourcentage;

        public float Pourcentage
        {
            get { return _pourcentage; }
            set { _pourcentage = value; 
                
                
                OnPropertyChanged();
                OnPourcentageChanged();
            }
        }

        bool firstCall = true;

        public void OnPourcentageChanged()
        {
            for(int i = 0; i < ellipses.Count; i++) { 
                float circlePourcenrtage = (float)i / (float)(NbCircle - 1);
                Brush bgCircle = new SolidColorBrush(Colors.Transparent);
                if (Pourcentage >= circlePourcenrtage)
                {
                    bgCircle = new SolidColorBrush(Colors.White);
                    lastCircleIndex = i;
                }
                ellipses[i].Fill = bgCircle;
            }
        }

        public ScrollRepere()
        {
            InitializeComponent();
            BindingContext = this;
            _stack.SizeChanged += Layout_SizeChanged;
            firstCall = true;
        }


        public static readonly BindableProperty ScrollViewProperty = BindableProperty.Create(nameof(ScrollView), typeof(ScrollView), typeof(ScrollRepere), propertyChanged: OnInitScroll);

        public ScrollView ScrollView
        {
            get { return (ScrollView)GetValue(ScrollViewProperty); }
            set
            {
                SetValue(ScrollViewProperty, value);
            }
        }

        public static readonly BindableProperty NbCircleProperty = BindableProperty.Create(nameof(NbCircle), typeof(int), typeof(ScrollRepere), propertyChanged: OnToggleStateChanged);

        //  private int _nbCircle { get; set; }
        public int NbCircle
        {
            get { return (int)GetValue(NbCircleProperty); }
            set
            {
                SetValue(NbCircleProperty, value);
            }
        }

        static void OnInitScroll(BindableObject bindable, object oldValue, object newValue)
        {
            ScrollRepere view = (ScrollRepere)bindable;
            view.ScrollView = (ScrollView)newValue;
            view.ScrollView.Scrolled += (s, e) =>
            {
                view.Pourcentage = (float)view.ScrollView.ScrollX / (float)(view.ScrollView.ContentSize.Width - view.ScrollView.Width);
            };
        }


        static void OnToggleStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ScrollRepere view = (ScrollRepere)bindable;
            view.NbCircle = (int)newValue;
            view.Init();
        }

        private float lastCircleIndex = 0;

        private ObservableCollection<Line> lines = new ObservableCollection<Line>();
        private ObservableCollection<Ellipse> ellipses = new ObservableCollection<Ellipse>();
        private void Init()
        {
            float lastCirclePourcenrtage = (float)lastCircleIndex / (float)(NbCircle - 1);
            float nextCirclePourcenrtage = (float)(lastCircleIndex + 1) / (float)(NbCircle - 1);
            if (Pourcentage <= lastCirclePourcenrtage || Pourcentage >= nextCirclePourcenrtage || firstCall)
            {
        
                firstCall = false;
                SolidColorBrush brush = new SolidColorBrush(Colors.White);               
                int diametreCircle = 16;
                int lineY = diametreCircle / 2;
                double totalWidthCircle = NbCircle * diametreCircle;
                double totalWidthLine = _stack.Bounds.Width - totalWidthCircle;
                LineWidth = (float)(totalWidthLine / (ShowedExtremities ? (NbCircle+1) : NbCircle-1));

                for (int i = 0; i < NbCircle; i++)
                {
                    Line A = new Line()
                    {
                        X1 = 0,
                        X2 = LineWidth,
                        Y1 = lineY,
                        Y2 = lineY,
                        StrokeThickness = 1,
                        Stroke = brush,
                        Margin = 0
                    };

                    A.BindingContext = this;
                    A.SetBinding(Line.X2Property, "LineWidth", BindingMode.TwoWay);

                    float circlePourcenrtage = (float)i / (float)(NbCircle - 1);

                    Brush bgCircle = new SolidColorBrush(Colors.Transparent);
                    if (Pourcentage >= circlePourcenrtage)
                    {
                        bgCircle = new SolidColorBrush(Colors.White);
                        lastCircleIndex = i;
                    }

                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = bgCircle,
                        WidthRequest = diametreCircle,
                        HeightRequest = diametreCircle,
                        StrokeThickness = 1,
                        Stroke = brush,
                        Margin = 0
                    };
                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += ELlipseTapped; 
                    tapGestureRecognizer.NumberOfTapsRequired = 1;

                    ellipse.GestureRecognizers.Add(tapGestureRecognizer);

                    if(i > 0 || this.ShowedExtremities) {
                        lines.Add(A);
                        this._stack.Children.Add(A);
                    }
                    ellipses.Add(ellipse);
                    _stack.Children.Add(ellipse);
                }
                if (ShowedExtremities) {
                    Line endLine = new Line()
                    {
                        X1 = 0,
                        X2 = LineWidth,
                        Y1 = lineY,
                        Y2 = lineY,
                        StrokeThickness = 1,
                        Stroke = brush,
                        Margin = 0
                    };
                    endLine.BindingContext = this;
                    endLine.SetBinding(Line.X2Property, "LineWidth", BindingMode.TwoWay);
                    lines.Add(endLine);


                    _stack.Children.Add(endLine);
                }
            }
        }



        public void Layout_SizeChanged(object? sender, EventArgs e)
        {
            int diametreCircle = 16;
            float totalWidthCircle = NbCircle * diametreCircle;
            float totalWidthLine = (float)(_stack.Bounds.Width - totalWidthCircle);
            LineWidth = (float)totalWidthLine / (float)(ShowedExtremities ? (NbCircle + 1) : NbCircle - 1);
//            InvalidateLayout();
        }


        public void ELlipseTapped(object? sender, EventArgs e)
        {            
            Ellipse ellipse = sender as Ellipse;
            float circlePourcenrtage = (float)ellipses.IndexOf(ellipse) / (float)(NbCircle - 1);
            if (ScrollView != null)
            {
                int scrollX = (int)(circlePourcenrtage * (this.ScrollView.ContentSize.Width - ScrollView.Width)) + 1;
                ScrollView.ScrollToAsync(scrollX, 0, true);
            }
        }
    }




}