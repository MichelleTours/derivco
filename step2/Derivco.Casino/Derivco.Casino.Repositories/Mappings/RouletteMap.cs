using AutoMapper;
using Derivco.Casino.Data.Roulette;
using Derivco.Casino.DomainModels.Roulette;
using Derivco.Casino.DomainModels.Roulette.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Round = Derivco.Casino.DomainModels.Roulette.Round;

namespace Derivco.Casino.Repositories.Mappings
{
    public class RouletteProfile: Profile 
    {
        public RouletteProfile()
        {

            CreateMap<Bet, PlaceBetOption>().ReverseMap();
            CreateMap<Round, RoundDto>().ReverseMap();
            CreateMap<Bet, BetDto>().ReverseMap();
            CreateMap<Round, NewRound>().ReverseMap();
           


        }
    }
}
