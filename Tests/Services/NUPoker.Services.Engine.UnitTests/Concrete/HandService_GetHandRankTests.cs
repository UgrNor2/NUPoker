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
    }
}
