using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Data.OddsCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.UnitTests.Data.OddsCalculator
{
    [TestClass]
    public class PlayerOddsResultTests
    {
        [TestMethod]
        public void TotalNumberOfOccurencesTest()
        {
            // Given
            var expected = 15;

            // When
            var result = new PlayerOddsResult
            {
                NumberOfHandOccurences = new int[] { 0, 2, 2, 2, 2, 2, 2, 2, 1 }
            };

            // Then
            Assert.AreEqual(expected, result.TotalNumberOfOccurences);
        }

        [TestMethod]
        public void TotalNumberOfWinsTest()
        {
            // Given
            var expected = 8;

            // When
            var result = new PlayerOddsResult
            {
                NumberOfWins = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            // Then
            Assert.AreEqual(expected, result.TotalNumberOfWins);
        }

        [TestMethod]
        public void TotalNumberOfTiesTest()
        {
            // Given
            var expected = 12;

            // When
            var result = new PlayerOddsResult
            {
                NumberOfWins = new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            // Then
            Assert.AreEqual(expected, result.TotalNumberOfWins);
        }

        [TestMethod]
        public void GetOccurancePercentagesTest()
        {
            // Given
            var expected = new double[] { 10, 20, 30, 0, 10, 10, 10, 0, 10 };

            // When
            var result = new PlayerOddsResult
            {
                NumberOfHandOccurences = new int[] { 10, 20, 30, 0, 10, 10, 10, 0, 10 }
            };

            // Then
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], result.OccurancePercentages[i]);
            }
        }

        [TestMethod]
        public void GetWinPercentageTest()
        {
            // Given
            var expected = ((double)8 / 15) * 100;

            // When
            var result = new PlayerOddsResult
            {
                NumberOfWins = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1 },
                NumberOfHandOccurences = new int[] { 0, 2, 2, 2, 2, 2, 2, 2, 1 }
            };

            // Then
            Assert.AreEqual(expected, result.WinPercentage);
        }

        [TestMethod]
        public void GetTiePercentageTest()
        {
            // Given
            var expected = ((double)12 / 19) * 100;

            // When
            var result = new PlayerOddsResult
            {
                NumberOfTies = new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1 },
                NumberOfHandOccurences = new int[] { 4, 2, 2, 2, 2, 2, 2, 2, 1 }
            };

            // Then
            Assert.AreEqual(expected, result.TiePercentage);
        }
    }
}
