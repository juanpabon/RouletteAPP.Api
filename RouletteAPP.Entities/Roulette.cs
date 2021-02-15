using System;
using System.Collections.Generic;
using System.Text;

namespace RouletteAPP.Entities
{
    public class Roulette
    {
        public int IdRulette { get; set; }
        public int State { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime DateTime { get; set; }
        public int NumberWinner { get; set; }
        public int ColorWinner { get; set; }
    }
}
