using System;
using System.Linq;
using System.Threading.Tasks;
using ObjectsLib;
using WorldStateLib;

namespace ActionProviderLib
{
    public class ActionProvider : IActionProvider
    {
        public async Task<Action> GetAction(WorldState state, IObject obj)
        {
            return await Task.Run(() => Get(state, obj));
        }

        public Action Get(WorldState state, IObject obj)
        {
            if (obj is Food) return Actions.DoNothing;
            
            //TODO Logic!
            //TODO Step 1. Basic logic. //DONE
            int Distance(IObject obj1, IObject obj2)
            {
                return Math.Abs(obj1.Coords.X - obj2.Coords.X) + Math.Abs(obj1.Coords.Y - obj2.Coords.Y);
            }
            //TODO Step 2. Enhanced multiply logic //TBD
            if (obj.HP > Constants.MultiplyConsumeHP) return Actions.MultiplyUp;
            var food = state.Select<Food>().ToList();
            if (!food.Any()) return Actions.DoNothing;
            IObject o = food.First();
            (Coords coords, int distance) f = (o.Coords, Distance(obj, o));
            foreach (var fo in food)
            {
                var d = Distance(obj, fo);
                if (d < f.distance)
                {
                    f = (fo.Coords, d);
                }
            }

            if (f.distance > obj.HP)
            {
                f.coords = new Coords(0, 0);
            }
            return
                f.coords.X < obj.Coords.X ? Actions.MoveLeft :
                f.coords.X > obj.Coords.X ? Actions.MoveRight :
                f.coords.Y < obj.Coords.Y ? Actions.MoveDown :
                f.coords.Y > obj.Coords.Y ? Actions.MoveUp :
                Actions.DoNothing;
        }
    }
}