//using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Collections.ObjectModel;
using TestZone.Test.Views.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestZone.Test.Models
{
    public class ScrollTestModel : ITitled, INotifyPropertyChanged
    {
        private SKColor BackgroundColor { get; set; }

        private string _Name { get; set; }
        public string Name { get { return _Name; } set { _Name = value; OnPropertyChanged(); } }

        public ScrollTestModel(string name)
        {
            this.BackgroundColor = SKColors.Green;
            Name = name;
            InitView();
        }
     //      public ObservableCollection<CompetenceLine> CompetenceLines { get;  } = new ObservableCollection<CompetenceLine>();
        private void InitView()
        {
            //COPIER LE CODE POUR TESTER A PART
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
