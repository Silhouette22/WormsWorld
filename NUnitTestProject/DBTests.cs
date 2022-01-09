using System.IO;
using BehaviourGeneratorApp;
using ConsoleApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace NUnitTestProject
{
    [TestFixture]
    public class DBTests
    {
        private DbRepository _repository;
        private IBehaviourGenerator _behaviourGenerator;
        private IHostBuilder _hostBuilder;
        
        private const string BehaviourName = "testBehaviour";
        private const string AlreadyInDBBehaviourName = "alreadyInDBBehaviourName";
        private const int AlreadyInDBTurns = 100;


        [OneTimeSetUp]
        public void SetUp()
        {
            var builder = new DbContextOptionsBuilder<MyDbContext>();
            builder.UseInMemoryDatabase("InMemoryBehaviourDatabase");
            var options = builder.Options;
            _repository = new DbRepository(new MyDbContext(options));


            INameProvider nameProvider = new NameProvider(BehaviourName);
            _behaviourGenerator = new BehaviourGenerator(nameProvider);

            _repository.StoreBehaviour(new BehaviourGenerator(new NameProvider(AlreadyInDBBehaviourName))
                .GenerateBehaviour(AlreadyInDBTurns)
            );

            _hostBuilder = ConfigHostBuilder();
        }

        private IHostBuilder ConfigHostBuilder()
        {
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(services => services
                .AddHostedService<WorldController>()
                .AddSingleton(_repository)
                .AddSingleton<INameProvider>(new NameProvider(AlreadyInDBBehaviourName))
                .AddSingleton<IBehaviourProvider, BehaviourProvider>()
                .AddSingleton<IWormGenerator, WormGenerator>()
                .AddSingleton<WorldState>()
                .AddSingleton<IFoodGenerator, FoodGenerator>()
                .AddSingleton<IActionProvider, ActionProvider>()
                .AddSingleton<WorldController>()
                .AddSingleton<IReportWriter>(new ReportWriter(new StreamWriter("test.txt")))
                );
            return hostBuilder;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _repository.DeleteDB();
            _repository.Dispose();
        }

        [Test(TestOf = typeof(BehaviourGenerator))]
        public void BehaviourGeneratorTest()
        {
            const int turns = 10;
            var behaviour = _behaviourGenerator.GenerateBehaviour(turns);

            Assert.AreEqual(BehaviourName, behaviour.Name);
            Assert.AreEqual(turns, behaviour.TurnToCoord.Count);
        }

        [Test(TestOf = typeof(DbRepository))]
        public void DataStoringAndLoadingTest()
        {
            var storedBehaviour = _behaviourGenerator.GenerateBehaviour();

            _repository.StoreBehaviour(storedBehaviour);

            var loadedBehaviour = _repository.LoadBehaviour(BehaviourName);

            Assert.AreEqual(storedBehaviour, loadedBehaviour);
        }

        [Test(TestOf = typeof(WorldController))]
        public void WormActionsWithSetUpBehaviour()
        {
            var host = _hostBuilder.Build();
            var task = host.RunAsync();
            var awaiter = task.GetAwaiter();
            awaiter.OnCompleted(() => Assert.True(task.IsCompletedSuccessfully));
        }
    }
}