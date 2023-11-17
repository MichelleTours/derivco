using Moq;
using NUnit.Framework.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NuGet.Frameworks;
using Derivco.Casino.Repositories.Interfaces;
using Derivco.Casino.Data.Roulette;
using System.Diagnostics;
using Derivco.Casino.Shared.Enums;
using Derivco.Casino.Shared.Strategies.RouletteSpinStrategy;
using System.Linq;

namespace Derivco.Casino.Services.Tests
{
    public class RouletteServiceTests
    {
        private RouletteService serviceUnderTest;
        private Mock<ILogger<RouletteService>> loggerMock;
        private Mock<IAppDBRepository> appDBrepositoryMock;

        private Guid roundCorrelationId;

        List<PlaceBetOption> betsToPlaceSingle;
        List<PlaceBetOption> betsToPlaceMultiple;

        List<PlaceBetOption> betsToPlace_TooManyBets;
        List<PlaceBetOption> betsToPlace_ValueTooHigh;
        List<PlaceBetOption> betsToPlace_ValueTooLow;

        [SetUp]
        public void Setup()
        {
            appDBrepositoryMock = new Mock<IAppDBRepository> ();
            loggerMock = new Mock<ILogger<RouletteService>>();
            serviceUnderTest = new RouletteService(loggerMock.Object, appDBrepositoryMock.Object);

            this.roundCorrelationId = Guid.NewGuid();

            betsToPlaceSingle = new List<PlaceBetOption>
            {
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.eight, Value = 1000}
            };

            betsToPlaceMultiple = new List<PlaceBetOption>
            {
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.eight, Value = 1000},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.red, Value = 200},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.firstTwelve, Value = 800},

            };

            betsToPlace_TooManyBets = new List<PlaceBetOption>
            {
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.eight, Value = 1000},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.red, Value = 200},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.firstTwelve, Value = 800},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.fiveteen, Value = 100},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.secondTwelve, Value = 300},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.thirtySix, Value = 150},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.three, Value = 20},
            };

            betsToPlace_ValueTooHigh = new List<PlaceBetOption>
            {
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.eight, Value = 1000},
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.black, Value = 7000}
            };

            betsToPlace_ValueTooLow = new List<PlaceBetOption>
            {
                new PlaceBetOption{ RouletteBetOption = Shared.Enums.RouletteBetOption.eight, Value = 20}
                
            };

        }

        [Test]
        public void Test_Begin_Round()
        {                       
            var beginRoundResult = serviceUnderTest.BeginRound().Result;
           
            Assert.IsNotNull(beginRoundResult.RoundCorrelationId);
            this.appDBrepositoryMock.Verify(c => c.CreateNewRound(It.IsAny<Guid>()), Times.Exactly(1));
        }

        [Test]
        public void Test_PlaceBet()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

          
            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlaceSingle).Result;

            Assert.IsTrue(placeBetResult.isSuccess);
            Assert.IsTrue(placeBetResult.PlacedBets.Count == 1);

            foreach (var placedBet in placeBetResult.PlacedBets)
            {
                Assert.IsNotNull(placedBet.BetCorrelationId);
            }

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));
            this.appDBrepositoryMock.Verify(c => c.PlaceBets(betsToPlaceSingle), Times.Exactly(1));

            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_PlaceMultipleBets()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });
            
            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlaceMultiple).Result;

            Assert.IsTrue(placeBetResult.isSuccess);
            Assert.IsTrue(placeBetResult.PlacedBets.Count == betsToPlaceMultiple.Count);

            foreach (var placedBet in placeBetResult.PlacedBets)
            {
                Assert.IsNotNull(placedBet.BetCorrelationId);
            }

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));
            this.appDBrepositoryMock.Verify(c => c.PlaceBets(betsToPlaceMultiple), Times.Exactly(1));


            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_PlaceBet_InvalidRoundId()
        {           

            Round nullRound = null;

            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(It.IsAny<Guid>())).ReturnsAsync(nullRound);
                       
            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlaceSingle).Result;

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsTrue(placeBetResult.isSuccess == false);
            Assert.IsTrue( ! string.IsNullOrEmpty(placeBetResult.Message));
            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_PlaceBet_BetValueTooSmall()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlace_ValueTooLow).Result;

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsTrue(placeBetResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(placeBetResult.Message));
            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_PlaceBet_BetValueTooHigh()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlace_ValueTooHigh).Result;

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsTrue(placeBetResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(placeBetResult.Message));
            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_PlaceBet_MaximumNumberOfBetsExceeded()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var placeBetResult = serviceUnderTest.PlaceBet(this.roundCorrelationId, betsToPlace_TooManyBets).Result;

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsTrue(placeBetResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(placeBetResult.Message));

            Debug.WriteLine(placeBetResult.Message);
        }

        [Test]
        public void Test_Spin()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var spinResult = serviceUnderTest.Spin(this.roundCorrelationId).Result;

            this.appDBrepositoryMock.Verify(c => c.GetRoundByCorrelationId(It.IsAny<Guid>()), Times.Exactly(1));

            Assert.IsTrue(spinResult.isSuccess);
            Debug.WriteLine(spinResult.Message);
        }

        [Test]
        public void Test_Spin_Loaded_Red()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var spinResult = serviceUnderTest.Spin(this.roundCorrelationId, SpinStrategyEnum.Loaded_Red).Result;

            Assert.IsTrue(spinResult.isSuccess);
            Debug.WriteLine(spinResult.Message);

            Assert.That(RedSpinStrategy.RedNumbers.Contains(spinResult.SpinValue.Value)) ;

        }

        [Test]
        public void Test_Spin_Loaded_Third_Twelve()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId)).ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId });

            var spinResult = serviceUnderTest.Spin(this.roundCorrelationId, SpinStrategyEnum.Loaded_Third_Twelve).Result;

            Assert.IsTrue(spinResult.isSuccess);
            Debug.WriteLine(spinResult.Message);

            Assert.That(ThirdTwelveSpinStrategy.ThirdTwelveNumbers.Contains(spinResult.SpinValue.Value));
        }

        [Test]
        public void Test_Spin_InvalidRoundId()
        {
            Round nullRound = null;

            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(It.IsAny<Guid>())).ReturnsAsync(nullRound);

            var spinResult = serviceUnderTest.Spin(this.roundCorrelationId).Result;

            Assert.IsTrue(spinResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(spinResult.Message));
            Debug.WriteLine(spinResult.Message);
        }

        [Test]
        public void Test_Payout()
        {
            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId))
                .ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId, SpinValue = RouletteWheelBucket.eight});

            appDBrepositoryMock.Setup(c => c.GetBetsForRound(this.roundCorrelationId)).ReturnsAsync(betsToPlaceSingle);

            var betMultiplier = 36;

            appDBrepositoryMock.Setup(c => c.GetPayoutMapforBetOptions(betsToPlaceSingle.First().RouletteBetOption, RouletteWheelBucket.eight))
                .ReturnsAsync(new BetOptionsPaypoutMap 
                { 
                    RouletteWheelBucket = RouletteWheelBucket.eight,  
                    RouletteBetOption = betsToPlaceSingle.First().RouletteBetOption,
                    PayoutMultiplier = betMultiplier
                });

            var expectedBetValue = betsToPlaceSingle.First().Value * betMultiplier;

            var payoutResult = serviceUnderTest.Payout(this.roundCorrelationId).Result;

            Assert.IsTrue(payoutResult.isSuccess);
            Debug.WriteLine(payoutResult.Message);

            Assert.That(payoutResult.Payouts.Count == 1);
            Assert.That(payoutResult.Payouts.Sum(c => c.PayoutValue) ==expectedBetValue);
        }

    
        [Test]
        public void Test_Payout_InvalidRoundId()
        {
            Round nullRound = null;

            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(It.IsAny<Guid>())).ReturnsAsync(nullRound);

            var payoutResult = serviceUnderTest.Payout(this.roundCorrelationId).Result;

            Assert.IsTrue(payoutResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(payoutResult.Message));
            Debug.WriteLine(payoutResult.Message);
        }

        [Test]
        public void Test_Payout_RoundWithNoSpin()
        {
           

            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(It.IsAny<Guid>())).ReturnsAsync(new Round());

            var payoutResult = serviceUnderTest.Payout(this.roundCorrelationId).Result;

            Assert.IsTrue(payoutResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(payoutResult.Message));
            Debug.WriteLine(payoutResult.Message);
        }

        [Test]
        public void Test_Payout_RoundWithNoBets()
        {
            List <PlaceBetOption> emptyBets = new List<PlaceBetOption>();

            appDBrepositoryMock.Setup(c => c.GetRoundByCorrelationId(this.roundCorrelationId))
               .ReturnsAsync(new Round { CorrelationId = this.roundCorrelationId, SpinValue = RouletteWheelBucket.eight });

            appDBrepositoryMock.Setup(c => c.GetBetsForRound(this.roundCorrelationId)).ReturnsAsync(emptyBets);

            var payoutResult = serviceUnderTest.Payout(this.roundCorrelationId).Result;

            Assert.IsTrue(payoutResult.isSuccess == false);
            Assert.IsTrue(!string.IsNullOrEmpty(payoutResult.Message));
            Debug.WriteLine(payoutResult.Message);
        }

        [Test]
        [Ignore("Nothing to test")]
        public void Test_ShowPreviousSpins()
        {
            Assert.True(true);
        }

    }
}