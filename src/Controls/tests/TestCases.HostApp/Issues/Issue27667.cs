using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Handlers.Items2; // Added import for CollectionView2
using System.ComponentModel;
using System.Diagnostics;

namespace Maui.Controls.Sample.Issues;

[XamlCompilation(XamlCompilationOptions.Compile)]
[Issue(IssueTracker.Github, 27667, "CollectionView with Horizontal GridItemsLayout Becomes Unresponsive with Increased Column Span Updates on iOS Platform", PlatformAffected.iOS)]
public partial class Issue27667 : ContentPage, INotifyPropertyChanged
{
    public int _columnCount { get; set; } = 3;
    
    public static int MeasureInvalidatedCalled { get; private set; } = 0;
    
    private Label _measureLabelText;
    private Label _measureCountValue;
    private StackLayout _instructionsLayout;

    public Issue27667()
    {
        MeasureInvalidatedCalled = 0;
        var instructionsLayout = new StackLayout();
        instructionsLayout.Children.Add(new Label
        {
            Text = "1. The test passes if you can update the Items correctly after clicking the button."
        });
        
        var measureStack = new StackLayout 
        { 
            Orientation = StackOrientation.Horizontal,
            Spacing = 5
        };
        
        _measureLabelText = new Label 
        { 
            Text = "MeasureInvalidated count:",
            VerticalOptions = LayoutOptions.Center
        };
        
        _measureCountValue = new Label 
        { 
            Text = "0",
            AutomationId = "MeasureCountValue",
            VerticalOptions = LayoutOptions.Center
        };
        
        measureStack.Children.Add(_measureLabelText);
        measureStack.Children.Add(_measureCountValue);
        
        instructionsLayout.Children.Add(measureStack);

        var gridItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Horizontal);
        gridItemsLayout.SetBinding(GridItemsLayout.SpanProperty, new Binding(nameof(_columnCount), source: this));

        var collectionView = new CollectionView2
        {
            ItemsLayout = gridItemsLayout,
            AutomationId = "CollectionViewWithGrid",
            ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label { TextColor = Colors.Black };
                label.SetBinding(Label.TextProperty, new Binding("."));
                return label;
            }),
            ItemsSource = new[]
            {
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5",
                "Item 6", "Item 7", "Item 8", "Item 9"
            }
        };

        collectionView.MeasureInvalidated += (s, e) =>
        {
            MeasureInvalidatedCalled++;
            Dispatcher.Dispatch(() => {
                _measureCountValue.Text = MeasureInvalidatedCalled.ToString();
            });
        };

        MyCollectionView = collectionView;

        var updateButton = new Button
        {
            Text = "UpdateColumns",
            AutomationId = "UpdateColumnsButton"
        };
        updateButton.Clicked += UpdateColumnCount;

        var mainGrid = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = 48 }
            }
        };

        Grid.SetRow(instructionsLayout, 0);
        Grid.SetRow(collectionView, 1);
        Grid.SetRow(updateButton, 2);

        mainGrid.Children.Add(instructionsLayout);
        mainGrid.Children.Add(collectionView);
        mainGrid.Children.Add(updateButton);

        _instructionsLayout = instructionsLayout;
        Content = mainGrid;
    }

    private void UpdateColumnCount(object sender, EventArgs e)
    {
        _columnCount++;
		MeasureInvalidatedCalled = 0;
        Dispatcher.Dispatch(() =>
        {
            OnPropertyChanged(nameof(_columnCount));
        });
    }

    public CollectionView MyCollectionView { get; set; }

}