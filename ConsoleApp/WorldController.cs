using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp
{
    public class WorldController : IHostedService
    {
        private readonly WorldState _state;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostApplicationLifetime _applicationLifetime;

        private bool _running;

        private int _iterations;

        public WorldController(WorldState state, IServiceScopeFactory scopeFactory,
            IHostApplicationLifetime applicationLifetime) : this(state, 1)
        {
            _scopeFactory = scopeFactory;
            _applicationLifetime = applicationLifetime;
        }

        private WorldController(WorldState state, int wormsNumber)
        {
            _state = state;
            _state.AddWorm(Constants.DefaultGenerateCoords);
            for (var i = 1; i < wormsNumber; i++)
            {
                while (!_state.AddWorm()) ;
            }
        }

        private void Start(int iterations = 100)
        {
            _iterations = iterations;
            _running = true;
            Run();
        }

        private void Run()
        {
            using var reportWriter = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReportWriter>();
            for (var i = 0; i < _iterations && _running; i++)
            {
                using (var foodScope = _scopeFactory.CreateScope())
                {
                    var foodGenerator = foodScope.ServiceProvider.GetRequiredService<IFoodGenerator>();
                    while (!_state.AddFood(foodGenerator.GetFood())) ;
                }

                //in Console
                // reportWriter.WriteReportConsole(i, _state.ToString());
                //in file
                reportWriter.WriteReport(i, _state.ToString());

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
            _applicationLifetime.StopApplication();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            return Task.CompletedTask;
        }
    }
}