using Microsoft.Extensions.Configuration;
using RouletteAPP.BLL.Abstract;
using RouletteAPP.Cache.Abstract;
using RouletteAPP.Data.Service.Abstract;
using RouletteAPP.Entities;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RouletteAPP.BLL
{
    public class RouletteManager : IRouletteManager
    {
        private const string REDIS_CACHE_LIST_KEY = "RouletteList";
        private const string REDIS_CACHE_LIST_BET_KEY = "RouletteListBet.{0}";
        private IRouletteService _rouletteService;
        private ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan expirationTime;

        public RouletteManager(IRouletteService rouletteService, ICacheService cacheService, IConfiguration configuration)
        {
            _rouletteService = rouletteService;
            _cacheService = cacheService;
            _configuration = configuration;
            expirationTime = TimeSpan.FromSeconds(_configuration["RedisConfig:ExpirationTime"].ToInt());
        }
        public async Task<int> Add()
        {
            var result = await _rouletteService.Add();
            _cacheService.ClearKeysByPattern(REDIS_CACHE_LIST_KEY);
            return result;
        }
        public async Task<bool> Open(int id)
        {
            var result = await _rouletteService.Open(id);
            _cacheService.ClearKeysByPattern(REDIS_CACHE_LIST_KEY);
            return result;
        }
        public async Task<bool> Bet(BetUser model)
        {
            var validate = this.ValidateStateRoulette(model.IdRoulette);
            if (validate.Result == 1)
            {
                var totalBet = this.ValidateBetRoulette(model.IdRoulette);
                if ((totalBet.Result + model.MoneyBet) <= 10000)
                {
                    if (model.Number != null && model.Number >= 0 && model.Number <= 36)
                    {
                        model.Color = string.Empty;
                    }
                    else if (model.Color == null || (model.Color.ToUpper() != "NEGRO" && model.Color.ToUpper() != "ROJO"))
                    {
                        return false;
                    }
                    var result = await _rouletteService.Bet(model);
                    _cacheService.ClearKeysByPattern(string.Format(REDIS_CACHE_LIST_BET_KEY, model.IdRoulette));
                    return result;
                }
            }
            return false;
        }
        public async Task<ClosedRoulette> Closed(int id)
        {
            Random random = new Random();
            int _randomNumber = random.Next(0 - 37);
            string _color = string.Empty;
            _color = _randomNumber % 2 == 0 ? "Rojo" : "Negro";
            var result = await _rouletteService.Closed(id, _randomNumber, _color);
            _cacheService.ClearKeysByPattern(REDIS_CACHE_LIST_KEY);
            _cacheService.ClearKeysByPattern(string.Format(REDIS_CACHE_LIST_BET_KEY, id));
            ClosedRoulette closedRoulette = new ClosedRoulette();
            closedRoulette.IdRoulette = id;
            closedRoulette.State = "Colsed";
            closedRoulette.NumberWinner = _randomNumber;
            closedRoulette.ColorWinner = _color;
            closedRoulette.ListBet = new List<Bet>();
            closedRoulette.ListBet = this.ListBet(id).Result;
            return closedRoulette;
        }
        public async Task<List<RouletteState>> ListRoulettes(int id)
        {
            string cacheKey = REDIS_CACHE_LIST_KEY;
            var data = _cacheService.Get<List<RouletteState>>(cacheKey);
            if (data != null)
                return data;

            data = await _rouletteService.ListRoulettes(id);
            if (data != null)
            {
                _cacheService.Set<List<RouletteState>>(cacheKey, data, expirationTime);
                return data;
            }
            return null;
        }
        public async Task<List<Bet>> ListBet(int id)
        {
            string cacheKey = string.Format(REDIS_CACHE_LIST_BET_KEY, id);
            var data = _cacheService.Get<List<Bet>>(cacheKey);
            if (data != null)
                return data;

            data = await _rouletteService.ListBet(id);
            if (data != null)
            {
                _cacheService.Set<List<Bet>>(cacheKey, data, expirationTime);
                return data;
            }
            return null;

        }
        public async Task<int> ValidateStateRoulette(int id)
        {
            return await _rouletteService.ValidateStateRoulette(id);
        }
        public async Task<decimal> ValidateBetRoulette(int id)
        {
            return await _rouletteService.ValidateBetRoulette(id);
        }
    }
}
