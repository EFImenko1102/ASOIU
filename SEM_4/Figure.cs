using System;

namespace seminar_4;

internal abstract class Figure(string type) : IComparable
{
    public string Type { get; } = type;

    public abstract double Area { get; }

    public override string ToString() => $"{Type} площадью {Area}";

    public int CompareTo(object? obj) =>
        obj is Figure other
            ? Area.CompareTo(other.Area)
            : throw new ArgumentException("Объект не является фигурой");
}

internal class Circle(double radius) : Figure("Круг")
{
    public override double Area => Math.PI * radius * radius;
}

internal class Rectangle(double height, double width, string type = "Прямоугольник") : Figure(type)
{
    public override double Area => width * height;
}

internal class Square(double size) : Rectangle(size, size, "Квадрат");
