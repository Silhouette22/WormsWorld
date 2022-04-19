using System.Threading.Tasks;
using ObjectsLib;
using WorldStateLib;

namespace ActionProviderLib
{
    public interface IActionProvider
    {
        Task<Action> GetAction(WorldState state, IObject obj);
    }
}