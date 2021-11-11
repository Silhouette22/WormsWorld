using System.IO;

namespace ConsoleApp
{
    public class WorldController
    {
        private readonly WorldState _state;

        private int _iterations;

        public WorldController() : this(1)
        {
        }

        public WorldController(int wormsNumber)
        {
            _state = new WorldState();
            _state.AddObject(WormGenerator.GetNewWorm(Constants.DefaultGenerateCoords));
            for (var i = 1; i < wormsNumber; i++)
            {
                while(!_state.AddObject(WormGenerator.GetRandomNewWorm()));
            }
        }

        public void Start(int iterations = 100)
        {
            _iterations = iterations;
            Run();
        }

        private void Run()
        {
            using StreamWriter file = new("output.txt");
            for (var i = 0; i < _iterations; i++)
            {
                while (!_state.AddFood());
                
                //in file
                // ReportWriter.WriteReport(file, i, _state.ToString());
                //in Console
                ReportWriter.WriteReport(i, _state.ToString());

                foreach (IObject obj in _state.ReverseObjects())
                {
                    var doAction = obj.AskForAction(_state);
                    doAction(obj, _state);
                }

                foreach (IObject obj in _state.ReverseObjects())
                {
                    obj.LoseHP(out var isDead);
                    if (isDead) _state.RemoveObject(obj.Coords);
                }
            }
        }
    }
}