using System.Collections.Generic;
using System.Text.Json;
using ObjectsLib;

namespace BehaviourLib
{
    public class BehaviourDto
    {
        public string Name { get; set; }
        public string Behaviour { get; set; }

        public Behaviour ToBehaviour()
        {
            return new Behaviour(Name)
            {
                TurnToCoord = JsonSerializer.Deserialize<Dictionary<int, Coords>>(Behaviour)
            };
        }
    }
}