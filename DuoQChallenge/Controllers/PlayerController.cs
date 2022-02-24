using DuoQChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Riot.Api.ApiClient.Services;
using Riot.Api.ApiClient.Dtos;
using DuoQChallenge.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using DuoQChallenge.Models.ViewModels;

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
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            Riot.Api.ApiClient.Dtos.PlayerDto player = new Riot.Api.ApiClient.Dtos.PlayerDto();

            if (ModelState.IsValid)
            {
                try
                {
                    string userId = await _riotService.GetPlayerIdAsync(model.InGameName);
                    var playerList = await _riotService.GetPlayerByIdAsync(userId);

                    foreach (Riot.Api.ApiClient.Dtos.PlayerDto item in playerList)
                    {
                        if (item.queueType == "RANKED_SOLO_5x5")
                            player = item;
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }

                var playerAux = new Player() {
                    Name = model.Name,
                    Role = model.Role,
                    Account = player.summonerName,
                    Elo = player.tier + " " + player.leaguePoints + "LPS",
                    Wins = player.wins,
                    Loses = player.losses,
                    Winrate = (player.wins * 100) / (player.wins + player.losses),
                    OpggUrl = "https://las.op.gg/summoners/las/" + player.summonerName
                };

                var alreadyExists = _context.Players.Where(x => x.Account == player.summonerName).FirstOrDefault();

                if (alreadyExists == null)
                {
                    _context.Add(playerAux);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    }
    public class PlayerRequest
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
}
