using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SkiaSharp;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using TestZone.Test.Views.Models;
using TestZone.Test.Models;

namespace TestZone.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScrollTestViewWithoutCarousel : ContentPage
    {

        private ObservableCollection<ITitled> _ContentViews { get; set; } = new ObservableCollection<ITitled>();
        public ObservableCollection<ITitled> ContentViews { get { return _ContentViews; } set { _ContentViews = value; OnPropertyChanged(); } }
            
        public ScrollTestViewWithoutCarousel()
        {
           InitializeComponent();
            //BindingContext = this;
            init();
        }


    
        private void init()
        {
            this.ContentViews.Clear();
          
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    this.ContentViews.Add(new ScrollTestModel("Tous"));

                }
                else
                {
                    this.ContentViews.Add(new ScrollTestModel("View "  + i));
                }
            }
//           this._CarouselCompetencesView.ScrollTo(0);
        }





    }


    
}