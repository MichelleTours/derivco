using Derivco.Casino.DomainModels.Roulette.DTO;
using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.DomainModels.Roulette
{
    public class Bet
    {
        
        public long Id { get; set; }

        public Guid CorrelationId { get; set; }
        public Guid RoundCorrelationId { get; set; }

        //Backing Property For Round Entity
        public long RoundId { get; set; }
        public Round Round { get; set; }


        public RouletteBetOption Option { get; set; }

        public decimal Value { get; set; }

        public bool HasPayout { get; set; }

        public decimal PayoutValue { get; set; }
    }
}
