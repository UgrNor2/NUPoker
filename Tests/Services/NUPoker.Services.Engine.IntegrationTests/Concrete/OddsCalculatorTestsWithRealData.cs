using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.IntegrationTests.Concrete
{
    [TestClass]
    public class OddsCalculatorTestsWithRealData
    {

        [TestMethod]
        public void GetOddsTest_WithRealData()
        {
            // Given
            var input = new GetOddsTestInput
            {
                PlayerCards = new List<(Cards, Cards)> { (Cards.TwoDiamonds, Cards.SevenDiamonds), (Cards.FiveSpades, Cards.NineSpades), (Cards.SevenHearts, Cards.EightClubs) },
                FlopCard1 = Cards.AceClubs,
                FlopCard2 = Cards.KingClubs,
                FlopCard3 = Cards.QueenClubs,
                TurnCard = Cards.Empty,
                ExpectedWinRates = new double[][]
                {
                    new double[] { 5.65, 15.84 },
                    new double[] { 31.01, 12.96 },
                    new double[] { 47.51, 15.84 }
                }
            };
            var input2 = new GetOddsTestInput
            {
                PlayerCards = new List<(Cards, Cards)> { (Cards.QueenHearts, Cards.JackHearts), (Cards.FourDiamonds, Cards.FiveDiamonds), (Cards.AceSpades, Cards.AceDiamonds) },
                FlopCard1 = Cards.KingClubs,
                FlopCard2 = Cards.TenClubs,
                FlopCard3 = Cards.FourHearts,
                TurnCard = Cards.Empty,
                ExpectedWinRates = new double[][]
               {
                    new double[] { 28.13, 0.00 },
                    new double[] { 16.28, 0.00 },
                    new double[] { 55.59, 0.00 }
               }
            };

            TestRun(input);
            TestRun(input2);
        }

        private void TestRun(GetOddsTestInput input)
        {
            // Given
            double allCombination = (43 * 42) / 2;

            // When
            var calculator = new OddsCalculator(new HandService(), new CardValidator());
            var odds = calculator.GetOdds(input.PlayerCards.Select(i => ((int)i.Item1, (int)i.Item2)).ToList(), (int)input.FlopCard1, (int)input.FlopCard2, (int)input.FlopCard3, (int)input.TurnCard);

            for (int i = 0; i < input.PlayerCards.Count; i++)
            {
                Assert.AreEqual(input.ExpectedWinRates[i][0], ((odds[i].Take(9).Sum() / allCombination) * 100), 0.1);
                Assert.AreEqual(input.ExpectedWinRates[i][1], ((odds[i].Skip(9).Take(9).Sum() / allCombination) * 100), 0.1);
            }
        }


        private class GetOddsTestInput
        {
            public List<(Cards, Cards)> PlayerCards { get; init; }

            public Cards FlopCard1 { get; init; }

            public Cards FlopCard2 { get; init; }

            public Cards FlopCard3 { get; init; }

            public Cards TurnCard { get; init; }

            public double[][] ExpectedWinRates { get; init; }
        }
    }
}
