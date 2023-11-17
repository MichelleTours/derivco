using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Data.Roulette
{
    public class PayoutResult
    {
        public Guid RoundCorrelationId { get; set; }

        public bool isSuccess { get; set; }
        public string? Message { get; set; }

        public List<PlaceBetPayout> Payouts { get; set; } = new List<PlaceBetPayout>();
    }
}
