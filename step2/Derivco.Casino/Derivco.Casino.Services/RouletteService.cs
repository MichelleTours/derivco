﻿using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Repositories.Interfaces;
using Derivco.Casino.Services.Interfaces;
using Derivco.Casino.Shared.Constants;
using Derivco.Casino.Shared.Enums;
using Derivco.Casino.Shared.Strategies.RouletteSpinStrategy;
using Microsoft.Extensions.Logging;

namespace Derivco.Casino.Services
{


    public class RouletteService : IRouletteService
    {

        private readonly ILogger<RouletteService> logger;
        private readonly IAppDBRepository appDBRepository;

        public RouletteService(
            ILogger<RouletteService> logger,
            IAppDBRepository appDBRepository)
        {
            this.logger = logger;
            this.appDBRepository = appDBRepository;
        }

        public async Task<BeginRoundResult> BeginRound()
        {

            var beginRoundResult = new BeginRoundResult { isSuccess = true };

            beginRoundResult.RoundCorrelationId = Guid.NewGuid();
            await this.appDBRepository.CreateNewRound(beginRoundResult.RoundCorrelationId.Value);
            return beginRoundResult;

        }

        public async Task<PlaceBetResult> PlaceBet(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace)
        {
            var placeBetResult = new PlaceBetResult ();

            var rouletteRound = await this.appDBRepository.GetRoundByCorrelationId(roundCorrelationId);

            if (rouletteRound == null)
            {
                placeBetResult.Message = $"Roulette Round not found: '{roundCorrelationId}'";
                placeBetResult.isSuccess = false;
                return placeBetResult;
            }

            var validationMessage = ValidateBetInputs(betsToPlace);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                placeBetResult.Message = $"Roulette Round Place Bet Failed: '{roundCorrelationId}', reason '{validationMessage}'";
                placeBetResult.isSuccess = false;
                return placeBetResult;
            }

            List<PlaceBetOption> betOptionsToPlace = new List<PlaceBetOption>();

            foreach (var bet in betsToPlace)
            {
                betOptionsToPlace.Add( new PlaceBetOption
                {
                    RoundCorrelationId = roundCorrelationId,
                    CorrelationId = Guid.NewGuid(),
                    Value = bet.Value,
                    Option = bet.Option,
                    HasPayout = false,
                    PayoutValue = 0
                });
            }

            //Persist the Bets
            await this.appDBRepository.PlaceBets(roundCorrelationId, betOptionsToPlace);
            placeBetResult.PlacedBets.AddRange(betOptionsToPlace);

            placeBetResult.isSuccess = true;
            placeBetResult.Message = $"Roulette Round Place Bet Success: '{roundCorrelationId}', {betsToPlace.Count} bets to the value of {betsToPlace.Sum(c => c.Value)} placed";
            return placeBetResult;


        }



        private string ValidateBetInputs(List<PlaceBetOption> betsToPlace)
        {
            if (betsToPlace.Count > RouletteConstants.MAX_BETS_PER_ROUND) return $"Max Bets ({RouletteConstants.MAX_BETS_PER_ROUND}) exceeded, {betsToPlace.Count} is too many";

            var totalBetValue = betsToPlace.Sum(C => C.Value);

            if (totalBetValue > RouletteConstants.MAX_BET_VALUE) return $"Max Bets total value ({RouletteConstants.MAX_BET_VALUE}) exceeded, {totalBetValue} is too high";
            if (totalBetValue < RouletteConstants.MIN_BET_VALUE) return $"Min Bets total value ({RouletteConstants.MIN_BET_VALUE}) not reached, {totalBetValue} is too low";

            return string.Empty;
        }

        public async Task<SpinResult> Spin(Guid roundCorrelationId)
        {
            return await Spin(roundCorrelationId, SpinStrategyEnum.Random);
        }

        public async Task<SpinResult> Spin(Guid roundCorrelationId, SpinStrategyEnum spinStrategy)
        {
            var spinResult = new SpinResult();

            var rouletteRound = await this.appDBRepository.GetRoundByCorrelationId(roundCorrelationId);

            if (rouletteRound == null)
            {
                spinResult.Message = $"Roulette Round not found: '{roundCorrelationId}'";
                spinResult.isSuccess = false;
                return spinResult;
            }

            //Implement the spin Strategy
            var spinStrategyInstance = SpinStrategyFactory.Instance(spinStrategy);
            rouletteRound.SpinValue = spinStrategyInstance.SpinTheWheel();

            await this.appDBRepository.UpdateRoulleteRound(rouletteRound);

            spinResult.SpinValue = rouletteRound.SpinValue;
            spinResult.isSuccess = true;
            spinResult.Message = $"Roulette Round Spin Success: '{roundCorrelationId}', the result is '{spinResult.SpinValue}' ";

            return spinResult;
        }

        public async Task<PayoutResult> Payout(Guid roundCorrelationId)
        {
            var payoutResult = new PayoutResult
            {
                RoundCorrelationId = roundCorrelationId
            };

            var rouletteRound = await this.appDBRepository.GetRoundByCorrelationId(roundCorrelationId);

            if (rouletteRound == null)
            {
                payoutResult.Message = $"Roulette Round not found: '{roundCorrelationId}'";
                payoutResult.isSuccess = false;
                return payoutResult;
            }

            if (rouletteRound.SpinValue == null)
            {
                payoutResult.Message = $"Roulette Round Payout failed: '{roundCorrelationId}', user has not 'Spun the Wheel'";
                payoutResult.isSuccess = false;
                return payoutResult;
            }

            var placedBets = await this.appDBRepository.GetBetsForRound(rouletteRound.CorrelationId);

            if(placedBets == null || ! placedBets.Any())
            {
                payoutResult.Message = $"Roulette Round Payout failed: '{roundCorrelationId}', there are no bets to pay out'";
                payoutResult.isSuccess = false;
                return payoutResult;
            }

            foreach(var placedBet in placedBets)
            {
                var payoutyMap = await this.appDBRepository.GetPayoutMapforBetOptions(placedBet.Option, rouletteRound.SpinValue.Value);

                if (payoutyMap != null)                {

                    placedBet.HasPayout = true;
                    placedBet.PayoutValue = placedBet.Value * payoutyMap.PayoutMultiplier;
                }
            }

            await this.appDBRepository.SaveBets(roundCorrelationId, placedBets);



            if(payoutResult.Payouts.Count == 0 ) payoutResult.Message = $"No Payout for Round '{roundCorrelationId}',  Better Luck Next Time";
            else payoutResult.Message = $"{payoutResult.Payouts.Count} Payouts for Round '{roundCorrelationId}', to the sum of {payoutResult.Payouts.Sum(c => c.PayoutValue)}, Congratulations";

            payoutResult.Payouts = placedBets.Where(x => x.HasPayout == true).ToList();
            payoutResult.isSuccess = true;
            return payoutResult;


        }

        public async Task<IEnumerable<RoundDto>> GetPreviousRounds()
        {
            return await this.appDBRepository.GetPreviousRounds();
        }

        public async Task<RoundDto> GetPreviousRound(Guid roundCorellationId)
        {
            return await this.appDBRepository.GetPreviousRound(roundCorellationId);
        }

       

       
    }
}

