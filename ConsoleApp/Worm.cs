using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    public class Worm : IObject
    {
        public Worm(string name, int x, int y) : this(name, new Coords(x, y))
        {
        }

        public Worm(string name, Coords coords)
        {
            Name = name;
            Coords = coords;
        }

        public string Name { get; }

        public Coords Coords { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HP { get; private set; } = Constants.BaseHP;

        public void Eat()
        {
            HP += Constants.AdditionalHP;
        }

        public void LoseHP(out bool isDead)
        {
            isDead = (HP -= Constants.LostHP) <= 0;
        }

        public void LoseHPDuringMultiply()
        {
            HP -= Constants.MultiplyConsumeHP;
        }

        public Action AskForAction(WorldState state)
        {
            //TODO Logic!
            //TODO Step 1. Basic logic. //DONE
            int Distance(IObject obj1, IObject obj2)
            {
                return Math.Abs(obj1.Coords.X - obj2.Coords.X) + Math.Abs(obj1.Coords.Y - obj2.Coords.Y);
            }
            //TODO Step 2. Enhanced multiply logic //TBD
            if (this.HP > Constants.MultiplyConsumeHP) return Actions.MultiplyUp;
            var food = state.Select<Food>().ToList();
            // if (food is null || !food.Any()) return Actions.DoNothing;
            IObject o = food.First();
            (Coords coords, int distance) f = (o.Coords, Distance(this, o));
            foreach (var fo in food)
            {
                var d = Distance(this, fo);
                if (d < f.distance)
                {
                    f = (fo.Coords, d);
                }
            }

            if (f.distance > HP)
            {
                f.coords = new Coords(0, 0);
            }
            return
                f.coords.X < Coords.X ? Actions.MoveLeft :
                f.coords.X > Coords.X ? Actions.MoveRight :
                f.coords.Y < Coords.Y ? Actions.MoveDown :
                f.coords.Y > Coords.Y ? Actions.MoveUp :
                Actions.DoNothing;
        }

        public override string ToString()
        {
            return $"{Name}|{HP}HP{Coords}";
        }
    }
}