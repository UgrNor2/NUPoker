﻿using NUPoker.Services.Engine.Data.OddsCalculator;
using NUPoker.Services.Engine.Helper;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.StaticTables;

namespace NUPoker.Services.Engine.Concrete
{
    public class OddsCalculator : IOddsCalculator
    {
        private readonly IHandService _handService;
        private readonly ICardValidator _cardValidator;
        private readonly IBoardEnumerator _boardEnumerator;

        public OddsCalculator(IHandService handService, ICardValidator cardValidator, IBoardEnumerator boardEnumerator)
        {
            _handService = handService;
            _cardValidator = cardValidator;
            _boardEnumerator = boardEnumerator;
        }

        public OddsCalculatorResult GetOdds(List<(int, int)> playerCards, int flopCard1 = 52, int flopCard2 = 52, int flopCard3 = 52, int turnCard = 52)
        {
            ValidategetOddsInput(playerCards, flopCard1, flopCard2, flopCard3, turnCard);

            var numberOfPlayers = playerCards.Count;
            var oddsCalculatorResult = new OddsCalculatorResult(numberOfPlayers, GetNumberOfCases(numberOfPlayers, flopCard1 == 52 ? 0 : (turnCard == 52 ? 3 : 4)));

            uint[] playerHandRanks = new uint[numberOfPlayers];
            ulong notUseableCardsMask = CreateNotUseableCardsMask(playerCards, flopCard1, flopCard2, flopCard3, turnCard);
            var winningPlayerIndexes = new List<int>();

            var deadCards = playerCards.Select(i => i.Item1).Union(playerCards.Select(i => i.Item2)).ToList();
            var possibleBoards = _boardEnumerator.GetAllPossibleBoards(deadCards, flopCard1, flopCard2, flopCard3, turnCard);

            foreach (var item in possibleBoards)
            {
                DetermineWinnerAndRegisterItInResult(playerCards, item.Item1, item.Item2, item.Item3, item.Item4, numberOfPlayers, oddsCalculatorResult, playerHandRanks, winningPlayerIndexes, item.Item5);
            }

            return oddsCalculatorResult;
        }

        private void DetermineWinnerAndRegisterItInResult(List<(int, int)> playerCards, int flopCard1, int flopCard2, int flopCard3, int turnCard, int numberOfPlayers, OddsCalculatorResult oddsCalculatorResult, uint[] playerHandRanks, List<int> winningPlayerIndexes, int riverCard)
        {
            uint handRankType = 0;

            for (int playerNumber = 0; playerNumber < numberOfPlayers; playerNumber++)
            {
                playerHandRanks[playerNumber] = _handService.GetHandRank(playerCards[playerNumber].Item1, playerCards[playerNumber].Item2, flopCard1, flopCard2, flopCard3, turnCard, riverCard);
                handRankType = GetHandRankType(playerHandRanks[playerNumber]);
                oddsCalculatorResult.PlayerOddsResults[playerNumber].NumberOfHandOccurences[handRankType]++;
            }

            SetWinningPlayers(playerHandRanks, winningPlayerIndexes);

            handRankType = GetHandRankType(playerHandRanks[winningPlayerIndexes[0]]);

            // One winner only => win
            if (winningPlayerIndexes.Count == 1)
            {
                oddsCalculatorResult.PlayerOddsResults[winningPlayerIndexes[0]].NumberOfWins[handRankType]++;
            }
            // More winners => tie
            else
            {
                for (int i = 0; i < winningPlayerIndexes.Count; i++)
                {
                    oddsCalculatorResult.PlayerOddsResults[winningPlayerIndexes[i]].NumberOfTies[handRankType]++;
                }
            }
        }

        private void ValidategetOddsInput(List<(int, int)> playerCards, int flopCard1, int flopCard2, int flopCard3, int turnCard)
        {
            if (playerCards.Count == 0)
            {
                throw new ArgumentException($"{nameof(playerCards)} list cannot be empty.", nameof(playerCards));
            }

            if (playerCards.Count == 1)
            {
                throw new ArgumentException($"{nameof(playerCards)} should have min 2 elements.", nameof(playerCards));
            }

            if (playerCards.Count > 10)
            {
                throw new ArgumentException($"{nameof(playerCards)} should have max 10 elements.", nameof(playerCards));
            }

            playerCards.ForEach(p =>
            {
                _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(p.Item1);
                _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(p.Item2);
            });

            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, flopCard1 == 52 && flopCard2 == 52 && flopCard3 == 52 && turnCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, true);
        }

        private ulong CreateNotUseableCardsMask(List<(int, int)> playerCards, int flopCard1, int flopCard2, int flopCard3, int turnCard)
        {
            ulong notUseableCardsMask = 0;

            for (int i = 0; i < playerCards.Count; i++)
            {
                notUseableCardsMask |= Tables.CardMasksTable[playerCards[i].Item1];
                notUseableCardsMask |= Tables.CardMasksTable[playerCards[i].Item2];

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

        private int GetNumberOfCases(int playerCount, int boardCards)
        {
            int availableCards = 52 - (2 * playerCount) - boardCards;
            int numberOfCardsToSelect = 5 - boardCards;

            return (int)CombinationHelper.Combination(availableCards, numberOfCardsToSelect);
        }

        private uint GetHandRankType(uint handRank)
        {
            return handRank >> 20;
        }

        private void SetWinningPlayers(uint[] playerHandRanks, List<int> winningPlayerIndexes)
        {
            var winningHandRank = playerHandRanks[0];
            winningPlayerIndexes.Clear();
            winningPlayerIndexes.Add(0);

            for (int i = 1; i < playerHandRanks.Length; i++)
            {
                if (playerHandRanks[i] > winningHandRank)
                {
                    winningPlayerIndexes.Clear();
                    winningPlayerIndexes.Add(i);
                    winningHandRank = playerHandRanks[i];
                }
                else if (playerHandRanks[i] == winningHandRank)
                {
                    winningPlayerIndexes.Add(i);
                }
            }
        }
    }
}
