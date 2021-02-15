using System;
using System.Collections.Generic;
using System.Text;

namespace RouletteAPP.Entities
{
    public class BetUser
    {
        public decimal MoneyBet { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public int IdRoulette { get; set; }
        public int? IdUser { get; set; }
    }
}
