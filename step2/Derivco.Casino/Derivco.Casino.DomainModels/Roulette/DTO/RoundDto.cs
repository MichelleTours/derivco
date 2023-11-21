using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.DomainModels.Roulette.DTO
{
    [NotMapped]
    public class RoundDto
    {       
        public Guid CorrelationId { get; set; }

        public DateTime DateStarted { get; set; }

        public DateTime? DateBetsPlaced { get; set; }


        public DateTime? DateWheelSpin { get; set; }

        public RouletteWheelBucket SpinValue { get; set; }

        public DateTime? DateOfPayout { get; set; }

        public bool IsInPlay { get; set; }

        public List<BetDto> PlacedBets { get; set; } = new List<BetDto>();
    }
}
