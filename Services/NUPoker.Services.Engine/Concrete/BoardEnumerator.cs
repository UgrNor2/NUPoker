using NUPoker.Services.Engine.Data.OddsCalculator;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.StaticTables;

namespace NUPoker.Services.Engine.Concrete
{
    public class BoardEnumerator : IBoardEnumerator
    {
        private readonly ICardValidator _cardValidator;

        public BoardEnumerator(ICardValidator cardValidator)
        {
            _cardValidator = cardValidator;
        }

        public IEnumerable<(int, int, int, int, int)> GetAllPossibleBoards(List<int> deadCards, int flopCard1 = 52, int flopCard2 = 52, int flopCard3 = 52, int turnCard = 52)
        {
            ValidateInput(deadCards, flopCard1, flopCard2, flopCard3, turnCard);

            List<int> availableCards = GetAvailableCards(deadCards, flopCard1, flopCard2, flopCard3, turnCard);

            if (flopCard3 == 52)
            {
                for (flopCard1 = 0; flopCard1 < availableCards.Count - 4; flopCard1++)
                {
                    for (flopCard2 = flopCard1 + 1; flopCard2 < availableCards.Count - 3; flopCard2++)
                    {
                        for (flopCard3 = flopCard2 + 1; flopCard3 < availableCards.Count - 2; flopCard3++)
                        {
                            for (turnCard = flopCard3 + 1; turnCard < availableCards.Count - 1; turnCard++)
                            {
                                for (var riverCard = turnCard + 1; riverCard < availableCards.Count; riverCard++)
                                {
                                    yield return (availableCards[flopCard1], availableCards[flopCard2], availableCards[flopCard3], availableCards[turnCard], availableCards[riverCard]);
                                }
                            }
                        }
                    }
                }
            }

            else if (flopCard3 != 52 && turnCard == 52)
            {
                for (turnCard = 0; turnCard < availableCards.Count - 1; turnCard++)
                {
                    for (int riverCard = turnCard + 1; riverCard < availableCards.Count; riverCard++)
                    {
                        yield return (flopCard1, flopCard2, flopCard3, availableCards[turnCard], availableCards[riverCard]);
                    }
                }
            }

            else if (turnCard != 52)
            {
                for (int riverCard = 0; riverCard < availableCards.Count; riverCard++)
                {
                    yield return (flopCard1, flopCard2, flopCard3, turnCard, availableCards[riverCard]);
                }
            }

        }

        private void ValidateInput(List<int> deadCards, int flopCard1, int flopCard2, int flopCard3, int turnCard)
        {
            if (deadCards != null)
            {
                foreach (var deadCard in deadCards)
                {
                    _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(deadCard, false);
                }
            }

            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, true);
        }

        private List<int> GetAvailableCards(List<int> deadCards, int flopCard1, int flopCard2, int flopCard3, int turnCard)
        {
            List<int> availableCards = new List<int>();

            for (int i = 0; i < 52; i++)
            {
                if (!deadCards.Contains(i) && i != flopCard1 && i != flopCard2 && i != flopCard3 && i != turnCard)
                {
                    availableCards.Add(i);
                }
            }

            return availableCards;
        }
    }
}
