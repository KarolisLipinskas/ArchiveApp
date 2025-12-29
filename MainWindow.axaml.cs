using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace ArchiveApp;

public partial class MainWindow : Window
{
    public ObservableCollection<string> Tabs { get; } = [InitialTab];
    private int _nextTabNo = 2;
    private const string InitialTab = "Tab1";

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
        catch {  /* IGNORED */ }
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
}