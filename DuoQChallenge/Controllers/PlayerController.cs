using DuoQChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Riot.Api.ApiClient.Services;
using Riot.Api.ApiClient.Dtos;
using DuoQChallenge.Dtos;

namespace DuoQChallenge.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        private readonly duoqchallengeContext _context;
        private readonly IRiotService _riotService;

        public PlayerController(duoqchallengeContext context, IRiotService riotService)
        {
            _context = context;
            _riotService = riotService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        [HttpGet("{userId}")]
        public async Task<Riot.Api.ApiClient.Dtos.PlayerDto> GetPlayerInfoFromRiot([FromRoute] string userId)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            Riot.Api.ApiClient.Dtos.PlayerDto player = new Riot.Api.ApiClient.Dtos.PlayerDto();

            try
            {
                var playerList = await _riotService.GetPlayerByIdAsync(userId);

                foreach (Riot.Api.ApiClient.Dtos.PlayerDto item in playerList)
                {
                    if(item.queueType == "RANKED_SOLO_5x5")
                        player = item;
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return player;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]PlayerRequest p)
        {
            if (string.IsNullOrEmpty(p.userId))
            {
                return BadRequest();
            }

            var playerInfo = await _riotService.GetPlayerByIdSoloQAsync(p.userId);

            if(playerInfo == null)
            {
                return NotFound("No se encontro el jugador");
            }

            var player = new Player()
            {
                Name = p.name,
                Role = p.role,
                Account = playerInfo.summonerName,
                Elo = playerInfo.tier + " " + playerInfo.leaguePoints + "LPS",
                Wins = playerInfo.wins,
                Loses = playerInfo.losses,
                Winrate = (playerInfo.wins * 100) / (playerInfo.wins + playerInfo.losses),
                OpggUrl = "https://las.op.gg/summoners/las/" + playerInfo.summonerName
            };

            _context.Add(player);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class PlayerRequest
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
}
