using System;
using System.Collections.Generic;
using System.Text;

namespace RouletteAPP.Entities
{
    public class ClosedRoulette
    {
        public int IdRoulette { get; set; }
        public String State { get; set; }
        public int NumberWinner { get; set; }
        public string ColorWinner { get; set; }
        public List<Bet> ListBet { get; set; }
    }
}
