using RouletteAPP.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RouletteAPP.Data.Service.Abstract
{
    public interface IService
    {
        Task<int> Add();
        Task<bool> Open(int id);
        Task<bool> Bet(BetUser model);
        Task<bool> Closed(int id, int numberWinner, string colorWinner);
        Task<List<RouletteState>> ListRoulettes(int id);
        Task<List<Bet>> ListBet(int id);
        Task<int> ValidateStateRoulette(int id);
        Task<decimal> ValidateBetRoulette(int id);
    }
}
