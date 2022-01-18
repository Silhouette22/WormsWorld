using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ObjectsLib;
using WorldStateLib;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NUnitTestProject
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void SerializeWorldState()
        {
            
        }

        [Test]
        public void DeserializeIObject()
        {

            var json = "{\"coords\": {\"X\":1, \"Y\":1},\"hp\":10}";
            var food = JsonSerializer.Deserialize<Food>(json);
            Assert.IsInstanceOf<Food>(food);
        }

        [Test]
        public void SerializeTest()
        {
            var worm1 = new Worm("worm1", 1, 1);
            var worm2 = new Worm("worm2", 1, 2);
            var food1 = new Food(new Coords(3, 3));
            var map = new Dictionary<Coords, IObject>()
            {
                {worm1.Coords, worm1},
                {worm2.Coords, worm2},
                {food1.Coords, food1}
            };
            var state = new WorldState(map);
            var dto = state.ToDto(worm1);
            var (newWorm1, newState) = dto.ToData();
            var worms = new List<Worm>
            {
                worm1,
                worm2
            };
            var json = JsonSerializer.Serialize(worms);
            Console.WriteLine(json);

            var newWorms = JsonSerializer.Deserialize<IEnumerable<Worm>>(json);
            Assert.NotNull(newWorms);
            Assert.AreEqual(2, newWorms.Count());
        }
    }
}