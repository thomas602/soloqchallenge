using DuoQChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DuoQChallenge.Dtos;
using System.Net;
using System.IO;

namespace DuoQChallenge.Controllers
{
    public class PlayerController : Controller
    {
        private readonly duoqchallengeContext _context;
        static readonly HttpClient client = new HttpClient();

        public PlayerController(duoqchallengeContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        [HttpGet("get-player")]
        public  PlayerDto GetPlayerInfoFromRiot([FromQuery] string userId)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            PlayerDto player = new PlayerDto();
            string respose = string.Empty;
            string url = "https://la2.api.riotgames.com/lol/league/v4/entries/by-summoner/" + userId + "?api_key=RGAPI-eaab9024-a6e2-4315-8955-3f3e61fca951";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                var result = reader.ReadToEnd();

                List<PlayerDto> myDeserializedObjList = (List<PlayerDto>)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(List<PlayerDto>));

                foreach (PlayerDto item in myDeserializedObjList)
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

        [HttpPost("post-player")]
        public IActionResult Create([FromBody]PlayerRequest p)
        {
            if (string.IsNullOrEmpty(p.userId))
            {
                return BadRequest();
            }

            var playerInfo = GetPlayerInfoFromRiot(p.userId);

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
