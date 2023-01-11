using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.StaticTables;

namespace NUPoker.Services.Engine.Concrete
{
    public class HandService : IHandService
    {
        private const int TYPE_SHIFT = 20;

        public ulong CreateHand(int myCard1, int myCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52, int riverCard = 52)
        {
            ValidateInput(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard);

            return Tables.CardMasksTable[myCard1] | 
                   Tables.CardMasksTable[myCard2] | 
                   Tables.CardMasksTable[flopCard1] | 
                   Tables.CardMasksTable[flopCard2] | 
                   Tables.CardMasksTable[flopCard3] | 
                   Tables.CardMasksTable[turnCard] | 
                   Tables.CardMasksTable[riverCard];
        }

        private void ValidateInput(int myCard1, int myCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard, int riverCard)
        {
            ThrowArgumentExceptionIfCardIsOutOfRange(myCard1, nameof(myCard1));
            ThrowArgumentExceptionIfCardIsOutOfRange(myCard2, nameof(myCard2));
            ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, nameof(flopCard1));
            ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, nameof(flopCard2));
            ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, nameof(flopCard3));
            ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, nameof(turnCard), true);
            ThrowArgumentExceptionIfCardIsOutOfRange(riverCard, nameof(riverCard), true);

            if (turnCard == 52 && riverCard != 52)
            {
                throw new ArgumentException($"{nameof(turnCard)} cannot be empty when {nameof(riverCard)} is not empty.");
            }
        }

        private void ThrowArgumentExceptionIfCardIsOutOfRange(int card, string paramName, bool canBeEmpty = false)
        {
            if (card < 0 || (canBeEmpty && card > 52) || (!canBeEmpty && card > 51))
            {
                throw new ArgumentException($"{paramName} is out of range.", paramName);
            }
        }

        public ulong CreateHand(Cards myCard1, Cards myCard2, Cards flopCard1, Cards flopCard2, Cards flopCard3, Cards turnCard = Cards.Empty, Cards riverCard = Cards.Empty)
        {
            return CreateHand((int)myCard1, (int)myCard2, (int)flopCard1, (int)flopCard2, (int)flopCard3, (int)turnCard, (int)riverCard);
        }

        public uint GetHandRank(ulong hand)
        {
            int numberOfCards = 7; //TODO: Create a lookup table for this

            uint spadeHand = (uint)(hand & 0x1FFFUL);
            uint heartHand = (uint)(hand >> 13 & 0x1FFFUL);
            uint clubHand = (uint)(hand >> 26 & 0x1FFFUL);
            uint diamondHand = (uint)(hand >> 39 & 0x1FFFUL);

            uint ranks = spadeHand | heartHand | clubHand | diamondHand;

            uint n_ranks = Tables.NumberOfBitsTable[ranks];
            uint n_dups = ((uint)(numberOfCards - n_ranks));

            switch (n_dups)
            {
                case 0:
                    return (((uint)HandTypes.HighCard) << TYPE_SHIFT) + Tables.TopFiveCardsTable[ranks];
            }

            throw new NotImplementedException();
        }
    }
}
