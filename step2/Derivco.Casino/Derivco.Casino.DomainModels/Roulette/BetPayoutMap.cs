using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.DomainModels.Roulette
{
    public class BetPayoutMap
    {
        
        public long Id { get; set; }
        public RouletteBetOption  BetOption  { get; set; }
        public RouletteWheelBucket SpinValue { get; set; }

        public decimal Multiplier { get; set; } //Effectivly odds turns into a Value to multiple the bet value from 

    }
}
