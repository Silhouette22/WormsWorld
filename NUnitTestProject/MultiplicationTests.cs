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
    public class MultiplicationTests
    {
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MultiplyToFoodCellTest(int x, int y)
        {
            var direction = new Coords(x, y);

            var action = GetMultiply(direction);
            GetStabWormGenerator(new Coords(0, 0), out var stabWormGenerator);
            GetStabFoodGenerator(direction, out var stabFoodGenerator);
            GetStabActionProvider(action, out var stabActionProvider);
            
            var state = new WorldState(stabWormGenerator);
            state.AddWorm();
            Assert.True(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm = obj as Worm;

            state.AddFood(stabFoodGenerator.GetFood());

            var controller = new WorldController(state, stabActionProvider);
            controller.MakeStep();

            Assert.AreEqual(new Coords(0, 0), worm!.Coords);

            // Assert.Greater(worm.HP, Constants.BaseHP);
        }
        
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MultiplyToEmptyCellTest(int x, int y)
        {
            var direction = new Coords(x, y);

            var action = GetMultiply(direction);
            GetStabActionProvider(action, out var stabActionProvider);
            
            var state = new WorldState(new WormGenerator());
            state.AddWorm(new Coords(0, 0));
            Assert.True(state.TryGetObject(new Coords(0, 0), out var worm1));
            (worm1 as Worm)!.Eat();

            var controller = new WorldController(state, stabActionProvider);
            controller.MakeStep();

            Assert.AreEqual(new Coords(0, 0), worm1!.Coords);

            Assert.True(state.TryGetObject(direction, out var worm2));
            Assert.IsInstanceOf<Worm>(worm2);
            
            // Assert.Greater(worm.HP, Constants.BaseHP);
        }
        
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, 0)]
        public void MultiplyToBusyCellTest(int x, int y)
        {
            var direction = new Coords(x, y);
            var action = GetMultiply(direction);

            var state = new WorldState(new WormGenerator());
            state.AddWorm( new Coords(0, 0));
            Assert.True(state.TryGetObject(new Coords(0, 0), out var obj));
            var worm1 = obj as Worm;

            state.AddWorm(direction);
            var pastHP = worm1!.HP;
            action(worm1, state);
            Assert.AreEqual(pastHP, worm1.HP);
            //Did not move
            Assert.AreEqual(new Coords(0, 0), worm1!.Coords);
        }

        private void GetStabWormGenerator(Coords coords, out IWormGenerator stabWormGenerator)
        {
            // var stabWorm = Mock.Of<IObject>(worm =>
            //     worm.AskForAction(It.IsAny<WorldState>()) == Actions.MoveRight);
            stabWormGenerator = Mock.Of<IWormGenerator>(generator =>
                generator.GetNewWorm(It.IsAny<Coords>()) == new Worm("", coords));
        }

        private IFoodGenerator GetStabFoodGenerator(Coords coords, out IFoodGenerator stabFoodGenerator)
        {
            return stabFoodGenerator = Mock.Of<IFoodGenerator>(generator =>
                generator.GetFood() == new Food(coords));
        }

        private void GetStabActionProvider(Action action, out IActionProvider stabActionProvider)
        {
            stabActionProvider = Mock.Of<IActionProvider>(provider =>
                provider.GetAction(It.IsAny<WorldState>(), It.IsAny<IObject>()) == action);
        }

        private Action GetMultiply(Coords direction)
        {
            Action action = direction switch
            {
                (0, 1) => Actions.MultiplyUp,
                (1, 0) => Actions.MultiplyRight,
                (0, -1) => Actions.MultiplyDown,
                (-1, 0) => Actions.MultiplyLeft,
                _ => Actions.DoNothing
            };
            return action;
        }
    }
}