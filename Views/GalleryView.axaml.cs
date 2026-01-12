using Avalonia.Controls;
using Avalonia.Input;

namespace ArchiveApp.Views;

public partial class GalleryView : UserControl
{
    public GalleryView()
    {
        InitializeComponent();
    }

    private void Item_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Image image) return;
        if (image.Source == null) return;
        
        if (DataContext is MainWindow mainWindow)
        {
            mainWindow.ItemSource = image.Source;
            mainWindow.ShowItem = true;
            
            var ratio = image.Source.Size.Width / image.Source.Size.Height;
            switch (ratio)
            {
                case < 0.5:
                    mainWindow.ImageVerticalScroll = true;
                    break;
                case > 3.5:
                    mainWindow.ImageHorizontalScroll = true;
                    break;
                default:
                    mainWindow.ImageVerticalScroll = false;
                    mainWindow.ImageHorizontalScroll = false;
                    break;
            }
            mainWindow.ImageScroll = mainWindow.ImageVerticalScroll || mainWindow.ImageHorizontalScroll;
        }
    }
}