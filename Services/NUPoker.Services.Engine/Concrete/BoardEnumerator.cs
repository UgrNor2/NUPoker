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
            ulong notUseableCardsMask = CreateNotUseableCardsMask(deadCards, flopCard1, flopCard2, flopCard3, turnCard);


            if (flopCard3 == 52)
            {
                for (flopCard1 = 0; flopCard1 < 48; flopCard1++)
                {
                    if ((notUseableCardsMask & Tables.CardMasksTable[flopCard1]) != 0)
                    {
                        continue;
                    }

                    for (flopCard2 = flopCard1 + 1; flopCard2 < 49; flopCard2++)
                    {
                        if ((notUseableCardsMask & Tables.CardMasksTable[flopCard2]) != 0)
                        {
                            continue;
                        }

                        for (flopCard3 = flopCard2 + 1; flopCard3 < 50; flopCard3++)
                        {
                            if ((notUseableCardsMask & Tables.CardMasksTable[flopCard3]) != 0)
                            {
                                continue;
                            }

                            for (turnCard = flopCard3 + 1; turnCard < 51; turnCard++)
                            {
                                if ((notUseableCardsMask & Tables.CardMasksTable[turnCard]) != 0)
                                {
                                    continue;
                                }

                                for (var riverCard = turnCard + 1; riverCard < 52; riverCard++)
                                {
                                    if ((notUseableCardsMask & Tables.CardMasksTable[riverCard]) != 0)
                                    {
                                        continue;
                                    }

                                    yield return (flopCard1, flopCard2, flopCard3, turnCard, riverCard);
                                }
                            }
                        }
                    }
                }
            }

            if (flopCard3 != 52 && turnCard == 52)
            {
                for (turnCard = 0; turnCard < 52; turnCard++)
                {
                    if ((notUseableCardsMask & Tables.CardMasksTable[turnCard]) != 0)
                    {
                        continue;
                    }

                    for (int riverCard = turnCard + 1; riverCard < 52; riverCard++)
                    {
                        if ((notUseableCardsMask & Tables.CardMasksTable[riverCard]) != 0)
                        {
                            continue;
                        }

                        yield return (flopCard1, flopCard2, flopCard3, turnCard, riverCard);
                    }
                }
            }

            if (turnCard != 52)
            {
                for (int riverCard = turnCard + 1; riverCard < 52; riverCard++)
                {
                    if ((notUseableCardsMask & Tables.CardMasksTable[riverCard]) != 0)
                    {
                        continue;
                    }

                    yield return (flopCard1, flopCard2, flopCard3, turnCard, riverCard);
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

        private ulong CreateNotUseableCardsMask(List<int> deadCards, int flopCard1, int flopCard2, int flopCard3, int turnCard)
        {
            ulong notUseableCardsMask = 0;

            for (int i = 0; i < deadCards.Count; i++)
            {
                notUseableCardsMask |= Tables.CardMasksTable[deadCards[i]];

                if (flopCard3 != 52)
                {
                    notUseableCardsMask |= Tables.CardMasksTable[flopCard1];
                    notUseableCardsMask |= Tables.CardMasksTable[flopCard2];
                    notUseableCardsMask |= Tables.CardMasksTable[flopCard3];

                    if (turnCard != 52)
                    {
                        notUseableCardsMask |= Tables.CardMasksTable[turnCard];
                    }
                }
            }

            return notUseableCardsMask;
        }
    }
}
