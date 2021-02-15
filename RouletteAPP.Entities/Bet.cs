using System;

namespace RouletteAPP.Entities
{
    public class Bet
    {
        public int IdBet { get; set; }
        public int IdUser { get; set; }
        public decimal MoneyBet { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public decimal MoneyWon { get; set; }
        public int IdRoulette { get; set; }
    }
}
