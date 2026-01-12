using System;
using Avalonia;
using Avalonia.Controls;

namespace ArchiveApp.Classes;

public class GalleryGrid : Panel
{
    public static readonly StyledProperty<int> RowsProperty =
        AvaloniaProperty.Register<GalleryGrid, int>(nameof(Rows), 1);

    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<GalleryGrid, int>(nameof(Columns), 1);

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        Console.WriteLine("measure: " + availableSize);
        var cellWidth = availableSize.Width / Columns;
        var cellHeight = availableSize.Height / Rows;
        Console.WriteLine("Cell Width: " + cellWidth + " Height: " + cellHeight);

        foreach (var child in Children)
        {
            child.Measure(new Size(cellWidth, cellHeight));
        }

        return availableSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var cellWidth = finalSize.Width / Columns;
        var cellHeight = finalSize.Height / Rows;

        for (int i = 0; i < Children.Count; i++)
        {
            int row = i / Columns;
            int column = i % Columns;

            if (row >= Rows)
                break;

            var rect = new Rect(
                column * cellWidth,
                row * cellHeight,
                cellWidth,
                cellHeight);

            Children[i].Arrange(rect);
        }

        return finalSize;
    }
}