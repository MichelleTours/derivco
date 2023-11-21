using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Derivco.Casino.Repositories.Interfaces
{
    public interface IAppDBRepository
    {
        Task CreateNewRound(Guid correlationId);
        Task<List<PlaceBetOption>> GetBetsForRound(Guid roundCorrelationId);
        Task<BetOptionsPaypoutMap> GetPayoutMapforBetOptions(RouletteBetOption placedBetOption, RouletteWheelBucket spinValue);
        
        Task<NewRound> GetRoundByCorrelationId(Guid roundCorrelationId);
        Task<RoundDto> GetPreviousRound(Guid roundCorellationId);
        Task<IEnumerable<RoundDto>> GetPreviousRounds();
        Task PlaceBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace);
        Task SaveBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace);
        Task UpdateRoulleteRound(NewRound rouletteRound);
    }
}
