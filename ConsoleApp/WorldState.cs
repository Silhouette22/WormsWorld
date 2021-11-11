using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class WorldState : IEnumerable
    {
        private readonly Dictionary<Coords, IObject> _map = new();

        public bool AddObject(IObject obj)
        {
            if (_map.ContainsKey(obj.Coords)) return false;
            _map.Add(obj.Coords, obj);
            return true;
        }

        public bool AddFood()
        {
            var food = FoodGenerator.GetFood();
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

        public bool ContainsCoords(Coords coords)
        {
            return _map.ContainsKey(coords);
        }

        public IEnumerable<T> Select<T>()
        {
            return _map.Select(i => i.Value).Where(obj => obj is T).Cast<T>();
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

    public interface IObject
    {
        public Coords Coords { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HP { get; }

        public void LoseHP(out bool isDead);
        public Action AskForAction(WorldState state);
    }

    public class Food : IObject
    {
        public Food(Coords coords) => Coords = coords;
        public Coords Coords { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HP { get; private set; } = Constants.BaseHP;

        public void LoseHP(out bool isDead)
        {
            isDead = (HP -= Constants.LostHP) <= 0;
        }

        public Action AskForAction(WorldState state)
        {
            return Actions.DoNothing;
        }

        public override string ToString()
        {
            return $"{HP}HP{Coords}";
        }
    }
}