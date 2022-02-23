using Microsoft.AspNetCore.Mvc;
using Refit;
using Riot.Api.ApiClient.Dtos;

namespace Riot.Api.ApiClient.Interfaces
{
    public interface IRiot
    {
        [Get("/lol/league/v4/entries/by-summoner/{id}?api_key={apiKey}")]
        Task<List<PlayerDto>> Get([FromRoute] string id, string apiKey);

        [Get("/lol/summoner/v4/summoners/by-name/{username}?api_key={apiKey}")]
        Task<PlayerInfoDto> GetId([FromRoute] string username, string apiKey);

    }
}
