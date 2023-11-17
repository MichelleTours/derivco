using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Strategies.RouletteSpinStrategy
{
    public abstract class SpinStrategy
    {
        public abstract RouletteWheelBucket SpinTheWheel();
    }
}
