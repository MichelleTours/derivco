using Derivco.Casino.Data.Roulette;
using Derivco.Casino.Repositories.Interfaces;
using Derivco.Casino.Shared.Enums;

namespace Derivco.Casino.Repositories
{
    public class AppDBRepository : IAppDBRepository
    {
        public async Task CreateNewRound(Guid correlationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PlaceBetOption>> GetBetsForRound(Guid correlationId)
        {
            throw new NotImplementedException();
        }

        public Task<BetOptionsPaypoutMap> GetPayoutMapforBetOptions(RouletteBetOption placedBetOption, RouletteWheelBucket spinValue)
        {
            throw new NotImplementedException();
        }

        public async Task<Round> GetRoundByCorrelationId(Guid correlationId)
        {
            throw new NotImplementedException();
        }

        public async Task PlaceBets(List<PlaceBetOption> betsToPlace)
        {
            throw new NotImplementedException();
        }

        public Task SavePayouts(List<PlaceBetPayout> payouts)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRoulleteRound(Round rouletteRound)
        {
            throw new NotImplementedException();
        }
    }
}
