namespace ConsoleApp
{
    public delegate void Action(IObject obj, WorldState state);

    public static class Direction
    {
        public static readonly (int x, int y) Up = (0, 1);
        public static readonly (int x, int y) Down = (0, -1);
        public static readonly (int x, int y) Left = (-1, 0);
        public static readonly (int x, int y) Right = (1, 0);
    }

    public static class Actions
    {
        private static bool CheckIfCanMoveTo(WorldState state, Coords coords, out bool isThereFood)
        {
            //if there's nothing, can move
            if (!state.TryGetObject(coords, out var neighbor)) return true | (isThereFood = false);
            //if there's something, except food, can not move
            if (neighbor is not Food) return isThereFood = false;
            //if there's food, can move
            var food = state.GetAndRemoveObject(coords);
            return true | (isThereFood = food.HP > 0);
        }

        private static void MakeMove(WorldState state, Coords coords, (int, int) direction, bool eat)
        {
            var o = (Worm) state.GetAndRemoveObject(coords);
            o.Coords += direction;
            if (eat) o.Eat();
            state.AddObject(o);
        }

        private static void MoveTemplate(IObject obj, WorldState state, (int, int) direction)
        {
            if (obj is not Worm) return;
            if (CheckIfCanMoveTo(state, obj.Coords + direction, out var isThereFood))
            {
                MakeMove(state, obj.Coords, direction, isThereFood);
            }
        }

        private static void DoMultiply(WorldState state, Coords coords, (int, int) direction)
        {
            var obj = (Worm) state[coords];
            obj.LoseHPDuringMultiply();
            state.AddWorm(obj.Coords + direction);
        }

        private static void MultiplyTemplate(IObject obj, WorldState state, (int, int) direction)
        {
            if (obj is not Worm) return;
            if (CheckIfCanMoveTo(state, obj.Coords + direction, out var isThereFood) && !isThereFood)
            {
                if (obj.HP > Constants.MultiplyConsumeHP)
                {
                    DoMultiply(state, obj.Coords, direction);
                }
            }
        }

        public static void MoveUp(IObject obj, WorldState state)
        {
            MoveTemplate(obj, state, Direction.Up);
        }

        public static void MoveDown(IObject obj, WorldState state)
        {
            MoveTemplate(obj, state, Direction.Down);
        }

        public static void MoveLeft(IObject obj, WorldState state)
        {
            MoveTemplate(obj, state, Direction.Left);
        }

        public static void MoveRight(IObject obj, WorldState state)
        {
            MoveTemplate(obj, state, Direction.Right);
        }

        public static void MultiplyUp(IObject obj, WorldState state)
        {
            MultiplyTemplate(obj, state, Direction.Up);
        }

        public static void MultiplyDown(IObject obj, WorldState state)
        {
            MultiplyTemplate(obj, state, Direction.Down);
        }

        public static void MultiplyLeft(IObject obj, WorldState state)
        {
            MultiplyTemplate(obj, state, Direction.Left);
        }

        public static void MultiplyRight(IObject obj, WorldState state)
        {
            MultiplyTemplate(obj, state, Direction.Right);
        }

        public static void DoNothing(IObject obj, WorldState state)
        {
        }
    }
}