using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NUPoker.Services.Engine.IntegrationTests.Concrete
{
    [TestClass]
    public class OddsCalculatorTestsWithRealData
    {
        [TestMethod]
        [DynamicData(nameof(OddsCalculatorInputs))]
        public void GetOddsTest_WithRealData(GetOddsTestInput input)
        {
            // Given
            // When
            var calculator = new OddsCalculator(new HandService(), new CardValidator(), new BoardEnumerator(new CardValidator()));
            var odds = calculator.GetOdds(input.PlayerCards.Select(i => ((int)i.Item1, (int)i.Item2)).ToList(), (int)input.FlopCard1, (int)input.FlopCard2, (int)input.FlopCard3, (int)input.TurnCard);

            // Then

            if (input.ExpectedNumberOfCases.HasValue)
            {
                Assert.AreEqual(input.ExpectedNumberOfCases.Value, odds.NumberOfCases);
            }

            if (input.ExpectedNumberOfPlayers.HasValue)
            {
                Assert.AreEqual(input.ExpectedNumberOfPlayers, odds.NumberOfPlayers);
            }

            for (int i = 0; i < input.PlayerCards.Count; i++)
            {
                Assert.AreEqual(input.ExpectedResult[i].WinPercentage, Math.Round(odds.PlayerOddsResults[i].GetWinPercentage(), 2));
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

        /// <summary>
        /// Results are coming from these sources:
        /// - https://www.pokernews.com/poker-tools/poker-odds-calculator.htm
        /// - https://www.cardplayer.com/poker-tools/odds-calculator/texas-holdem
        /// </summary>
        public static IEnumerable<object[]> OddsCalculatorInputs
        {
            get
            {
                return new []
                {
                    // 1.
                    new object[]
                    {
                        new GetOddsTestInput
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
                                    RankOccurence = new double[] { 36.66, 47.73, 8.53, 1.44, 1.66, 3.88, 0, 0, 0.11 }
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
                        }
                    },
                    // 2.
                    new object[]
                    {
                         new GetOddsTestInput
                         {
                            PlayerCards = new List<(Cards, Cards)> { (Cards.QueenSpades, Cards.FiveClubs), (Cards.AceClubs, Cards.TwoDiamonds), (Cards.JackClubs, Cards.TenClubs), (Cards.TenSpades, Cards.FiveDiamonds) },
                            FlopCard1 = Cards.SevenSpades,
                            FlopCard2 = Cards.SixSpades,
                            FlopCard3 = Cards.TwoHearts,
                            TurnCard = Cards.Empty,
                            ExpectedResult = new List<GetOddsTestInput.ExpectedPlayerOdds>
                            {
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 22.44,
                                    TiePercentage = 3.66,
                                    RankOccurence = new double[] { 33.54, 47.2, 8.05, 1.34, 5.49, 4.39, 0, 0, 0 }
                                },
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 54.39,
                                    TiePercentage = 0,
                                },
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 18.9,
                                    TiePercentage = 0,
                                },
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 0.61,
                                    TiePercentage = 3.66,
                                }
                            },
                            ExpectedNumberOfPlayers = 4,
                            ExpectedNumberOfCases = 820,
                         }
                    },
                    // 3.
                    new object[]
                    {
                         new GetOddsTestInput
                         {
                            PlayerCards = new List<(Cards, Cards)> { (Cards.QueenSpades, Cards.FiveClubs), (Cards.AceClubs, Cards.TwoDiamonds) },
                            FlopCard1 = Cards.SevenSpades,
                            FlopCard2 = Cards.SixSpades,
                            FlopCard3 = Cards.TwoHearts,
                            TurnCard = Cards.Empty,
                            ExpectedResult = new List<GetOddsTestInput.ExpectedPlayerOdds>
                            {
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 30.40,
                                    TiePercentage = 0,
                                    RankOccurence = new double[] { 35.05, 46.77, 7.78, 1.31, 4.55, 4.55, 0, 0, 0 }
                                },
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 69.60,
                                    TiePercentage = 0,
                                    RankOccurence = new double[] { 0, 51.82, 38.48, 6.87, 0, 0,2.73, 0.1, 0 }
                                }
                            },
                            ExpectedNumberOfPlayers = 2,
                            ExpectedNumberOfCases = 990,
                         }
                    },
                    // 4.
                    new object[]
                    {
                         new GetOddsTestInput
                         {
                            PlayerCards = new List<(Cards, Cards)> { (Cards.QueenSpades, Cards.FiveClubs), (Cards.AceClubs, Cards.TwoDiamonds) },
                            FlopCard1 = Cards.Empty,
                            FlopCard2 = Cards.Empty,
                            FlopCard3 = Cards.Empty,
                            TurnCard = Cards.Empty,
                            ExpectedResult = new List<GetOddsTestInput.ExpectedPlayerOdds>
                            {
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 39.17,
                                    TiePercentage = 0.55,
                                    RankOccurence = new double[] { 18.55, 45.39, 23.35, 4.62, 3.62, 1.93, 2.39, 0.14, 0.02}
                                },
                                new GetOddsTestInput.ExpectedPlayerOdds
                                {
                                    WinPercentage = 60.28,
                                    TiePercentage = 0.55,
                                    RankOccurence = new double[] { 19.27, 45.5, 23.24, 4.6, 2.92, 1.94, 2.39, 0.14, 0.01 }
                                }
                            },
                            ExpectedNumberOfPlayers = 2,
                            ExpectedNumberOfCases = 1712304,
                         }
                    },
                };
            }
        }


        public class GetOddsTestInput
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
