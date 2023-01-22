using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.UnitTests.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class HandService_CreateHandTests
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

        private HandService CreateHandService()
        {
            return new HandService(_cardValidator.Object);
        }

        [TestMethod]
        public void ValidationsAreCalled()
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40, flopCard3 = 50, turnCard = 52, riverCard = 52;

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(myCard1, "myCard1", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(myCard2, "myCard2", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, "flopCard1", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, "flopCard2", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, "flopCard3", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, "turnCard", true));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(riverCard, "riverCard", true))
                          .Throws<ExpectedTestException>();

            // When
            var handService = CreateHandService();
            Assert.ThrowsException<ExpectedTestException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(7));
        }

        [TestMethod]
        public void ValidationsAreCalled_TurnCantBeEmpty()
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40, flopCard3 = 50, turnCard = 52, riverCard = 1;

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(myCard1, "myCard1", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(myCard2, "myCard2", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, "flopCard1", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, "flopCard2", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, "flopCard3", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, "turnCard", false));
            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(riverCard, "riverCard", true))
                          .Throws<ExpectedTestException>();

            // When
            var handService = CreateHandService();
            Assert.ThrowsException<ExpectedTestException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard));

            // Then
            _cardValidator.Verify(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Exactly(7));
        }

        [TestMethod]
        public void ReturnsHandValueFor5Cards()
        {
            // Given
            int myCard1 = 1, myCard2 = 2, flopCard1 = 3, flopCard2 = 30, flopCard3 = 51;
            ulong myCard1Mask = (ulong)0x1 << 1, myCard2Mask = (ulong)0x1 << 2, flopCard1Mask = (ulong)0x1 << 3, flop2CardMask = (ulong)0x1 << 30, flop3CardMask = (ulong)0x1 << 51;
            ulong expectedHandValue = myCard1Mask | myCard2Mask | flopCard1Mask | flop2CardMask | flop3CardMask; //1st 2nd 3rd 30th 51th bits are 1

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }

        [TestMethod]
        public void ReturnsHandValueFor6Cards()
        {
            // Given
            int myCard1 = 1, myCard2 = 2, flopCard1 = 3, flopCard2 = 30, flopCard3 = 31, turnCard = 40;
            ulong myCard1Mask = (ulong)0x1 << 1, myCard2Mask = (ulong)0x1 << 2, flopCard1Mask = (ulong)0x1 << 3, flop2CardMask = (ulong)0x1 << 30, flop3CardMask = (ulong)0x1 << 31, turnCardMask = (ulong)0x1 << 40;
            ulong expectedHandValue = myCard1Mask | myCard2Mask | flopCard1Mask | flop2CardMask | flop3CardMask | turnCardMask; //1st 2nd 3rd 30th 31th 40th bits are 1

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }

        [TestMethod]
        public void ReturnsHandValueFor7Cards()
        {
            // Given
            int myCard1 = 1, myCard2 = 2, flopCard1 = 3, flopCard2 = 30, flopCard3 = 31, turnCard = 40, riverCard = 50;
            ulong myCard1Mask = (ulong)0x1 << 1, myCard2Mask = (ulong)0x1 << 2, flopCard1Mask = (ulong)0x1 << 3, flop2CardMask = (ulong)0x1 << 30, flop3CardMask = (ulong)0x1 << 31, turnCardMask = (ulong)0x1 << 40, riverCardMask = (ulong)0x1 << 50;
            ulong expectedHandValue = myCard1Mask | myCard2Mask | flopCard1Mask | flop2CardMask | flop3CardMask | turnCardMask | riverCardMask; //1st 2nd 3rd 30th 31th 40th 50th bits are 1

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }

        [TestMethod]
        public void ReturnsHandValueFor5CardEnums()
        {
            // Given
            Cards myCard1 = Cards.TwoSpades;
            Cards myCard2 = Cards.ThreeSpades;
            Cards flopCard1 = Cards.FourSpades;
            Cards flopCard2 = Cards.FiveSpades;
            Cards flopCard3 = Cards.SixSpades;

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            ulong expectedHandValue = (ulong)0x1 << (int)myCard1 |
                                      (ulong)0x1 << (int)myCard2 |
                                      (ulong)0x1 << (int)flopCard1 |
                                      (ulong)0x1 << (int)flopCard2 |
                                      (ulong)0x1 << (int)flopCard3;

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand((int)myCard1, (int)myCard2, (int)flopCard1, (int)flopCard2, (int)flopCard3);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }

        [TestMethod]
        public void ReturnsHandValueFor6CardEnums()
        {
            // Given
            Cards myCard1 = Cards.TwoSpades;
            Cards myCard2 = Cards.ThreeSpades;
            Cards flopCard1 = Cards.FourSpades;
            Cards flopCard2 = Cards.FiveSpades;
            Cards flopCard3 = Cards.SixSpades;
            Cards turnCard = Cards.SevenSpades;

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            ulong expectedHandValue = (ulong)0x1 << (int)myCard1 |
                                      (ulong)0x1 << (int)myCard2 |
                                      (ulong)0x1 << (int)flopCard1 |
                                      (ulong)0x1 << (int)flopCard2 |
                                      (ulong)0x1 << (int)flopCard3 |
                                      (ulong)0x1 << (int)turnCard;

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand((int)myCard1, (int)myCard2, (int)flopCard1, (int)flopCard2, (int)flopCard3, (int)turnCard);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }

        [TestMethod]
        public void ReturnsHandValueFor7CardEnums()
        {
            // Given
            Cards myCard1 = Cards.TwoSpades;
            Cards myCard2 = Cards.ThreeSpades;
            Cards flopCard1 = Cards.FourSpades;
            Cards flopCard2 = Cards.FiveSpades;
            Cards flopCard3 = Cards.SixSpades;
            Cards turnCard = Cards.SevenSpades;
            Cards riverCard = Cards.EightSpades;

            _cardValidator.Setup(m => m.ThrowArgumentExceptionIfCardIsOutOfRange(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()));

            ulong expectedHandValue = (ulong)0x1 << (int)myCard1 |
                                      (ulong)0x1 << (int)myCard2 |
                                      (ulong)0x1 << (int)flopCard1 |
                                      (ulong)0x1 << (int)flopCard2 |
                                      (ulong)0x1 << (int)flopCard3 |
                                      (ulong)0x1 << (int)turnCard |
                                      (ulong)0x1 << (int)riverCard;

            // When
            var handService = CreateHandService();
            var handValue = handService.CreateHand((int)myCard1, (int)myCard2, (int)flopCard1, (int)flopCard2, (int)flopCard3, (int)turnCard, (int)riverCard);

            // Then
            Assert.AreEqual(expectedHandValue, handValue);
        }
    }
}
