using ActionProviderLib;
using Microsoft.AspNetCore.Mvc;
using WorldStateLib;

namespace ActionProviderWebServer.Controllers
{
    [ApiController]
    [Route("WormsWorld/ActionProvider")]
    public class ActionProviderHttpController : ControllerBase
    {
        private readonly IActionProvider _actionProvider;

        public ActionProviderHttpController(IActionProvider actionProvider) {
            _actionProvider = actionProvider;
        }

        [HttpPost("{name}")]
        public ActionDto GetAction(string name, [FromBody] WorldStateDto dto)
        {
            var (w, state) = dto.ToData();
            if (!state.TryGetWormByName(name, out var worm)) return null;
            if (w.Name == name) return new ActionDto(_actionProvider.GetAction(state, worm));
            StatusCode(404);
            return null;
        }
    }
}