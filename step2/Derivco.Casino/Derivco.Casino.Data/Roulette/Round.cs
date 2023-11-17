using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Data.Roulette
{
    public class Round
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime DateTime { get; set; }

        public RouletteWheelBucket? SpinValue { get; set; }
    }
}
