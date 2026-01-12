using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ArchiveApp.Views;

public partial class ItemView : UserControl
{
    private double _imageWidth;

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

        VerticalImage.Width = _imageWidth;
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
}