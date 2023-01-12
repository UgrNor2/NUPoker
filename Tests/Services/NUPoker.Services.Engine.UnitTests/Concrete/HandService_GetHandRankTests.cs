using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using NUPoker.Services.Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class HandService_GetHandRankTests
    {
        private HandService CreateHandService()
        {
            return new HandService();
        }

        [TestMethod]
        public void ReturnsHandRankForHighCard()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.ThreeHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.SevenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x000cb975;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForPair()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.AceHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.SevenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x001cb970;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForTwoPairs()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.AceHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.FiveClubs;
            Cards flopCard3 = Cards.SevenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x002c3b00;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForThreeOfAKind()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.AceHearts;
            Cards flopCard1 = Cards.AceSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.SevenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x003cb900;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForStraight_1()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.ThreeHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.TwoClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.FourDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x00430000;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForStraight_2()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.TenHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.QueenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x004c0000;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForFlush_1()
        {
            // Given
            Cards myCard1 = Cards.AceClubs;
            Cards myCard2 = Cards.ThreeHearts;
            Cards flopCard1 = Cards.FiveSpades;
            Cards flopCard2 = Cards.JackClubs;
            Cards flopCard3 = Cards.SevenClubs;
            Cards turnCard = Cards.NineClubs;
            Cards riverCard = Cards.KingClubs;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x005cb975;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }

        [TestMethod]
        public void ReturnsHandRankForFlush_2()
        {
            // Given
            Cards myCard1 = Cards.AceDiamonds;
            Cards myCard2 = Cards.ThreeDiamonds;
            Cards flopCard1 = Cards.FiveDiamonds;
            Cards flopCard2 = Cards.JackDiamonds;
            Cards flopCard3 = Cards.SevenDiamonds;
            Cards turnCard = Cards.NineDiamonds;
            Cards riverCard = Cards.KingDiamonds;

            ulong hand = (ulong)0x1 << (int)myCard1 |
                         (ulong)0x1 << (int)myCard2 |
                         (ulong)0x1 << (int)flopCard1 |
                         (ulong)0x1 << (int)flopCard2 |
                         (ulong)0x1 << (int)flopCard3 |
                         (ulong)0x1 << (int)turnCard |
                         (ulong)0x1 << (int)riverCard;

            uint expectedHandRank = 0x005cb975;

            // When
            var service = CreateHandService();
            var handRank = service.GetHandRank(hand);

            // Then
            Assert.AreEqual(expectedHandRank, handRank);
        }
    }
}
