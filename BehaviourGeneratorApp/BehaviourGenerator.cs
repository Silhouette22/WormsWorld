using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp;

namespace BehaviourGeneratorApp
{
    public class BehaviourGenerator : IBehaviourGenerator
    {
        private readonly Dictionary<Coords, int> _needToWaitBeforeGeneratingHere = new();
        private readonly Random _random = new();
        private readonly INameProvider _nameProvider;

        public BehaviourGenerator(INameProvider nameProvider)
        {
            _nameProvider = nameProvider;
        }

        public Behaviour GenerateBehaviour(int turns)
        {
            var behaviour = new Behaviour(_nameProvider.GetName());
            for (var i = 0; i < turns; i++)
            {
                Coords coords;
                do
                {
                    coords = GetRandomCoords();
                } while (!CanGenerateHere(coords));
                
                behaviour.TurnToCoord[i] = coords;
                ProhibitGenerationHere(coords);
                UpdateProhibitions();
            }

            return behaviour;
        }

        private bool CanGenerateHere(Coords coords)
        {
            return !_needToWaitBeforeGeneratingHere.ContainsKey(coords);
        }

        private void ProhibitGenerationHere(Coords coords)
        {
            _needToWaitBeforeGeneratingHere[coords] = Constants.BaseHP;
        }

        private void UpdateProhibitions()
        {
            foreach (var coords in _needToWaitBeforeGeneratingHere.Keys.Reverse())
            {
                if (_needToWaitBeforeGeneratingHere[coords] > 0)
                {
                    _needToWaitBeforeGeneratingHere[coords]--;
                }
                else
                {
                    _needToWaitBeforeGeneratingHere.Remove(coords);
                }
            }
        }

        private Coords GetRandomCoords()
        {
            return new Coords(_random.NextNormal(0, 5), _random.NextNormal(0, 5));
        }
    }

    public interface IBehaviourGenerator
    {
        Behaviour GenerateBehaviour(int turns = 100);
    }
}