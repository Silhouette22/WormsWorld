using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ObjectsLib;
using WorldStateLib;

namespace ActionProviderLib
{
    public class HttpActionProvider : IActionProvider
    {
        private readonly HttpClient _client;

        public HttpActionProvider(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.Timeout = new TimeSpan(0, 1, 0);
        }

        public async Task<Action> GetAction(WorldState state, IObject obj)
        {
            if (obj is not Worm worm) return Actions.DoNothing;
            
            var response = await _client.PostAsJsonAsync(
                _client.BaseAddress + $"/{worm.Name}", 
                new WorldStateDto(worm, state.Select<Worm>(), state.Select<Food>()));
            var actionDto = await response.Content.ReadFromJsonAsync<ActionDto>();
            return actionDto?.ToAction();
        }
    }
}