using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Strategies.RouletteSpinStrategy
{
    public class RedSpinStrategy : SpinStrategy
    {

        /* Red Numbers
             1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, and 36
            */
        public static List<RouletteWheelBucket> RedNumbers =
            [
                RouletteWheelBucket.one,
                RouletteWheelBucket.three,
                RouletteWheelBucket.five,
                RouletteWheelBucket.seven,
                RouletteWheelBucket.nine,
                RouletteWheelBucket.twelve,
                RouletteWheelBucket.fourteen,
                RouletteWheelBucket.sixteen,
                RouletteWheelBucket.eighteen,
                RouletteWheelBucket.nineteen,
                RouletteWheelBucket.twentyOne,
                RouletteWheelBucket.twentyThree,
                RouletteWheelBucket.twentyFive,
                RouletteWheelBucket.twentySeven,
                RouletteWheelBucket.thirty,
                RouletteWheelBucket.thirtyTwo,
                RouletteWheelBucket.thirtyFour,
                RouletteWheelBucket.thirtySix,
            ];


        public override RouletteWheelBucket SpinTheWheel()
        {
            Random r = new Random();

            var redResult = RedNumbers[r.Next(0, RedNumbers.Count - 1)];
            Debug.WriteLine(redResult);

            return redResult;

        }
    }
}
