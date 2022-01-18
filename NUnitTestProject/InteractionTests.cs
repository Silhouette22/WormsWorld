using ActionProviderLib;
using NUnit.Framework;
using ObjectsLib;
using WorldStateLib;
using WormGeneratorLib;

namespace NUnitTestProject
{
    [TestFixture]
    public class InteractionTests
    {
        [Test(TestOf = typeof(WormGenerator))]
        public void NameGeneratorTest()
        {
            IWormGenerator generator = new WormGenerator();
            var worm1 = generator.GetNewWorm(new Coords(0, 0)) as Worm;
            var worm2 = generator.GetNewWorm(new Coords(0, 0)) as Worm;
            Assert.AreNotEqual(worm1!.Name, worm2!.Name);
        }

        [Test(TestOf = typeof(WorldState))]
        public void FoodBusyCellGenerationTest()
        {
            var coords = new Coords(0, 0);

            var state = new WorldState(new WormGenerator());
            state.AddWorm(coords);
            
            Assert.IsTrue(state.TryGetObject(coords, out var obj));
            var worm = obj as Worm;
            var pastHP = worm!.HP;
            //Inserted
            Assert.IsTrue(state.AddFood(new Food(coords)));
            Assert.AreEqual(pastHP + Constants.AdditionalHP, worm.HP);
        }
        
        [Test(TestOf = typeof(WorldState))]
        public void FoodUniqueCellGenerationTest()
        {
            var coords = new Coords(0, 0);

            var state = new WorldState(new WormGenerator());
            state.AddFood(new Food(coords));
            
            //Not inserted because of busy cell
            Assert.IsFalse(state.AddFood(new Food(coords)));
        }

        [Test(TestOf = typeof(ActionProvider))]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void LogicTest(int x, int y)
        {
            var direction = new Coords(x, y);
            IActionProvider actionProvider = new ActionProvider();
            var action1 = GetMove(direction);
            var state = new WorldState(new WormGenerator());

            state.AddWorm(new Coords(0, 0));
            state.AddFood(new Food(direction));

            Assert.IsTrue(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm = obj as Worm;
            
            var action2 = actionProvider.GetAction(state, worm);
            
            Assert.AreEqual(action1, action2);
            
         // TODO: Multiply tests   
        }
        
        private Action GetMove(Coords direction)
        {
            Action action = direction switch
            {
                (0, 1) => Actions.MoveUp,
                (1, 0) => Actions.MoveRight,
                (0, -1) => Actions.MoveDown,
                (-1, 0) => Actions.MoveLeft,
                _ => Actions.DoNothing
            };
            return action;
        }
    }
}