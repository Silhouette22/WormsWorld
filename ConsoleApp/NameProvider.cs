namespace ConsoleApp
{
    public class NameProvider : INameProvider
    {
        private readonly string _name;
        public NameProvider(string name)
        {
            _name = name;
        }
        public string GetName()
        {
            return _name;
        }
    }

    public interface INameProvider
    {
        public string GetName();
    }
}