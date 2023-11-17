using Derivco.Casino.Data.Roulette;
using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Derivco.Casino.Repositories.Interfaces
{
    public interface IAppDBRepository
    {
        Task CreateNewRound(Guid correlationId);
        Task<List<PlaceBetOption>> GetBetsForRound(Guid correlationId);
        Task<BetOptionsPaypoutMap> GetPayoutMapforBetOptions(RouletteBetOption placedBetOption, RouletteWheelBucket spinValue);
        Task<Round> GetRoundByCorrelationId(Guid correlationId);
        Task PlaceBets(List<PlaceBetOption> betsToPlace);
        Task SavePayouts(List<PlaceBetPayout> payouts);
        Task UpdateRoulleteRound(Round rouletteRound);
    }
}
