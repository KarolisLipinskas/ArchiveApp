using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace ArchiveApp;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    public ObservableCollection<string> Tabs { get; } = [InitialTab];
    private int _nextTabNo = 2;
    private const string InitialTab = "Tab1";
    
    private bool _showMoreControls;
    public bool ShowMoreControls
    {
        get => _showMoreControls;
        set
        {
            _showMoreControls = value;
            OnPropertyChanged(nameof(ShowMoreControls));
        }
    }
    
    private IImage? _itemSource;
    public IImage? ItemSource
    {
        get => _itemSource;
        set
        {
            _itemSource = value;
            OnPropertyChanged(nameof(ItemSource));
        }
    }

    private bool _showItem;
    public bool ShowItem
    {
        get => _showItem;
        set
        {
            _showItem = value;
            OnPropertyChanged(nameof(ShowItem));
        }
    }

    private bool _imageVerticalScroll;
    public bool ImageVerticalScroll
    {
        get => _imageVerticalScroll;
        set
        {
            _imageVerticalScroll = value;
            OnPropertyChanged(nameof(ImageVerticalScroll));
        }
    }
    
    private bool _imageHorizontalScroll;
    public bool ImageHorizontalScroll
    {
        get => _imageHorizontalScroll;
        set
        {
            _imageHorizontalScroll = value;
            OnPropertyChanged(nameof(ImageHorizontalScroll));
        }
    }

    private bool _imageScroll;
    public bool ImageScroll
    {
        get => _imageScroll;
        set
        {
            _imageScroll = value;
            OnPropertyChanged(nameof(ImageScroll));
        }
    }

    public ObservableCollection<Bitmap> Images { get; } =
    [
        new Bitmap(@"C:\Users\karol\Pictures\desktop backgrounds\planets-space-person-stars.jpg"),
        new Bitmap(@"C:\Users\karol\Pictures\UbisoftConnect\Trackmania\Trackmania2021-9-7-23-35-54.jpg"),
        new Bitmap(@"C:\Users\karol\Pictures\UbisoftConnect\Trackmania\Trackmania2022-1-6-22-29-56.jpg"),
        new Bitmap(@"C:\Users\karol\Pictures\Simplanova 2024-10-03  (4).jpg")
    ];
    
    public ObservableCollection<string> Tags { get; } = ["Test1", "opa", "Test2", "OK", "Space", "hh", "mm", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
    
    private static async void StartAsyncUIThread(Action callback, DispatcherPriority priority)
    {
        try
        {
            await Dispatcher.UIThread.InvokeAsync(callback, priority);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private void ScrollTabsToEnd()
    {
        TabScrollViewer.Offset = new Vector(
            Math.Max(0, TabScrollViewer.Extent.Width - TabScrollViewer.Viewport.Width),
            0);
    }
    
    private void RestoreScrollPosition(double x)
    {
        TabScrollViewer.Offset = new Vector(
            Math.Max(0, Math.Min(x, TabScrollViewer.Extent.Width)),
            0);
    }

    private void NewTabButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var newTab = "Tab" + _nextTabNo;
        _nextTabNo++;
        Tabs.Add(newTab);
        TabControl.SelectedItem = newTab;
        
        StartAsyncUIThread(ScrollTabsToEnd, DispatcherPriority.Background);
    }

    private void CloseTabButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: string tab }) return;

        if (Tabs.Count == 1)
        {
            _nextTabNo = 2;
            Tabs.Remove(tab);
            Tabs.Add(InitialTab);
            TabControl.SelectedItem = InitialTab;
            return;
        }
        
        var scrollX = TabScrollViewer.Offset.X;
        
        var index = Tabs.IndexOf(tab);
        Tabs.Remove(tab);
        TabControl.SelectedItem = (index == Tabs.Count) ? Tabs.Last() : Tabs[index];
        
        StartAsyncUIThread(() => RestoreScrollPosition(scrollX), DispatcherPriority.Background);
    }

    private void TabScrollBar_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer) return;
        
        scrollViewer.Offset = scrollViewer.Offset.WithX(scrollViewer.Offset.X - e.Delta.Y * 40);
        e.Handled = true;
    }

    private void MoreControlsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ShowMoreControls = true;
    }
    
    private void HideControlsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ShowMoreControls = false;
    }

    private void ClearDateFromButton_OnClick(object? sender, RoutedEventArgs e)
    {
         FromDatePicker.Clear();
    }

    private void ClearDateToButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ToDatePicker.Clear();
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ShowItem)
        {
            ItemSource = null;
            ShowItem = false;
            ViewsItemView.ZoomSlider.Value = 50.0;
        }
    }
}