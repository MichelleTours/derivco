using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Data.Roulette
{
    public class PlaceBetResult
    {
        public bool isSuccess { get; set; }
        public string? Message { get; set; }

        public List<PlaceBetOption> PlacedBets { get; set; } = new List<PlaceBetOption>();
    }
}
