using System.Text.Json.Serialization;

namespace ObjectsLib
{
    public interface IObject
    {
        public Coords Coords { get; set; }

        public int HP { get; }

        public void LoseHP(out bool isDead);
    }

    public class Food : IObject
    {
        public Food(Coords coords)
        {
            Coords = coords;
            HP = Constants.BaseHP;
        }

        [JsonConstructor]
        public Food(Coords coords, int hp)
        {
            Coords = coords;
            HP = hp;
        }

        public Coords Coords { get; set; }

        public int HP { get; set; }

        public void LoseHP(out bool isDead)
        {
            isDead = (HP -= Constants.LostHP) <= 0;
        }

        public override string ToString()
        {
            return $"{HP}HP{Coords}";
        }
    }
}