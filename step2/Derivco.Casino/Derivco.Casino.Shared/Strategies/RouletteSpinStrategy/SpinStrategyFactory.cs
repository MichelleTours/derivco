using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Strategies.RouletteSpinStrategy
{
    public class SpinStrategyFactory
    {

        public static SpinStrategy Instance(SpinStrategyEnum strategy)
        {
            switch (strategy)
            {
                case SpinStrategyEnum.Loaded_Red: return new RedSpinStrategy();
                case SpinStrategyEnum.Loaded_Third_Twelve: return new ThirdTwelveSpinStrategy();
                case SpinStrategyEnum.Random: 
                default: return new RandomSpinStrategy();
            }

        }
    }
}
