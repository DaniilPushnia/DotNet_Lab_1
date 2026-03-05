# Лабораторна робота №1: Поліморфізм та зв'язування в .NET

**Виконав:** Пушня Даниїл 
**Варіант:** 2  
**Тема:** Обчислення об'єму геометричних тіл (Куля, Конус, Усічений конус)

---

## Опис завдання
1. Описати базовий абстрактний клас `GeometricBody`.
2. Реалізувати інтерфейс для обчислення об'єму за допомогою абстрактних методів.
3. Перевизначити метод `Equals` для порівняння значень (об'ємів), а не посилань.
4. Продемонструвати всі типи конструкторів (за замовчуванням, з параметрами, копіювання).
5. Показати різницю між **раннім** та **пізнім** зв'язуванням.

---

## Код програми

```csharp
using System;
using System.Collections.Generic;
using System.Text;

// Базовий абстрактний клас
abstract class GeometricBody
{
    public virtual string BodyType => "Геометричне тіло";

    public abstract double GetVolume();

    public GeometricBody() { }
    public GeometricBody(GeometricBody other) { }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        GeometricBody other = (GeometricBody)obj;
        return Math.Abs(this.GetVolume() - other.GetVolume()) < 0.001;
    }

    public override int GetHashCode() => GetVolume().GetHashCode();

    // Для раннього зв'язування
    public string GetInfoNonVirtual() => "Виклик через базовий тип (Early Binding)";
}

class Sphere : GeometricBody
{
    public double Radius { get; set; }
    public override string BodyType => "Куля";

    public Sphere(double radius) { Radius = radius; }
    public override double GetVolume() => (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
}

class Cone : GeometricBody
{
    public double Radius { get; set; }
    public double Height { get; set; }
    public override string BodyType => "Конус";

    public Cone(double r, double h) { Radius = r; Height = h; }
    public override double GetVolume() => (1.0 / 3.0) * Math.PI * Radius * Radius * Height;
}

class TruncatedCone : GeometricBody
{
    public double R { get; set; }
    public double r { get; set; }
    public double H { get; set; }
    public override string BodyType => "Усічений конус";

    public TruncatedCone(double R, double r, double H) { this.R = R; this.r = r; this.H = H; }
    public override double GetVolume() => (1.0 / 3.0) * Math.PI * H * (R * R + R * r + r * r);
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<GeometricBody> bodies = new List<GeometricBody>
        {
            new Sphere(5),
            new Cone(3, 7),
            new TruncatedCone(5, 2, 4),
            new Sphere(5)
        };

        foreach (var body in bodies)
        {
            Console.WriteLine($"Фігура: {body.BodyType,-15} | Об'єм: {body.GetVolume():F2}");
        }

        Console.WriteLine("\n--- Перевірка Equals ---");
        Console.WriteLine($"Куля 1 == Куля 2? Відповідь: {bodies[0].Equals(bodies[3])}");

        Console.WriteLine("\n--- Зв'язування ---");
        GeometricBody poly = new Sphere(10);
        Console.WriteLine($"Пізнє (virtual): {poly.BodyType}");
        Console.WriteLine($"Раннє (non-virtual): {poly.GetInfoNonVirtual()}");
    }
}