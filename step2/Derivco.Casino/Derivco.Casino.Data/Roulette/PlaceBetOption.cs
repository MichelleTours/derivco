using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Data.Roulette
{
    public class PlaceBetOption
    {

        public Guid RoomCorrelationId { get; set; }

        public Guid BetCorrelationId { get; set; }

        public RouletteBetOption RouletteBetOption { get; set; }

        public decimal Value { get; set; }
    }
}
