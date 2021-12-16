using System.Collections.Generic;
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

        public void InitState(int wormsNumber, IFoodGenerator foodGenerator, int foodNumber = 0)
        {
            _state.AddWorm(Constants.DefaultGenerateCoords);
            for (var i = 1; i < wormsNumber; i++)
            {
                while (!_state.AddWorm()) ;
            }

            for (int i = 0; i < foodNumber; i++)
            {
               AddFood(foodGenerator);
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
                AddFood();
                //in Console
                reportWriter.WriteReportConsole(i, _state.ToString());
                //in file
                // reportWriter.WriteReport(i, _state.ToString());
                MakeStep();
            }

            _applicationLifetime.StopApplication();
        }

        public void MakeStep()
        {
            foreach (IObject obj in _state.ReverseObjects())
            {
                var doAction = _actionProvider.GetAction(_state, obj);
                doAction(obj, _state);
            }


            foreach (IObject obj in _state.ReverseObjects())
            {
                obj.LoseHP(out var isDead);
                if (isDead) _state.RemoveObject(obj.Coords);
            }
        }

        public void AddFood(IFoodGenerator foodGenerator)
        {
            while (!_state.AddFood(foodGenerator.GetFood())) ;
        }

        private void AddFood()
        {
            using var foodScope = _scopeFactory.CreateScope();
            var foodGenerator = foodScope.ServiceProvider.GetRequiredService<IFoodGenerator>();
            AddFood(foodGenerator);
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