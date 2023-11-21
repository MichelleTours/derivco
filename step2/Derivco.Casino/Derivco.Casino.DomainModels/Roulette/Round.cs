using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.DomainModels.Roulette
{
    
    public class Round
    {
        
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime? DateBetsPlaced { get; set; }


        public DateTime? DateWheelSpin { get; set; }

        public RouletteWheelBucket? SpinValue { get; set; }

        public DateTime? DateOfPayout { get; set; }

        public bool IsInPlay { get; set; }

        public List<Bet> PlacedBets { get; set; } = new List<Bet>();
    }
}
