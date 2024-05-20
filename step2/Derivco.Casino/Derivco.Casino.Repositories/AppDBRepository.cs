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
        private AppDBContext _context;



        public AppDBRepository(ILogger<AppDBRepository> logger,
            IMapper mapper,
            AppDBContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;


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

            
                await _context.Rounds.AddAsync(round);
                await _context.SaveChangesAsync();
            
        }

        public async Task<List<PlaceBetOption>> GetBetsForRound(Guid roundCorrelationId)
        {
            
                var bets = _context.Bets.Where(c => c.RoundCorrelationId == roundCorrelationId).ToList();
                return _mapper.Map<List<PlaceBetOption>>(bets);                   
            
        }

        public async Task<BetOptionsPaypoutMap> GetPayoutMapforBetOptions(RouletteBetOption placedBetOption, RouletteWheelBucket spinValue)
        {
           
                var payoutMap = await _context.PayoutMaps.FirstOrDefaultAsync(c => c.BetOption == placedBetOption && c.SpinValue == spinValue);

             //   if (payoutMap == null) throw new AppException($" The Combination Spin value'{spinValue}' and Placed Bet Option '{placedBetOption}' does not exist");

                return _mapper.Map< BetOptionsPaypoutMap>(payoutMap);
            
        }
        public async Task<RoundDto> GetPreviousRound(Guid roundCorellationId)
        {
           

                var round = await _context.Rounds
                    .Include(c => c.PlacedBets)                   
                    .FirstOrDefaultAsync(c => c.CorrelationId == roundCorellationId);

                if (round == null) throw new KeyNotFoundException($"{roundCorellationId}");

                return _mapper.Map<RoundDto>(round);

            
        }

        public async Task<IEnumerable<RoundDto>> GetPreviousRounds()
        {
            

                var rounds = await _context.Rounds
                    .Include(c => c.PlacedBets)                   
                    .OrderByDescending(c => c.DateStarted)
                    .ToListAsync();


                return _mapper.Map<List<RoundDto>>(rounds);

            
        }

        public async Task<NewRound> GetRoundByCorrelationId(Guid roundCorrelationId)
        {
           
                var round = await _context.Rounds
                    //.Include(c => c.PlacedBets)
                    .FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);

                if (round == null) throw new KeyNotFoundException($"{roundCorrelationId}");


                return _mapper.Map<NewRound>(round);

            
        }

       

        public async Task PlaceBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace)
        {
            List<Bet> betsToSave = new List<Bet>();
            _mapper.Map(betsToPlace, betsToSave);

           

                var round = await _context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);
                if (round == null) throw new KeyNotFoundException($"{roundCorrelationId}");

                foreach (var bet in betsToSave)
                {
                    bet.RoundId = round.Id;
                }

                await _context.Bets.AddRangeAsync(betsToSave);
                await _context.SaveChangesAsync();
            
        }

        public async Task SaveBets(Guid roundCorrelationId, List<PlaceBetOption> betsToPlace)
        {
           
                foreach (var bet in betsToPlace)
                {
                    var betToSave = await _context.Bets.FirstAsync(b => b.CorrelationId == bet.CorrelationId);
                    betToSave.PayoutValue = bet.PayoutValue;
                    betToSave.HasPayout = bet.HasPayout;
                    _context.Bets.Update(betToSave);
                }

                var round = await _context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == roundCorrelationId);
                round.DateOfPayout = DateTime.Now;
                _context.Rounds.Update(round);

                await _context.SaveChangesAsync();
            
        }

                
        public async Task UpdateRoulleteRound(NewRound rouletteRound)
        {
            
                 var round = await _context.Rounds.FirstOrDefaultAsync(c => c.CorrelationId == rouletteRound.CorrelationId);
                if(round == null)    throw new KeyNotFoundException();

                round.SpinValue = rouletteRound.SpinValue;

                _context.Rounds.Update(round);
                await _context.SaveChangesAsync();

            
        }

       
    }
}
