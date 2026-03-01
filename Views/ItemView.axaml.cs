using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ArchiveApp.Views;

public partial class ItemView : UserControl
{
    private double _imageWidth;
    private double _imageHeight;

    public ItemView()
    {
        InitializeComponent();
    }

    private void ZoomSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is not Slider slider) return;

        if (VerticalImage.Source != null)
        {
            CalculateImageWidth(VerticalImage.Source.Size.Width, slider.Value);
        }

        if (HorizontalImage.Source != null)  // TODO -- need to change how zooming and scrolling is done (on horizontal scroll performance issues)
        {
            CalculateImageHeight(HorizontalImage.Source.Size.Height, slider.Value);
        }

        VerticalImage.Width = _imageWidth;
        HorizontalImage.Height = _imageHeight;
    }

    private void CalculateImageWidth(double width, double sliderValue)
    {
        _imageWidth = sliderValue switch
        {
            < 10 => width * 0.2,
            >= 10 and < 50 => width * (0.02 * sliderValue),
            >= 50 => width * (1.0 + 0.02 * (sliderValue - 50)),
            _ => width * 1.0
        };
    }

    private void CalculateImageHeight(double height, double sliderValue) // TODO -- change zoom calculations
    {
        _imageHeight = sliderValue switch
        {
            < 10 => height * 0.2,
            >= 10 and < 50 => height * (0.02 * sliderValue),
            >= 50 => height * (1.0 + 0.02 * (sliderValue - 50)),
            _ => height * 1.0
        };
    }
}