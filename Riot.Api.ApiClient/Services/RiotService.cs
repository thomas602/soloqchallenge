using Riot.Api.ApiClient.Dtos;
using Refit;
using Riot.Api.ApiClient.Interfaces;
using System.Linq;

namespace Riot.Api.ApiClient.Services
{
    public class RiotService : IRiotService
    {
        public RiotService(string baseUrl, string apiKey)
        {
            _baseUrl = baseUrl;
            _apiKey = apiKey;
        }

        private string _baseUrl { get; set; }
        private string _apiKey { get; set; }
        public async Task<List<PlayerDto>> GetPlayerByIdAsync(string id)
        {
            try
            {
                var riotApi = RestService.For<IRiot>(_baseUrl);
                return await riotApi.Get(id, _apiKey);
            }
            catch (Refit.ApiException ex)
            {
                var message = await ex.GetContentAsAsync<dynamic>();
                throw new Exception((string)message.message);
            }

        }
        public async Task<PlayerDto> GetPlayerByIdSoloQAsync(string id)
        {
            const string duoQ = "RANKED_SOLO_5x5";
            try
            {
                var riotApi = RestService.For<IRiot>(_baseUrl);
                var riotApiResponse = await riotApi.Get(id, _apiKey);
                PlayerDto playerDto = null;

                if (riotApiResponse != null)
                    playerDto = riotApiResponse.Where(x => x.queueType == duoQ).FirstOrDefault();

                return playerDto;
            }
            catch (Refit.ApiException ex)
            {
                var message = await ex.GetContentAsAsync<dynamic>();
                throw new Exception((string)message.message);
            }

        }

        public async Task<string> GetPlayerIdAsync(string inGameName)
        {
            try
            {
                var riotApi = RestService.For<IRiot>(_baseUrl);
                var riotApiResponse = await riotApi.GetId(inGameName, _apiKey);
                PlayerInfoDto playerInfoDto = null;

                if (riotApiResponse != null)
                    playerInfoDto = riotApiResponse;

                return playerInfoDto.Id;
            }
            catch (Refit.ApiException ex)
            {
                var message = await ex.GetContentAsAsync<dynamic>();
                throw new Exception((string)message.message);
            }
        }
    }
}
