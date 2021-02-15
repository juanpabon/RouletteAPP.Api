using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouletteAPP.Api.Model;
using RouletteAPP.BLL.Abstract;
using RouletteAPP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPP.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private IRouletteManager _rouletteManager;
        private const string ERROR_MESSAGE = "There is no data";
        public RouletteController(IRouletteManager rouletteManager)
        {
            _rouletteManager = rouletteManager;
        }
        [HttpPost]
        public async Task<ActionResult> Add()
        {
            var isCreated = await _rouletteManager.Add();
            if (isCreated > 0)
                return StatusCode(200, isCreated);
            return StatusCode(500);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Open(int id)
        {
            var isUpdated = await _rouletteManager.Open(id);
            if (!isUpdated)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });
            return StatusCode(204);
        }
        [HttpPost]
        public async Task<ActionResult> Bet([FromHeader]int IdUser, [FromBody] BetUser betUser)
        {
            betUser.IdUser = IdUser;
            var isCreated = await _rouletteManager.Bet(betUser);
            if (!isCreated)
                return StatusCode(201);
            return StatusCode(500);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Closed(int id)
        {
            var closedRoulette = await _rouletteManager.Closed(id);
            if (closedRoulette == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });
            return StatusCode(204, closedRoulette);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> ListRoulettes(int id)
        {
            var rouletteState = await _rouletteManager.ListRoulettes(id);
            if (rouletteState == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(200, rouletteState);
        }
    }
}
