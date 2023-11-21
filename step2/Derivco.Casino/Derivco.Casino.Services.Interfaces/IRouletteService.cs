using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Shared.Enums;

namespace Derivco.Casino.Services.Interfaces
{
    public interface IRouletteService
    {
        Task<BeginRoundResult> BeginRound();
        Task<IEnumerable<RoundDto>> GetPreviousRounds();
        Task<RoundDto> GetPreviousRound(Guid roundCorellationId);
        Task<PlaceBetResult> PlaceBet(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace);
        Task<SpinResult> Spin(Guid roundCorrelationId);
        Task<SpinResult> Spin(Guid roundCorrelationId, SpinStrategyEnum spinStrategy);

        Task<PayoutResult> Payout(Guid roundCorrelationId);
    }
}
