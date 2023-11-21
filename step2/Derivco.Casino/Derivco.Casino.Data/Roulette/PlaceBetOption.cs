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

        public Guid RoundCorrelationId { get; set; }

        public Guid CorrelationId { get; set; }

        public RouletteBetOption Option { get; set; }

        public decimal Value { get; set; }

        public bool HasPayout { get; set; }

        public decimal PayoutValue{ get; set; }
    }
}
