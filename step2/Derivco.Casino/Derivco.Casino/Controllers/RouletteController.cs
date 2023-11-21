using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Derivco.Casino.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
       

        private readonly ILogger<RouletteController> _logger;
        private readonly IRouletteService _rouletteService;

        public RouletteController(ILogger<RouletteController> logger,
            IRouletteService rouletteService)
        {
            _logger = logger;
            _rouletteService = rouletteService;
        }

        [HttpGet]
        [Route("Roulette/GetPreviousRounds")]       
        public async Task<IEnumerable<RoundDto>> GetPreviousRounds()
        {
            return await _rouletteService.GetPreviousRounds(); 
        }

        [HttpGet]
        [Route("Roulette/GetPreviousRound/{roundCorellationId}")]
        public async Task<RoundDto> GetPreviousRound(Guid roundCorellationId)
        {

            var previousRound = await _rouletteService.GetPreviousRound(roundCorellationId);
            if (previousRound == null)
            {
                // Handled By Global Exception Handler - returns HttpStatusCode.NotFound
                throw new KeyNotFoundException();
            }

            return previousRound;

        }

        [HttpPost]
        [Route("Roulette/StartRound")]
        public async Task<BeginRoundResult> StartRound()
        {
            return await _rouletteService.BeginRound();
        }        
       

        [HttpPost]
        [Route("Roulette/PlaceBet/{roundCorellationId}")]
        public async Task<PlaceBetResult> PlaceBet(Guid roundCorrelationId, List<NewBetOption> betsToPlace)
        {
            List<PlaceBetOption> newBets = new List<PlaceBetOption>();
                
            foreach(var bet in betsToPlace)
            {
                newBets.Add(new PlaceBetOption
                {
                    CorrelationId = Guid.NewGuid(),
                    RoundCorrelationId = roundCorrelationId,
                    Value = bet.Value,
                    Option = bet.RouletteBetOption,
                    HasPayout = false,
                    PayoutValue = 0
                });
            }


            return  await _rouletteService.PlaceBet(roundCorrelationId, newBets);
        }

        [HttpPost]
        [Route("Roulette/SpinRound/{roundCorellationId}")]
        public async Task<SpinResult> SpinRound(Guid roundCorrelationId)
        {
            return await _rouletteService.Spin(roundCorrelationId);
        }

        [HttpPost]
        [Route("Roulette/Payout/{roundCorellationId}")]
        public async Task<PayoutResult> Payout(Guid roundCorrelationId)
        {
            return await _rouletteService.Payout(roundCorrelationId);
        }

    }
}
