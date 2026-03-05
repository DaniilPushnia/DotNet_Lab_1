using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryLab
{
    // 1. Базовий абстрактний клас
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

        public string GetInfoEarly() => "Виклик через базовий тип (Early Binding)";
    }

    // 2. Клас: Куля
    class Sphere : GeometricBody
    {
        public double Radius { get; set; }
        public override string BodyType => "Куля";

        public Sphere() : base() { Radius = 0; }
        public Sphere(double radius) : base() { Radius = radius; }
        public Sphere(Sphere other) : base(other) { Radius = other.Radius; }

        public override double GetVolume() => (4.0 / 3.0) * Math.PI * Math.Pow(Radius, 3);
    }

    // 3. Клас: Конус
    class Cone : GeometricBody
    {
        public double Radius { get; set; }
        public double Height { get; set; }
        public override string BodyType => "Конус";

        public Cone() : base() { }
        public Cone(double r, double h) : base() { Radius = r; Height = h; }
        public Cone(Cone other) : base(other) { Radius = other.Radius; Height = other.Height; }

        public override double GetVolume() => (1.0 / 3.0) * Math.PI * Radius * Radius * Height;
    }

    // 4. Клас: Усічений конус
    class TruncatedCone : GeometricBody
    {
        public double R { get; set; }
        public double r { get; set; }
        public double H { get; set; }
        public override string BodyType => "Усічений конус";

        public TruncatedCone() : base() { }
        public TruncatedCone(double R, double r, double H) : base()
        {
            this.R = R; this.r = r; this.H = H;
        }
        public TruncatedCone(TruncatedCone other) : base(other)
        {
            this.R = other.R; this.r = other.r; this.H = other.H;
        }

        public override double GetVolume() => (1.0 / 3.0) * Math.PI * H * (R * R + R * r + r * r);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<GeometricBody> bodies = new List<GeometricBody>
            {
                new Sphere(5),
                new Cone(3, 7),
                new TruncatedCone(5, 2, 4),
                new Sphere(5) 
            };

            Console.WriteLine("=== 1. Демонстрація поліморфізму (Об'єми) ===");
            foreach (var body in bodies)
            {
                Console.WriteLine($"Фігура: {body.BodyType,-15} | Об'єм: {body.GetVolume():F2}");
            }

            Console.WriteLine("\n=== 2. Перевірка методу Equals (по значенню) ===");
            bool result = bodies[0].Equals(bodies[3]);
            Console.WriteLine($"Куля 1 (R=5) == Куля 2 (R=5)? Відповідь: {result}");

            Console.WriteLine("\n=== 3. Раннє та пізнє зв'язування ===");

            GeometricBody poly = new Sphere(10);
            
            // ПІЗНЄ (Late Binding) - через override/virtual
            Console.WriteLine($"Пізнє (virtual property): {poly.BodyType}");

            // РАННЄ (Early Binding) - через звичайний метод базового класу
            Console.WriteLine($"Раннє (non-virtual method): {poly.GetInfoEarly()}");

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }
    }
}
