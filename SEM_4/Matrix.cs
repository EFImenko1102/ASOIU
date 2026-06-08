using System;
using System.Collections.Generic;
using System.Text;

namespace seminar_4;

public interface IMatrixCheckEmpty<T>
{
    T GetEmptyElement();
    bool CheckEmptyElement(T element);
}

internal class FigureMatrixCheckEmpty : IMatrixCheckEmpty<Figure>
{
    public Figure GetEmptyElement() => null!;
    public bool CheckEmptyElement(Figure element) => element is null;
}

public class Matrix<T>(int maxX, int maxY, IMatrixCheckEmpty<T> checkEmpty)
{
    private readonly Dictionary<(int x, int y), T> _matrix = [];

    public T this[int x, int y]
    {
        get
        {
            CheckBounds(x, y);
            return _matrix.TryGetValue((x, y), out var element) ? element : checkEmpty.GetEmptyElement();
        }
        set
        {
            CheckBounds(x, y);
            _matrix[(x, y)] = value;
        }
    }

    private void CheckBounds(int x, int y)
    {
        if (x < 0 || x >= maxX)
            throw new ArgumentOutOfRangeException(nameof(x), $"x={x} выходит за границы");
        if (y < 0 || y >= maxY)
            throw new ArgumentOutOfRangeException(nameof(y), $"y={y} выходит за границы");
    }

    public int ColumnWidth { get; set; } = 32;

    public override string ToString()
    {
        var b = new StringBuilder();
        for (int j = 0; j < maxY; j++)
        {
            b.Append('|');
            for (int i = 0; i < maxX; i++)
            {
                string cell = !checkEmpty.CheckEmptyElement(this[i, j]) ? $"{this[i, j]}" : "-";
                b.Append(cell.PadRight(ColumnWidth));
                b.Append('|');
            }
            b.AppendLine();
        }
        return b.ToString();
    }
}
