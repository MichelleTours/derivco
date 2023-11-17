﻿using Derivco.Casino.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Strategies.RouletteSpinStrategy
{
    public class ThirdTwelveSpinStrategy : SpinStrategy
    {
        /* Third Twelve Numbers
                   1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, and 36
            */
        public static List<RouletteWheelBucket> ThirdTwelveNumbers =
            [
                RouletteWheelBucket.twentyFive,
                RouletteWheelBucket.twentySix,
                RouletteWheelBucket.twentySeven,
                RouletteWheelBucket.twentyEight,
                RouletteWheelBucket.twentyNine,
                RouletteWheelBucket.thirty,
                RouletteWheelBucket.thirtyOne,
                RouletteWheelBucket.thirtyTwo,
                RouletteWheelBucket.thirtyThree,
                RouletteWheelBucket.thirtyFour,
                RouletteWheelBucket.thirtyFive,
                RouletteWheelBucket.thirtySix
            ];

        

        public override RouletteWheelBucket SpinTheWheel()
        {
            Random r = new Random();

            var thirdTwelveResult = ThirdTwelveNumbers[r.Next(0, ThirdTwelveNumbers.Count - 1)];
            Debug.WriteLine(thirdTwelveResult);

            return thirdTwelveResult;

        }
    }
}
