using System.Threading.Tasks;
using ActionProviderLib;
using ConsoleApp;
using FoodGeneratorLib;
using Moq;
using NUnit.Framework;
using ObjectsLib;
using WorldStateLib;
using WormGeneratorLib;

namespace NUnitTestProject
{
    [TestFixture(TestOf = typeof(Actions))]
    public class MovementTests
    {
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MoveToEmptyCellTest(int x, int y)
        {
            var direction = new Coords(x, y);

            var action = GetMove(direction);
            InitStabs(action, out var stabWormGenerator, out var stabActionProvider);

            var state = new WorldState(stabWormGenerator);
            state.AddWorm();
            Assert.True(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm = obj as Worm;

            var controller = new WorldController(state, stabActionProvider);
            controller.MakeStep();

            Assert.AreEqual(direction, worm!.Coords);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MoveToFoodCellTest(int x, int y)
        {
            var direction = new Coords(x, y);

            var action = GetMove(direction);
            InitStabs(action, out var stabWormGenerator, out var stabActionProvider);
            GetStabFoodGenerator(direction, out var stabFoodGenerator);

            var state = new WorldState(stabWormGenerator);
            state.AddWorm();
            Assert.True(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm = obj as Worm;

            state.AddFood(stabFoodGenerator.GetFood());

            var controller = new WorldController(state, stabActionProvider);
            controller.MakeStep();

            Assert.AreEqual(direction, worm!.Coords);

            Assert.AreEqual(Constants.BaseHP - Constants.LostHP + Constants.AdditionalHP, worm.HP);
            // Assert.Greater(worm.HP, Constants.BaseHP);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MoveToBusyCellTest(int x, int y)
        {
            var direction = new Coords(x, y);
            var action = GetMove(direction);

            var state = new WorldState(new WormGenerator());
            state.AddWorm( new Coords(0, 0));
            Assert.True(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm1 = obj as Worm;

            state.AddWorm(direction);
            action(worm1, state);

            //Did not move
            Assert.AreEqual(new Coords(0, 0), worm1!.Coords);
        }
        
        private void InitStabs(Action action,
            out IWormGenerator stabWormGenerator, out IActionProvider stabActionProvider)
        {
            // var stabWorm = Mock.Of<IObject>(worm =>
            //     worm.AskForAction(It.IsAny<WorldState>()) == Actions.MoveRight);
            stabWormGenerator = Mock.Of<IWormGenerator>(generator =>
                generator.GetNewWorm(It.IsAny<Coords>()) == new Worm("", new Coords(0, 0)));
            stabActionProvider = Mock.Of<IActionProvider>(provider =>
                provider.GetAction(It.IsAny<WorldState>(), It.IsAny<IObject>()) == action);
        }

        private IFoodGenerator GetStabFoodGenerator(Coords coords, out IFoodGenerator stabFoodGenerator)
        {
            return stabFoodGenerator = Mock.Of<IFoodGenerator>(generator =>
                generator.GetFood() == new Food(coords));
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