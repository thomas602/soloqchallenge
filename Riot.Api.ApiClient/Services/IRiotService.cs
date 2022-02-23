using Riot.Api.ApiClient.Dtos;

namespace Riot.Api.ApiClient.Services
{
    public interface IRiotService
    {
        Task<List<PlayerDto>> GetPlayerByIdAsync(string id);
        Task<PlayerDto> GetPlayerByIdSoloQAsync(string id);
        Task<string> GetPlayerIdAsync(string inGameName);
    }
}
