using System;

namespace ConsoleApp
{
    
    public class WormGenerator : IWormGenerator
    {
        

        private int _counter;

        private string GetRandomName()
        {
            return $"Worm-{_counter++}";
        }

        public static IObject GetNewWorm(string name, Coords coords)
        {
            return new Worm(name, coords);
        }

        public IObject GetNewWorm(Coords coords)
        {
            return GetNewWorm(GetRandomName(), coords);
        }
    }

    public interface IWormGenerator
    {
        public IObject GetNewWorm(Coords coords);
    }
}