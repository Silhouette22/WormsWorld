using System.Threading;
using System.Threading.Tasks;
using ConsoleApp;
using Microsoft.Extensions.Hosting;

namespace BehaviourGeneratorApp
{
    public class BehaviourGeneratorService : IHostedService
    {
        private readonly IBehaviourGenerator _generator;
        private readonly MyDbContext _context;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly object _lockObj = new();
        private bool _isNotCancelled = true;

        public BehaviourGeneratorService(IBehaviourGenerator generator, MyDbContext context,
            IHostApplicationLifetime applicationLifetime)
        {
            _generator = generator;
            _context = context;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(Run, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Task.Run(Cancel, cancellationToken);
            return Task.CompletedTask;
        }

        private void Run()
        {
            var behaviour = _generator.GenerateBehaviour();
            lock (_lockObj){
                if (_isNotCancelled)
                {
                    using var repository = new DbRepository(_context);
                    repository.StoreBehaviour(behaviour);
                }
            }
            
            _applicationLifetime.StopApplication();
        }

        private void Cancel()
        {
            lock (_lockObj)
            {
                _isNotCancelled = false;
            }
        }
    }
}