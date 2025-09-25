using Microsoft.Maui.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestZone.Test.Views.Models;

namespace TestZone.Test.Views
{
    public class CanvasDrawer : INotifyPropertyChanged
    {
        public ITitled _titled { get; set; }
        public ITitled titled
        {
            get { return _titled; }
            set
            {
                _titled = value; OnPropertyChanged();
            }
        }

        public void Draw(SKCanvas canvas)
        {
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.Fill
            };
            canvas.DrawRect(0, 0, 100, 200, paint);
            paint.TextSize = 25;
            canvas.DrawText(_titled.Name, new SKPoint(100,300), paint);
        }


       
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}
