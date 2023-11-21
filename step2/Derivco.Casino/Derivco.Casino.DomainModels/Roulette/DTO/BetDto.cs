using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.DomainModels.Roulette.DTO
{
    [NotMapped]
    public class BetDto
    {
      
        public Guid CorrelationId { get; set; }
        public Guid RoundCorrelationId { get; set; }

        //Backing Property For Round Entity
        public long RoundId { get; set; }
            

        public RouletteBetOption Option { get; set; }

        public decimal Value { get; set; }

        public bool HasPayout { get; set; }

        public decimal PayoutValue { get; set; }
    }
}
