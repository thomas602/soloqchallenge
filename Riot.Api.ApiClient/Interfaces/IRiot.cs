using Microsoft.AspNetCore.Mvc;
using Refit;
using Riot.Api.ApiClient.Dtos;

namespace Riot.Api.ApiClient.Interfaces
{
    public interface IRiot
    {
        [Get("/lol/league/v4/entries/by-summoner/{id}?api_key={apiKey}")]
        Task<List<PlayerDto>> Get([FromRoute] string id, string apiKey);

    }
}
