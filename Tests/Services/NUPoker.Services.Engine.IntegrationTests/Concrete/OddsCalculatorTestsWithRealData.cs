using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Data.OddsCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
                ExpectedResult = new List<GetOddsTestInput.ExpectedPlayerOdds>
                {
                    new GetOddsTestInput.ExpectedPlayerOdds
                    {
                        WinPercentage = 5.65,
                        TiePercentage = 15.84,
                        RankOccurence = new double[]{36.66, 47.73, 8.53, 1.44, 1.66, 3.88, 0, 0, 0.11}
                    },
                    new GetOddsTestInput.ExpectedPlayerOdds
                    {
                        WinPercentage = 31.01,
                        TiePercentage = 12.96,
                    },
                    new GetOddsTestInput.ExpectedPlayerOdds
                    {
                        WinPercentage = 47.51,
                        TiePercentage = 15.84,
                    }
                },
                ExpectedNumberOfPlayers = 3,
                ExpectedNumberOfCases = 903,

            };

            TestRun(input);
        }

        private void TestRun(GetOddsTestInput input)
        {
            // Given
            // When
            var calculator = new OddsCalculator(new HandService(), new CardValidator());
            var odds = calculator.GetOdds(input.PlayerCards.Select(i => ((int)i.Item1, (int)i.Item2)).ToList(), (int)input.FlopCard1, (int)input.FlopCard2, (int)input.FlopCard3, (int)input.TurnCard);

            // Then

            if(input.ExpectedNumberOfCases.HasValue)
            {
                Assert.AreEqual(input.ExpectedNumberOfCases.Value, odds.NumberOfCases);
            }

            if(input.ExpectedNumberOfPlayers.HasValue)
            {
                Assert.AreEqual(input.ExpectedNumberOfPlayers, odds.NumberOfPlayers);
            }

            for (int i = 0; i < input.PlayerCards.Count; i++)
            {
                Assert.AreEqual(input.ExpectedResult[i].WinPercentage, Math.Round(odds.PlayerOddsResults[i].GetWinPercentage(),2));
                Assert.AreEqual(input.ExpectedResult[i].TiePercentage, Math.Round(odds.PlayerOddsResults[i].GetTiePercentage(), 2));

                if (input.ExpectedResult[i].RankOccurence != null)
                {
                    var occurancePercentages = odds.PlayerOddsResults[i].GetOccurancePercentages();

                    for (int r = 0; r < input.ExpectedResult[i].RankOccurence.Length; r++)
                    {
                        Assert.AreEqual(input.ExpectedResult[i].RankOccurence[r], Math.Round(occurancePercentages[r], 2));
                    }
                }
            }
        }


        private class GetOddsTestInput
        {
            public List<(Cards, Cards)> PlayerCards { get; init; }

            public Cards FlopCard1 { get; init; }

            public Cards FlopCard2 { get; init; }

            public Cards FlopCard3 { get; init; }

            public Cards TurnCard { get; init; }

            public List<ExpectedPlayerOdds> ExpectedResult { get; init; }

            public int? ExpectedNumberOfPlayers { get; init; }

            public int? ExpectedNumberOfCases { get; init; }


            public class ExpectedPlayerOdds
            {
                public double WinPercentage { get; set; }

                public double TiePercentage { get; set; }

                public double[]? RankOccurence { get; set; }
            }
        }
    }
}
