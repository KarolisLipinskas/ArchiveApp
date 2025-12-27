using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ArchiveApp;

public partial class MainWindow : Window
{
    public ObservableCollection<string> Tabs { get; } = ["Tab1"];
    private int _counter = 2;
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void NewTabButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Tabs.Add($"Tab{_counter++}");
    }

    private void CloseTabButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: string tab })
        {
            Tabs.Remove(tab);
        }
    }
}