using System.Threading.Tasks;
using ObjectsLib;
using WorldStateLib;

namespace ActionProviderLib
{
    public interface IActionProvider
    {
        Action GetAction(WorldState state, IObject obj);
    }
}