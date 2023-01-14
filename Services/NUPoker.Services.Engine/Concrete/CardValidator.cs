using NUPoker.Services.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.Concrete
{
    public class CardValidator : ICardValidator
    {
        public void ThrowArgumentExceptionIfCardIsOutOfRange(int card, bool canBeEmpty = false)
        {
            if (card < 0 || (canBeEmpty && card > 52) || (!canBeEmpty && card > 51))
            {
                throw new ArgumentException($"{card} is not a card value.");
            }
        }
    }
}
