using System.Threading;
using System.Threading.Tasks;
using ActionProviderLib;
using FoodGeneratorLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ObjectsLib;
using WorldStateLib;

namespace ConsoleApp
{
    public class WorldController : IHostedService
    {
        private readonly WorldState _state;
        private readonly IActionProvider _actionProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostApplicationLifetime _applicationLifetime;

        private bool _running;

        private int _iterations;

        public WorldController(WorldState state, IActionProvider actionProvider, IServiceScopeFactory scopeFactory, 
            IHostApplicationLifetime applicationLifetime, int wormsNumber = 1) : this(state, actionProvider)
        {
            _scopeFactory = scopeFactory;
            _applicationLifetime = applicationLifetime;
            InitState(wormsNumber, null);
        }

        public WorldController(WorldState state, IActionProvider actionProvider)
        {
            _state = state;
            _actionProvider = actionProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _iterations = 100;
            _running = true;
            await Task.Run(Run, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            return Task.CompletedTask;
        }

        public async Task MakeStep()
        {
            foreach (IObject obj in _state.ReverseObjects())
            {
                var doAction = await _actionProvider.GetAction(_state, obj);
                doAction(obj, _state);
            }


            foreach (IObject obj in _state.ReverseObjects())
            {
                obj.LoseHP(out var isDead);
                if (isDead) _state.RemoveObject(obj.Coords);
            }
        }

        private void InitState(int wormsNumber, IFoodGenerator foodGenerator, int foodNumber = 0)
        {
            _state.AddWorm(Constants.DefaultGenerateCoords);
            for (var i = 1; i < wormsNumber; i++)
            {
                while (!_state.AddWorm()) ;
            }

            for (var i = 0; i < foodNumber; i++)
            {
               AddFood(foodGenerator, i);
            }
        }

        private async Task Run()
        {
            using var reportWriter = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IReportWriter>();
            for (var i = 0; i < _iterations && _running; i++)
            {
                AddFood(i);
                //in Console
                reportWriter.WriteReportConsole(i, _state.ToString());
                //in file
                // reportWriter.WriteReport(i, _state.ToString());
                await MakeStep();
            }

            _applicationLifetime.StopApplication();
        }

        private void AddFood(IFoodGenerator foodGenerator, int turn)
        {
            while (!_state.AddFood(foodGenerator.GetFood(turn))) ;
        }

        private void AddFood(int turn)
        {
            using var foodScope = _scopeFactory.CreateScope();
            var foodGenerator = foodScope.ServiceProvider.GetRequiredService<IFoodGenerator>();
            AddFood(foodGenerator, turn);
        }
    }
}