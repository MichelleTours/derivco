using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Strategies.RouletteSpinStrategy
{
    public class RandomSpinStrategy : SpinStrategy
    {
        public override RouletteWheelBucket SpinTheWheel()
        {
           Random r = new Random();
           
            var result = (RouletteWheelBucket)r.Next(0, Enum.GetNames(typeof(RouletteWheelBucket)).Length - 1);
            Debug.WriteLine(result);

            return result;

        }
    }
}
