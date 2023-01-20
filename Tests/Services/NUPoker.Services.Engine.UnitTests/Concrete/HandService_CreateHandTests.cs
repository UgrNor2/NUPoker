using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Interfaces;
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
        private HandService CreateHandService()
        {
            return new HandService();
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(52)]
        [DataRow(100)]
        public void ThrowsException_WhenMyCard1IsNotInRange(int myCard1)
        {
            // Given
            int myCard2 = 10, flopCard1 = 20, flopCard2 = 30, flopCard3 = 40;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3));

            // Then
            Assert.AreEqual("myCard1 is out of range. (Parameter 'myCard1')", exception.Message);
            Assert.AreEqual("myCard1", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(52)]
        [DataRow(100)]
        public void ThrowsException_WhenMyCard2IsNotInRange(int myCard2)
        {
            // Given
            int myCard1 = 10, flopCard1 = 20, flopCard2 = 30, flopCard3 = 40;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3));

            // Then
            Assert.AreEqual("myCard2 is out of range. (Parameter 'myCard2')", exception.Message);
            Assert.AreEqual("myCard2", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(52)]
        [DataRow(100)]
        public void ThrowsException_WhenFlopCard1IsNotInRange(int flopCard1)
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard2 = 30, flopCard3 = 40;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3));

            // Then
            Assert.AreEqual("flopCard1 is out of range. (Parameter 'flopCard1')", exception.Message);
            Assert.AreEqual("flopCard1", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(52)]
        [DataRow(100)]
        public void ThrowsException_WhenFlopCard2IsNotInRange(int flopCard2)
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard3 = 40;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3));

            // Then
            Assert.AreEqual("flopCard2 is out of range. (Parameter 'flopCard2')", exception.Message);
            Assert.AreEqual("flopCard2", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(52)]
        [DataRow(100)]
        public void ThrowsException_WhenFlopCard3IsNotInRange(int flopCard3)
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3));

            // Then
            Assert.AreEqual("flopCard3 is out of range. (Parameter 'flopCard3')", exception.Message);
            Assert.AreEqual("flopCard3", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(53)]
        [DataRow(100)]
        public void ThrowsException_WhenTurnCardIsNotInRange(int turnCard)
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40, flopCard3 = 50;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard));

            // Then
            Assert.AreEqual("turnCard is out of range. (Parameter 'turnCard')", exception.Message);
            Assert.AreEqual("turnCard", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(53)]
        [DataRow(100)]
        public void ThrowsException_WhenRiverCardIsNotInRange(int riverCard)
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40, flopCard3 = 50, turnCard = 51;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard));

            // Then
            Assert.AreEqual("riverCard is out of range. (Parameter 'riverCard')", exception.Message);
            Assert.AreEqual("riverCard", exception.ParamName);
        }

        [TestMethod]
        public void ThrowsException_WhenTurnIsEmptyAndRiverIsNot()
        {
            // Given
            int myCard1 = 10, myCard2 = 20, flopCard1 = 30, flopCard2 = 40, flopCard3 = 50, turnCard = 52, riverCard = 1;

            // When
            var handService = CreateHandService();
            var exception = Assert.ThrowsException<ArgumentException>(() => handService.CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard));

            // Then
            Assert.AreEqual("turnCard cannot be empty when riverCard is not empty.", exception.Message);
        }

        [TestMethod]
        public void ReturnsHandValueFor5Cards()
        {
            // Given
            int myCard1 = 1, myCard2 = 2, flopCard1 = 3, flopCard2 = 30, flopCard3 = 51;
            ulong myCard1Mask = (ulong)0x1 << 1, myCard2Mask = (ulong)0x1 << 2, flopCard1Mask = (ulong)0x1 << 3, flop2CardMask = (ulong)0x1 << 30, flop3CardMask = (ulong)0x1 << 51;
            ulong expectedHandValue = myCard1Mask | myCard2Mask | flopCard1Mask | flop2CardMask | flop3CardMask; //1st 2nd 3rd 30th 51th bits are 1

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
