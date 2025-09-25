using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System.Diagnostics;
using TestZone.Test.Views.Models;

namespace TestZone.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSelector: StackLayout
    {
        
    
    
    
        public static readonly BindableProperty CarouselProperty = BindableProperty.Create(nameof(Carousel), typeof(CarouselView), typeof(CustomSelector), null, propertyChanged : InitCarouselOnChange);

        public CarouselView Carousel
        {
            get { return (CarouselView)GetValue(CarouselProperty); }
            set { SetValue(CarouselProperty, value); }
        }

        private static void InitCarouselOnChange(BindableObject view, object oldValue, object newValue)
        {
             CustomSelector _view = (CustomSelector)view;
            _view.Carousel = (CarouselView)newValue;

            if (_view.Carousel != null )
            {
                _view.Carousel.ScrollTo(0);
                _view.IsScrolling = true;

                _view.Carousel.Scrolled += (e, v) => {
                    ObservableCollection<ITitled> entries = (ObservableCollection<ITitled>)_view.Carousel.ItemsSource;
                        int currentIndex = entries.IndexOf((ITitled)_view.Carousel.CurrentItem);
                       _view._selectorAllCompetences.IsChecked = (currentIndex == 0);                    
                       _view.IsScrolling = false;
                };
                _view.Carousel.CurrentItemChanged += (e, v) =>
                {
                };
//                CurrentItemChanged = "CarouselViewItemChanged"
            }
        }

        private bool IsScrolling = false ;
        public CustomSelector()
        {
            InitializeComponent();
  //          BindingContext = this;
        }

        public void NextItem(object sender, EventArgs args)
        {
            if ( !Scrolling())
            {
                ObservableCollection<ITitled> entries = (ObservableCollection<ITitled>)Carousel.ItemsSource;
                ITitled currentItem = (ITitled)this.Carousel.CurrentItem;
                int currentIndex = entries.IndexOf((ITitled)this.Carousel.CurrentItem);
                int nextIndex = (currentIndex + 1) % entries.Count();

                ITitled nextItem = entries[nextIndex];

                setPositionCarousel(currentIndex, nextIndex, entries);
                this._selectorAllCompetences.IsChecked = (nextIndex == 0);

                this._selectorAllCompetences.IsEnabled = true;
            }                      
        }

        public void SetItem(String title, bool animate = true)
        {
            ObservableCollection<ITitled> entries = (ObservableCollection<ITitled>)Carousel.ItemsSource;
            foreach(ITitled entry in entries)
            {
                if(entry.Name == title)
                {
                    if (animate) { this.Carousel.ScrollTo(entries.IndexOf(entry)); }
                    else {
                        this.Carousel.CurrentItem = entry;
                    }
                    break;
                }
            }
        }

        private void setPositionCarousel(int currentIndex, int nextIndex, ObservableCollection<ITitled> entries)
        {

            IsScrolling = true;
            this.Carousel.ScrollTo(nextIndex);
        }

        private int GetIndexOfCurrentItem()
        {
            ObservableCollection<ITitled> entries = (ObservableCollection<ITitled>)Carousel.ItemsSource;
            ITitled currentItem = (ITitled)this.Carousel.CurrentItem;

            return entries.IndexOf((ITitled)this.Carousel.CurrentItem);
        }
        public void PreviousItem(object sender, EventArgs args)
        {
            if (!Scrolling()) {
                ObservableCollection<ITitled> entries = (ObservableCollection<ITitled>)Carousel.ItemsSource;
                int currentIndex = GetIndexOfCurrentItem();
                int nextIndex = currentIndex - 1 < 0 ? entries.Count() - 1 : currentIndex - 1;
                ITitled nextItem = (ITitled)entries[nextIndex];

                setPositionCarousel(currentIndex, nextIndex, entries);
            //On dirait que le carousel n'est pas actualisé            
            
            //AVEC CETTE LIGNE EN MOINS CA NE FONCTIONNE PLUS. PK???
                this._selectorAllCompetences.IsChecked = (nextIndex == 0);            
                this._selectorAllCompetences.IsEnabled = true;
            }

        }
        public void BackToFirst()
        {
            if(this.Carousel != null) {                
                if(GetIndexOfCurrentItem() != 0 && !Scrolling()) { 
                    IsScrolling = true;
                    this.Carousel.ScrollTo(0);
                }                               
            }
        }

        private bool Scrolling()
        {
            return IsScrolling || Carousel.IsDragging;
        }
    
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                BackToFirst();
            }
        }
    }
}