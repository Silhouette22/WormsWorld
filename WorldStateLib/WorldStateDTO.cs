using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ObjectsLib;

namespace WorldStateLib
{
    public class WorldStateDto
    {
        [Bindable(BindableSupport.Yes)]
        public Worm Worm { get; set; }
        [Bindable(BindableSupport.Yes)]
        public IEnumerable<Worm> Worms { get; set; }
        [Bindable(BindableSupport.Yes)]
        public IEnumerable<Food> Foods { get; set; }

        [JsonConstructor]
        public WorldStateDto(Worm worm, IEnumerable<Worm> worms, IEnumerable<Food> foods)
        {
            Worm = worm;
            Worms = worms;
            Foods = foods;
        }
        

        public (Worm, WorldState) ToData()
        {
            Dictionary<Coords, IObject> map = new Dictionary<Coords, IObject>();
            
            FillMapIfNotNullCollection(map, Worms);
            FillMapIfNotNullCollection(map, Foods);

            var state = new WorldState(map);
            return (Worm, state);
        }

        private void FillMapIfNotNullCollection(IDictionary<Coords, IObject> map, IEnumerable<IObject> collection)
        {
            if (collection is null) return;
            foreach (var obj in collection)
            {
                map[obj.Coords] = obj;
            }
        }
    }
}