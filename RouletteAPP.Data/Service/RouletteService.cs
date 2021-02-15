using RouletteAPP.Data.Service.Abstract;
using RouletteAPP.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RouletteAPP.Service.Data
{
    public class RouletteService : IRouletteService
    {
        private IBaseDataAccess _baseData = new BaseDataAccess();
        public async Task<int> Add()
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameterOut("idRoulette", SqlDbType.Int));
            var result = await _baseData.ExecuteScalarAsync("sp_AddRoulette", _listParameter, parameterOutput: "idRoulette");
            return Convert.ToInt32(result);
        }
        public async Task<bool> Open(int id)
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRoulette", id));
            int result = await _baseData.ExcecuteNonQueryAsync("sp_OpenRoulette", _listParameter);
            return result == 1 ? true : false;
        }
        public async Task<bool> Bet(BetUser model)
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRoulette", model.IdRoulette));
            _listParameter.Add(_baseData.GetParameter("moneyBet", model.MoneyBet));
            _listParameter.Add(_baseData.GetParameter("idUser", model.IdUser));
            _listParameter.Add(_baseData.GetParameter("number", model.Number));
            _listParameter.Add(_baseData.GetParameter("color", model.Color));
            int result = await _baseData.ExcecuteNonQueryAsync("sp_AddBet", _listParameter);
            return result == 1 ? true : false;
        }
        public async Task<bool> Closed(int id, int numberWinner, string colorWinner)
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRulette", id));
            _listParameter.Add(_baseData.GetParameter("numberWinner", numberWinner));
            _listParameter.Add(_baseData.GetParameter("colorWinner", colorWinner));
            int result = await _baseData.ExcecuteNonQueryAsync("sp_OpenRoulette", _listParameter);
            return result == 1 ? true : false;
        }
        public async Task<List<RouletteState>> ListRoulettes(int id)
        {
            SqlDataReader result;
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRulette", id));
            result = await _baseData.GetDataReaderAsync("sp_ListRoulettes", _listParameter);
            List<RouletteState> _listRouletteState = new List<RouletteState>();
            while (result.Read())
            {
                RouletteState _rouletteState = new RouletteState();
                _rouletteState.IdRoulette = Convert.ToInt32(result["idRoulette"]);
                _rouletteState.State = Convert.ToString(result["state"]);
                _listRouletteState.Add(_rouletteState);
            }
            return _listRouletteState;
        }
        public async Task<List<Bet>> ListBet(int id)
        {
            SqlDataReader result;
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRulette", id));
            result = await _baseData.GetDataReaderAsync("sp_ListBetsRoulette", _listParameter);
            List<Bet> _listBet = new List<Bet>();
            while (result.Read())
            {
                Bet _bet = new Bet();
                _bet.IdBet = Convert.ToInt32(result["idBet"]);
                _bet.IdUser = Convert.ToInt32(result["idUser"]);
                _bet.MoneyBet = Convert.ToDecimal(result["moneyBet"]);
                _bet.Number = Convert.ToInt32(result["number"]);
                _bet.Color = Convert.ToString(result["color"]);
                _bet.MoneyWon = Convert.ToDecimal(result["moneyWon"]);
                _listBet.Add(_bet);
            }
            return _listBet;
        }
        public async Task<int> ValidateStateRoulette(int id)
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRoulette", id));
            _listParameter.Add(_baseData.GetParameterOut("state", SqlDbType.Int));
            int result = Convert.ToInt32(await _baseData.ExecuteScalarAsync("sp_ValidateStateRoulette", _listParameter, parameterOutput: "state"));
            return result;
        }
        public async Task<decimal> ValidateBetRoulette(int id)
        {
            List<SqlParameter> _listParameter = new List<SqlParameter>();
            _listParameter.Add(_baseData.GetParameter("idRoulette", id));
            _listParameter.Add(_baseData.GetParameterOut("remainingMoney", SqlDbType.Decimal));
            decimal result = Convert.ToDecimal(await _baseData.ExecuteScalarAsync("sp_ValidateBetRoulette", _listParameter, parameterOutput: "remainingMoney"));
            return result;
        }
    }
}
