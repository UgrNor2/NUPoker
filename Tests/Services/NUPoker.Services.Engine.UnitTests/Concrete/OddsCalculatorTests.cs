using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Data.OddsCalculator;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.UnitTests.Exception;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class OddsCalculatorTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Strict);
        private Mock<IHandService> _handService = new Mock<IHandService>();
        private Mock<ICardValidator> _cardValidator = new Mock<ICardValidator>();

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _handService = _mockRepository.Create<IHandService>();
            _cardValidator = _mockRepository.Create<ICardValidator>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockRepository.Verify();
        }

        private OddsCalculator CreateOddsCalculator()
        {
            return new OddsCalculator(_handService.Object, _cardValidator.Object);
        }

        [TestMethod]
        public void ThrowExceptionIfPlayerCardsIsEmpty()
        {
            // Given

            // When
            var calculator = CreateOddsCalculator();
            var exception = Assert.ThrowsException<ArgumentException>(() => calculator.GetOdds(new List<(int, int)>(), 1, 2, 3));

            // Then
            Assert.AreEqual("playerCards list cannot be empty. (Parameter 'playerCards')", exception.Message);
        }

        [TestMethod]
        public void ThrowExceptionIfPlayerCardsHasOnlyOneElement()
        {
            // Given

            // When
            var calculator = CreateOddsCalculator();
            var exception = Assert.ThrowsException<ArgumentException>(() => calculator.GetOdds(new List<(int, int)> { (4, 5) }, 1, 2, 3));

            // Then
            Assert.AreEqual("playerCards should have min 2 elements. (Parameter 'playerCards')", exception.Message);
        }

        [TestMethod]
        public void ThrowExceptionIfPlayerCardsHasMoreThanElements()
        {
            // Given

            // When
            var calculator = CreateOddsCalculator();
            var exception = Assert.ThrowsException<ArgumentException>(() => calculator.GetOdds(new List<(int, int)> { (4, 5), (6, 7), (8, 9), (10, 11), (12, 13), (14, 15), (16, 17), (18, 19), (20, 21), (22, 23), (24, 25) }, 1, 2, 3));

            // Then
            Assert.AreEqual("playerCards should have max 10 elements. (Parameter 'playerCards')", exception.Message);
        }

        [TestMethod]
        public void ValidationRunsForAllCards_1()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(3, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(4, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(5, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(6, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(7, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(8, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(9, true)).Callback(() => { });

            _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new ExpectedTestException());

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5) }, 6, 7, 8, 9));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
        }

        [TestMethod]
        public void ValidationRunsForAllCards_2()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(3, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(4, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(5, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(6, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(7, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(8, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new ExpectedTestException());

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5) }, 6, 7, 8));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
        }

        [TestMethod]
        public void ValidationRunsForAllCards_3()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(3, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(4, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(5, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(6, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(7, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new ExpectedTestException());

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5) }, 6, 7));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
        }

        [TestMethod]
        public void ValidationRunsForAllCards_4()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(3, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(4, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(5, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(6, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new ExpectedTestException());

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5)}, 6));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true), Times.Exactly(1));
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, false), Times.Exactly(2));
        }

        [TestMethod]
        public void ValidationRunsForAllCards_5()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(3, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(4, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(5, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                        .Throws(new ExpectedTestException());

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5) }));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true), Times.Exactly(4));
        }

        //[TestMethod]
        //public void GetOddsTest()
        //{
        //    // Given
        //    List<(int, int)> playerCards = new List<(int, int)> { ((int)Cards.AceClubs, (int)Cards.AceDiamonds), ((int)Cards.KingClubs, (int)Cards.KingDiamonds), ((int)Cards.JackClubs, (int)Cards.JackDiamonds) };
        //    int flopCard1 = (int)Cards.EightDiamonds;
        //    int flopCard2 = (int)Cards.EightHearts;
        //    int flopCard3 = (int)Cards.EightSpades;

        //    Random rnd = new Random();

        //    int[][] expectedNumberOfOccurences = new int[3][];
        //    int[][] expectedNumberOfWins = new int[3][];
        //    int[][] expectedNumberOfTies = new int[3][];
        //    int expectedNumberOfPlayers = 3;
        //    int expectedNumberOfCases = 903;

        //    int winningHandType = 0;
        //    int winningPlayer = 0;
        //    uint winningHandRank = 0;
        //    var losingHandRank = (uint)0x000b9752;

        //    _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()));

        //    _handService.Setup(m => m.GetHandRank(It.IsAny<int>(), It.IsAny<int>(), (int)Cards.EightDiamonds, (int)Cards.EightHearts, (int)Cards.EightSpades, It.IsAny<int>(), It.IsAny<int>()))
        //                .Returns((int playerCard1, int playerCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard, int riverCard) =>
        //                {
        //                    if (playerCard1 == (int)Cards.AceClubs && playerCard2 == (int)Cards.AceDiamonds)
        //                    {
        //                        // New turn and river variance and do rng who wins it, then store it
        //                        winningHandType = rnd.Next(0, 8);
        //                        winningPlayer = rnd.Next(0, 2);
        //                        winningHandRank = (uint)(winningHandType << 20) | (uint)0x000c9752;

        //                        expectedNumberOfWins[winningPlayer][winningHandType]++;

        //                        return winningPlayer == 0 ? winningHandRank : losingHandRank;
        //                    }
        //                    else if (playerCard1 == (int)Cards.KingClubs && playerCard2 == (int)Cards.KingDiamonds)
        //                    {
        //                        return winningPlayer == 1 ? winningHandRank : losingHandRank;
        //                    }
        //                    else if (playerCard1 == (int)Cards.JackClubs && playerCard2 == (int)Cards.JackDiamonds)
        //                    {
        //                        return winningPlayer == 2 ? winningHandRank : losingHandRank;
        //                    }

        //                    throw new System.Exception("Test error");
        //                });

        //    // When
        //    var calculator = CreateOddsCalculator();
        //    var odds = calculator.GetOdds(playerCards, flopCard1, flopCard2, flopCard3);

        //    // Then
        //    Assert.AreEqual(expected.Length, odds.Length);
        //    for (int i = 0; i < expected.Length; i++)
        //    {
        //        Assert.AreEqual(expected[i].Length, odds[i].Length);
        //        for (int j = 0; j < expected[i].Length; j++)
        //        {
        //            Assert.AreEqual(expected[i][j], odds[i][j]);
        //        }
        //    }
        //}
    }
}
