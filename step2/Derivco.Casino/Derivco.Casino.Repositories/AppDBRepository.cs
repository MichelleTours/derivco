using AutoMapper;
using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Repositories.Interfaces;
using Derivco.Casino.Shared.Enums;
using Derivco.Casino.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Derivco.Casino.Repositories
{
    public class AppDBRepository : IAppDBRepository
    {

        private readonly IDbContextFactory<AppDBContext> _contextFactory;
        private readonly ILogger<AppDBRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AppDBRepository(ILogger<AppDBRepository> logger,
            IMapper mapper,
            IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task CreateNewRound(Guid correlationId)
        {
            var round = new Round
            {
                CorrelationId = correlationId,
                DateStarted = DateTime.UtcNow,
                IsInPlay = true,
                SpinValue = null
            };

            using (var context = new AppDBContext(_configuration))    /// contextFactory.CreateDbContext())
            {
                await context.Rounds.AddAsync(round);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<PlaceBetOption>> GetBetsForRound(Guid roundCorrelationId)
        {
            using (var context = new AppDBContext(_configuration))
            {
                var bets =context.Bets.Where(c => c.RoundCorrelationId == roundCorrelationId).ToList();
                return _mapper.Map<List<PlaceBetOption>>(bets);                   
            }
        }

        public async Task<BetOptionsPaypoutMap> GetPayoutMapforBetOptions(RouletteBetOption placedBetOption, RouletteWheelBucket spinValue)
        {
            using (var context = new AppDBContext(_configuration))
            {
                var payoutMap = await context.PayoutMaps.FirstOrDefaultAsync(c => c.BetOption == placedBetOption && c.SpinValue == spinValue);

             //   if (payoutMap == null) throw new AppException($" The Combination Spin value'{spinValue}' and Placed Bet Option '{placedBetOption}' does not exist");

                return _mapper.Map< BetOptionsPaypoutMap>(payoutMap);
            }
        }
        public async Task<RoundDto> GetPreviousRound(Guid roundCorellationId)
        {
            using (var context = new AppDBContext(_configuration))
            {

                var round = await context.Rounds
                    .Include(c => c.PlacedBets)                   
                    .FirstOrDefaultAsync(c => c.CorrelationId == roundCorellationId);

                if (round == null) throw new KeyNotFoundException($"{roundCorellationId}");

                return _mapper.Map<RoundDto>(round);

            }
        }

        public async Task<IEnumerable<RoundDto>> GetPreviousRounds()
        {
            using (var context = new AppDBContext(_configuration))    /// contextFactory.CreateDbContext())
            {

                var rounds = await context.Rounds
                    .Include(c => c.PlacedBets)                   
                    .OrderByDescending(c => c.DateStarted)
                    .ToListAsync();


                return _mapper.Map<List<RoundDto>>(rounds);

            }
        }

        public async Task<NewRound> GetRoundByCorrelationId(Guid roundCorrelationId)
        {
            using (var context = new AppDBContext(_configuration))
            {

                var round = await context.Rounds
                    //.Include(c => c.PlacedBets)
                    .FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);

                if (round == null) throw new KeyNotFoundException($"{roundCorrelationId}");


                return _mapper.Map<NewRound>(round);

            }
        }

       

        public async Task PlaceBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace)
        {
            List<Bet> betsToSave = new List<Bet>();
            _mapper.Map(betsToPlace, betsToSave);

            using (var context = new AppDBContext(_configuration))    
            {

                var round = await context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);
                if (round == null) throw new KeyNotFoundException($"{roundCorrelationId}");

                foreach (var bet in betsToSave)
                {
                    bet.RoundId = round.Id;
                }

                await context.Bets.AddRangeAsync(betsToSave);
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace)
        {
            using (var context = new AppDBContext(_configuration))
            {
                foreach (var bet in betsToPlace)
                {
                    var betToSave = await context.Bets.FirstAsync(b => b.CorrelationId == bet.CorrelationId);
                    betToSave.PayoutValue = bet.PayoutValue;
                    betToSave.HasPayout = bet.HasPayout;
                    context.Bets.Update(betToSave);
                }

                var round = await context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);
                round.DateOfPayout = DateTime.Now;
                context.Rounds.Update(round);

                await context.SaveChangesAsync();
            }
        }

                
        public async Task UpdateRoulleteRound(NewRound rouletteRound)
        {
            using (var context = new AppDBContext(_configuration))
            {
                 var round = await context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == rouletteRound.CorrelationId);
                if(round == null)    throw new KeyNotFoundException();

                round.SpinValue = rouletteRound.SpinValue;

                context.Rounds.Update(round);
                await context.SaveChangesAsync();

            }
        }

       
    }
}
