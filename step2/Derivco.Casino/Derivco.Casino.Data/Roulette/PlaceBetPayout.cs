using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Data.Roulette
{
    public class PlaceBetPayout    
    {

        public Guid RoomCorrelationId { get; set; }

        public Guid BetCorrelationId { get; set; }

        public Guid PayoutCorrelationId { get; set; }
               
        public decimal PayoutValue { get; set; }
    }
}
