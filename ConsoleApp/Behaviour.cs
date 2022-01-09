using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ConsoleApp
{
    public class Behaviour
    {
        public string Name { get; set; }

        public Dictionary<int, Coords> TurnToCoord { get; set; }

        public Behaviour(string name)
        {
            TurnToCoord = new Dictionary<int, Coords>();
            Name = name;
        }

        public BehaviourDto ToDto()
        {
            return new BehaviourDto
            {
                Name = Name,
                Behaviour = JsonSerializer.Serialize(TurnToCoord)
            };
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Behaviour) obj);
        }

        private bool Equals(Behaviour other)
        {
            bool equals = Name == other.Name;
            equals = equals && TurnToCoord.Count == other.TurnToCoord.Count;

            for (var i = 0; equals && i < TurnToCoord.Count; i++)
            {
                equals = TurnToCoord[i].Equals(other.TurnToCoord[i]);
            }
            return equals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TurnToCoord);
        }
    }
}