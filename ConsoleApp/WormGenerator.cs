using System;

namespace ConsoleApp
{
    public static class WormGenerator
    {
        private static readonly Random Random = new();
        private static int NextNormal(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var randNormal = mu + sigma * randStdNormal;
            return (int)Math.Round(randNormal);
        }
        private static int _counter;
        private static string GetRandomName()
        {
            return $"Worm-{_counter++}";
        }

        public static Coords GetRandomCoords()
        {
            return new Coords(Random.NextNormal(0, 5), Random.NextNormal(0, 5));
        }
        
        public static IObject GetRandomNewWorm()
        {
            return GetNewWorm(GetRandomName(), GetRandomCoords());
        }

        public static IObject GetNewWorm(string name, Coords coords)
        {
            return new Worm(name, coords);
        }

        public static IObject GetNewWorm(Coords coords)
        {
            return new Worm(GetRandomName(), coords);
        }
    }
}