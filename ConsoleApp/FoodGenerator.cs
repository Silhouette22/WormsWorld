using System;

namespace ConsoleApp
{
    public class FoodGenerator : IFoodGenerator
    {
        private readonly Random _random = new();
        private Coords GetRandomCoords()
        {
            return new Coords(_random.NextNormal(0, 5), _random.NextNormal(0, 5));
        }

        public IObject GetFood()
        {
            return new Food(GetRandomCoords());
        }
    }

    public interface IFoodGenerator
    {
        public IObject GetFood();
    }
}