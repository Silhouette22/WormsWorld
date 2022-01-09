namespace ConsoleApp
{
    public interface IObject
    {
        public Coords Coords { get; set; }
        
        public int HP { get; }

        public void LoseHP(out bool isDead);
    }
    
    public class Food : IObject
    {
        public Food(Coords coords) => Coords = coords;
        public Coords Coords { get; set; }
        
        public int HP { get; private set; } = Constants.BaseHP;

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