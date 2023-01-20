using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class BoardEnumertorTests
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Strict);
        private Mock<ICardValidator> _cardValidator = new Mock<ICardValidator>();

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _cardValidator = _mockRepository.Create<ICardValidator>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockRepository.Verify();
        }

        private BoardEnumerator CreateBoardEnumerator()
        {
            return new BoardEnumerator(_cardValidator.Object);
        }

        [TestMethod]
        public void ValidationRunsForAllCards_1()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(0, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(1, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(2, false)).Callback(() => { });
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            // When
            var boardEnumerator = CreateBoardEnumerator();
            var result = boardEnumerator.GetAllPossibleBoards(new List<int> { 0, 1, 2 }).ToList();

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(7));
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
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(52, true)).Callback(() => { });

            // When
            var boardEnumerator = CreateBoardEnumerator();
            boardEnumerator.GetAllPossibleBoards(new List<int> { 0, 1, 2 }, 3, 4, 5).ToList();

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(7));
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
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(6, true)).Callback(() => { });

            // When
            var boardEnumerator = CreateBoardEnumerator();
            boardEnumerator.GetAllPossibleBoards(new List<int> { 0, 1, 2 }, 3, 4, 5, 6).ToList();

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>()), Times.Exactly(7));
        }

        [TestMethod]
        public void IfThereIsNoEnoughAvailableCards_ReturnEmptyList()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>())).Callback(() => { });
            var deadCards = Enumerable.Range(0, 48).ToList(); //4 cards remained

            // When
            var boardEnumerator = CreateBoardEnumerator();
            var allPossibleBoards = boardEnumerator.GetAllPossibleBoards(deadCards).ToList();

            // Then
            Assert.AreEqual(0, allPossibleBoards.Count);
        }

        [TestMethod]
        public void ReturnsAllPossibleBoards_1()
        {
            // Given
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<bool>())).Callback(() => { });
            var deadCards = Enumerable.Range(0, 46).ToList();
            var expectedBoards = new List<(int, int, int, int, int)>
            {
                (46,47,48,49,50),
                (46,47,48,49,51),
                (46,47,48,50,51),
                (46,47,49,50,51),
                (46,48,49,50,51),
                (47,48,49,50,51)
            };

            // When
            var boardEnumerator = CreateBoardEnumerator();
            var allPossibleBoards = boardEnumerator.GetAllPossibleBoards(deadCards).ToList();

            // Then
            Assert.AreEqual(expectedBoards.Count, allPossibleBoards.Count);
            Assert.IsTrue(AreTheListsEqual(expectedBoards, allPossibleBoards));
        }

        private bool AreTheListsEqual(List<(int, int, int, int, int)> list1, List<(int, int, int, int, int)> list2)
        {
            foreach (var item in list1)
            {
                if (!list2.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
