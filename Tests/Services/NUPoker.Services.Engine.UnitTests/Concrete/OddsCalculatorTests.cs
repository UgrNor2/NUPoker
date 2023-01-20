using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.UnitTests.Exception;
using System;
using System.Collections.Generic;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class OddsCalculatorTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Strict);
        private Mock<IHandService> _handService = new Mock<IHandService>();
        private Mock<ICardValidator> _cardValidator = new Mock<ICardValidator>();
        private Mock<IBoardEnumerator> _boardEnumerator = new Mock<IBoardEnumerator>();

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _handService = _mockRepository.Create<IHandService>();
            _cardValidator = _mockRepository.Create<ICardValidator>();
            _boardEnumerator = _mockRepository.Create<IBoardEnumerator>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockRepository.Verify();
        }

        private OddsCalculator CreateOddsCalculator()
        {
            return new OddsCalculator(_handService.Object, _cardValidator.Object, _boardEnumerator.Object);
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

            _boardEnumerator.Setup(m => m.GetAllPossibleBoards(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(new List<(int, int, int, int, int)> { (1,2,3,4,5)});                            

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

            _boardEnumerator.Setup(m => m.GetAllPossibleBoards(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<(int, int, int, int, int)> { (1, 2, 3, 4, 5) });

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

            _boardEnumerator.Setup(m => m.GetAllPossibleBoards(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<(int, int, int, int, int)> { (1, 2, 3, 4, 5) });

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

            _boardEnumerator.Setup(m => m.GetAllPossibleBoards(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<(int, int, int, int, int)> { (1, 2, 3, 4, 5) });

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

            _boardEnumerator.Setup(m => m.GetAllPossibleBoards(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<(int, int, int, int, int)> { (1, 2, 3, 4, 5) });

            // When
            var calculator = CreateOddsCalculator();
            Assert.ThrowsException<ExpectedTestException>(() => calculator.GetOdds(new List<(int, int)> { (0, 1), (2, 3), (4, 5) }));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(10));
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true), Times.Exactly(4));
        }
    }
}
