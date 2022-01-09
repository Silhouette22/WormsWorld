namespace ConsoleApp
{
    public class Worm : IObject
    {
        public Worm(string name, int x, int y) : this(name, new Coords(x, y))
        {
        }

        public Worm(string name, Coords coords)
        {
            Name = name;
            Coords = coords;
        }

        public string Name { get; }

        public Coords Coords { get; set; }

        // ReSharper disable once InconsistentNaming
        public int HP { get; private set; } = Constants.BaseHP;

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