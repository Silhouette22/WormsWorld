using System;
using AdvancedRandomLib;
using BehaviourLib;
using BehaviourProviderLib;
using ObjectsLib;

namespace FoodGeneratorLib
{
    public class FoodGenerator : IFoodGenerator
    {
        private readonly Random _random = new();
        private readonly Behaviour? _behaviour;

        public FoodGenerator(IBehaviourProvider behaviourProvider)
        {
            _behaviour = behaviourProvider.GetBehaviour();
        }

        public FoodGenerator() {}

        public FoodGenerator(Behaviour behaviour)
        {
            _behaviour = behaviour;
        }

        private Coords GetRandomCoords()
        {
            return new Coords(_random.NextNormal(0, 5), _random.NextNormal(0, 5));
        }

        public IObject GetFood()
        {
            return GetFood(GetRandomCoords());
        }

        public IObject GetFood(Coords coords)
        {
            return new Food(coords);
        }

        public IObject GetFood(int turn)
        {
            return _behaviour switch
            {
                null => GetFood(),
                _ => GetFood(_behaviour.TurnToCoord[turn])
            };
        }
    }

    public interface IFoodGenerator
    {
        public IObject GetFood();

        public IObject GetFood(int turn);
    }
}