//using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using TestZone.Test.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestZone.Test.Models
{
    public class GraphicModel : TestZone.Test.Views.Models.ITitled
    {

        public ContentView _curveContent { get; set; }
        public ContentView curveContent { get { return _curveContent; } set { _curveContent = value; OnPropertyChanged();
            } }


        private string _Name { get; set; }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        public GraphicModel(String name)
        {
            this.Name = name;
         //   initCurve();
        }
        /*
        private void initCurve()
        {
            Console.WriteLine("INIT DRAWER");
            //TODO MODIFIER CETTE FONCTION


            MyCanvasView chartView = new MyCanvasView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Drawer = new CanvasDrawer { }
            };
            
            this.curveContent = new ContentView();
            this.curveContent.BackgroundColor = Colors.Green;
            this.curveContent.Content = chartView;

        }
        */
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
