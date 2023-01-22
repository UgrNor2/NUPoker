using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUPoker.Services.Engine.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.UnitTests.Concrete
{
    [TestClass]
    public class CardValidatorTests
    {
        private CardValidator CreateCardValidator()
        {
            return new CardValidator();
        }

        [TestMethod]
        [DataRow(-10, false, true)]
        [DataRow(-10, true, true)]
        [DataRow(0, true, false)]
        [DataRow(0, false, false)]
        [DataRow(10, false, false)]
        [DataRow(10, true, false)]
        [DataRow(51, false, false)]
        [DataRow(51, true, false)]
        [DataRow(52, false, true)]
        [DataRow(52, true, false)]
        public void ThrowArgumentExceptionIfCardIsOutOfRange_WithoutParamName(int card, bool canBeEmpty, bool throwsException)
        {
            // Given

            // When
            var cardValidator = CreateCardValidator();
            try
            {
                cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(card, canBeEmpty);
            }
            catch(ArgumentException exc) when (throwsException)
            {
                Assert.AreEqual($"{card} is not a card value.", exc.Message);
            }

            // Then
        }

        [TestMethod]
        [DataRow(-10, false, true)]
        [DataRow(-10, true, true)]
        [DataRow(0, true, false)]
        [DataRow(0, false, false)]
        [DataRow(10, false, false)]
        [DataRow(10, true, false)]
        [DataRow(51, false, false)]
        [DataRow(51, true, false)]
        [DataRow(52, false, true)]
        [DataRow(52, true, false)]
        public void ThrowArgumentExceptionIfCardIsOutOfRange_WithParamName(int card, bool canBeEmpty, bool throwsException)
        {
            // Given
            string paramName = "test_paramName";

            // When
            var cardValidator = CreateCardValidator();
            try
            {
                cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(card, paramName, canBeEmpty);
            }
            catch (ArgumentException exc) when (throwsException)
            {
                Assert.AreEqual($"{paramName} is out of range. (Parameter '{paramName}')", exc.Message);
                Assert.AreEqual(paramName, exc.ParamName);
            }

            // Then
        }
    }
}
