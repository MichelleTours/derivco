using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Shared.Enums
{

    public enum RouletteBetOption
    {
        zero = 0,
        one,
        two, three, four, five, six, seven,
        eight, nine, ten, eleven, twelve,
        thirteen, fourteen, fiveteen, sixteen, seventeen,
        eighteen, nineteen, twenty, twentyOne, twentyTwo,
        twentyThree, twentyFour, twentyFive, twentySix, twentySeven,
        twentyEight, twentyNine, thirty, thirtyOne, thirtyTwo, thirtyThree,
        thirtyFour, thirtyFive, thirtySix,
        red, black,
        odd, even,
        High, low,
        firstTwelve, secondTwelve, thirdTwelve
    }

    public enum RouletteWheelBucket
    {
        zero = 0,
        one,
        two, three, four, five, six, seven,
        eight, nine, ten, eleven, twelve,
        thirteen, fourteen, fiveteen, sixteen, seventeen,
        eighteen, nineteen, twenty, twentyOne, twentyTwo,
        twentyThree, twentyFour, twentyFive, twentySix, twentySeven,
        twentyEight, twentyNine, thirty, thirtyOne, thirtyTwo, thirtyThree,
        thirtyFour, thirtyFive, thirtySix
    }

    public enum SpinStrategyEnum
    { 
        Random = 0,
        Loaded_Red, 
        Loaded_Third_Twelve
    }
}