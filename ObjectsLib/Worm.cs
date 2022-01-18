using System.Text.Json.Serialization;

namespace ObjectsLib
{
    public class Worm : IObject
    {
        public Worm(string name, int x, int y) : this(name, new Coords(x, y))
        {
        }

        [JsonConstructor]
        public Worm(string name, Coords coords, int hp) : this(name, coords)
        {
            HP = hp;
        }
        public Worm(string name, Coords coords)
        {
            Name = name;
            Coords = coords;
            HP = Constants.BaseHP;
        }

        public string Name { get; set; }

        public Coords Coords { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HP { get; set; }

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

        public override string ToString()
        {
            return $"{Name}|{HP}HP{Coords}";
        }
    }
}