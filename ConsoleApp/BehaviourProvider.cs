namespace ConsoleApp
{
    public class BehaviourProvider : IBehaviourProvider
    {
        private readonly DbRepository _repository;
        private readonly INameProvider _nameProvider;

        public BehaviourProvider(DbRepository repository, INameProvider nameProvider)
        {
            _repository = repository;
            _nameProvider = nameProvider;
        }

        public Behaviour GetBehaviour(string name)
        {
            return _repository.LoadBehaviour(name);
        }

        public Behaviour GetBehaviour()
        {
            return _repository.LoadBehaviour(_nameProvider.GetName());
        }
    }

    public interface IBehaviourProvider
    {
        public Behaviour GetBehaviour(string name);

        public Behaviour GetBehaviour();
    }
}