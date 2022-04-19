using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using AdvancedRandomLib;
using ObjectsLib;
using WormGeneratorLib;

namespace WorldStateLib
{
    public class WorldState : IEnumerable
    {
        private readonly IDictionary<Coords, IObject> _map = new Dictionary<Coords, IObject>();
        private readonly Random _random = new();
        private readonly IWormGenerator _wormGenerator;

        public WorldState(IWormGenerator wormGenerator)
        {
            _wormGenerator = wormGenerator;
        }

        [JsonConstructor]
        public WorldState(IDictionary<Coords, IObject> state)
        {
            _map = state;
        }

        public WorldStateDto ToDto(Worm worm)
        {
            return new WorldStateDto(worm, Select<Worm>(), Select<Food>());
        }

        public bool AddObject(IObject obj)
        {
            if (_map.ContainsKey(obj.Coords)) return false;
            _map.Add(obj.Coords, obj);
            return true;
        }

        public bool AddWorm(Coords coords)
        {
            return AddObject(_wormGenerator.GetNewWorm(coords));
        }

        private Coords GetRandomCoords()
        {
            return new Coords(_random.NextNormal(0, 5), _random.NextNormal(0, 5));
        }

        public bool AddWorm()
        {
            return AddObject(_wormGenerator.GetNewWorm(GetRandomCoords()));
        }

        public bool AddFood(IObject food)
        {
            if (_map.TryGetValue(food.Coords, out var value))
            {
                if (value is not Worm worm) return false;
                worm.Eat();
                return true;
            }
            _map.Add(food.Coords, food);
            return true;
        }

        public IObject GetAndRemoveObject(Coords coords)
        {
            _map.Remove(coords, out var obj);
            return obj;
        }

        public bool RemoveObject(Coords coords)
        {
            return _map.Remove(coords);
        }

        public bool TryGetObject(Coords coords, out IObject value)
        {
            return _map.TryGetValue(coords, out value);
        }

        public bool TryGetWormByName(string name, out Worm worm)
        {
            try
            {
                worm = Select<Worm>().First(w => w.Name == name);
            }
            catch (InvalidOperationException ex)
            {
                worm = null;
                return false;
            }

            return true;
        }

        public bool ContainsCoords(Coords coords)
        {
            return _map.ContainsKey(coords);
        }

        public IEnumerable<T> Select<T>()
        {
            // return _map.Select(i => i.Value).Where(obj => obj is T).Cast<T>();
            return _map.Select(i => i.Value).OfType<T>();
        }

        public IObject this[Coords coords]
        {
            get => _map[coords];
            set => _map[coords] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        public IEnumerable ReverseObjects()
        {
            return _map.Values.Reverse();
        }

        public override string ToString()
        {
            var worms = Select<Worm>().ToList();
            var food = Select<Food>().ToList();
            return new StringBuilder($"[Objects on a map:  Worms:{worms.Count}: [")
                .AppendJoin(", ", worms).Append($"] || Food:{food.Count}: [")
                .AppendJoin(", ", food).Append("]]").ToString();
        }
    }
}