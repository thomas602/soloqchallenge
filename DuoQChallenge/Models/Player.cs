using System;
using System.Collections.Generic;

namespace DuoQChallenge.Models
{
    public partial class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Elo { get; set; } = null!;
        public int Wins { get; set; }
        public int Loses { get; set; }
        public double Winrate { get; set; }
        public string OpggUrl { get; set; } = null!;
    }
}
