using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using TestZone.Test.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using TestZone.Test.Views.Models;

namespace TestZone.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public ObservableCollection<ITitled> ContentViews { get; set; } = new ObservableCollection<ITitled>();


        public TestPage()
        {
            InitializeComponent();
//            this.BindingContext = new TestPageViewModel();
            for(int i = 0 ; i < 30; i++)
            {
                ContentViews.Add(new GraphicModel("Test " + i));
            }
        }

    }
}