using Derivco.Casino.Shared.Enums;

namespace Derivco.Casino.Data.Roulette
{
    public class BetOptionsPaypoutMap
    {

        public RouletteWheelBucket RouletteWheelBucket { get; set; }
        public RouletteBetOption RouletteBetOption { get; set; }

        public int PayoutMultiplier { get; set; }

    }


}
