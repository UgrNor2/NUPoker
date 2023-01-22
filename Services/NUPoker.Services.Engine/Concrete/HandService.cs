﻿using NUPoker.Services.Engine.Data;
using NUPoker.Services.Engine.Interfaces;
using NUPoker.Services.Engine.StaticTables;
using System;

namespace NUPoker.Services.Engine.Concrete
{
    public class HandService : IHandService
    {
        private const int TYPE_SHIFT = 20;
        private const int FIRST_CARD_SHIFT = 16;
        private const int SECOND_CARD_SHIFT = 12;
        private const int THIRD_CARD_SHIFT = 8;
        
        private readonly ICardValidator _cardValidator;

        public HandService(ICardValidator cardValidator)
        {
            _cardValidator = cardValidator;
        }

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
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(myCard1, nameof(myCard1));
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(myCard2, nameof(myCard2));
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard1, nameof(flopCard1));
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard2, nameof(flopCard2));
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(flopCard3, nameof(flopCard3));
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(turnCard, nameof(turnCard), riverCard == 52);
            _cardValidator.ThrowArgumentExceptionIfCardIsOutOfRange(riverCard, nameof(riverCard), true);
        }

        public uint GetHandRank(ulong hand)
        {
            uint spadeHand = (uint)(hand & 0x1FFFUL); //Only spade cards in 13 bit
            uint heartHand = (uint)(hand >> 13 & 0x1FFFUL); //Only heart cards in 13 bit
            uint clubHand = (uint)(hand >> 26 & 0x1FFFUL); //Only club cards in 13 bit
            uint diamondHand = (uint)(hand >> 39 & 0x1FFFUL); //Only diamond cards in 13 bit

            int numberOfCards = Tables.NumberOfBitsTable[spadeHand] + Tables.NumberOfBitsTable[heartHand] + Tables.NumberOfBitsTable[clubHand] + Tables.NumberOfBitsTable[diamondHand];

            uint ranks = spadeHand | heartHand | clubHand | diamondHand;

            uint numberOfDifferentRanks = Tables.NumberOfBitsTable[ranks];
            uint numberOfDuplications = ((uint)(numberOfCards - numberOfDifferentRanks));

            return GetHandRank(numberOfDifferentRanks, numberOfDuplications, spadeHand, heartHand, clubHand, diamondHand, ranks);
        }

        public uint GetHandRank(int myCard1, int myCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52, int riverCard = 52)
        {
            return GetHandRank(CreateHand(myCard1, myCard2, flopCard1, flopCard2, flopCard3, turnCard, riverCard));
        }

        private static uint GetHandRank(uint numberOfDifferentRanks, uint numberOfDuplications, uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            if(numberOfDifferentRanks >= 5)
            {
                if (Tables.NumberOfBitsTable[spadeHand] >= 5)
                {
                    if (Tables.StraightTable[spadeHand] != 0)
                        return GetHandRank_StraightFlush(spadeHand);
                    else
                        return GetHandRank_Flush(spadeHand);
                }
                else if (Tables.NumberOfBitsTable[heartHand] >= 5)
                {
                    if (Tables.StraightTable[heartHand] != 0)
                        return GetHandRank_StraightFlush(heartHand);
                    else
                        return GetHandRank_Flush(heartHand);
                }
                else if (Tables.NumberOfBitsTable[clubHand] >= 5)
                {
                    if (Tables.StraightTable[clubHand] != 0)
                        return GetHandRank_StraightFlush(clubHand);
                    else
                        return  GetHandRank_Flush(clubHand);
                }
                else if (Tables.NumberOfBitsTable[diamondHand] >= 5)
                {
                    if (Tables.StraightTable[diamondHand] != 0)
                        return GetHandRank_StraightFlush(diamondHand);
                    else
                        return GetHandRank_Flush(diamondHand);
                }
                else if (Tables.StraightTable[ranks] != 0)
                {
                    return GetHandRank_Straight(ranks);
                }
            }

            switch (numberOfDuplications)
            {
                case 0: return GetHandRank_HighCard(ranks);
                case 1: return GetHandRank_Pair(spadeHand, heartHand, clubHand, diamondHand, ranks);
                case 2:
                    {
                        var pairCardsMask = ranks ^ (spadeHand ^ heartHand ^ clubHand ^ diamondHand);
                        if(pairCardsMask != 0)
                        {
                            return GetHandRank_TwoPairs(spadeHand, heartHand, clubHand, diamondHand, ranks, pairCardsMask);
                        }
                        else
                        {
                            return GetHandRank_ThreeOfAKind(spadeHand, heartHand, clubHand, diamondHand, ranks);
                        }                      
                    }
                default:
                    {
                        var fourCardsMask = spadeHand & heartHand & clubHand & diamondHand;
                        if (fourCardsMask != 0)
                        {
                            return GetHandRank_FourOfAKind(fourCardsMask, ranks);
                        }

                        var pairCardsMask = ranks ^ (spadeHand ^ heartHand ^ clubHand ^ diamondHand);
                        if (Tables.NumberOfBitsTable[pairCardsMask] != numberOfDuplications)
                        {
                            return GetHandRank_FullHouse(spadeHand, heartHand, clubHand, diamondHand, pairCardsMask);
                        }

                        return GetHandRank_TwoPairs(ranks, pairCardsMask);

                        throw new InvalidOperationException("Unexpected error for getting the hand rank.");
                    }
            }
        }

        private static uint GetHandRank_HighCard(uint ranks)
        {
            return (((uint)HandTypes.HighCard) << TYPE_SHIFT) | Tables.TopFiveCardsTable[ranks];
        }

        private static uint GetHandRank_Pair(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            // Get the pair card, it will contain 1 bit only
            var pairCardMask = ranks ^ (spadeHand ^ heartHand ^ clubHand ^ diamondHand);

            // Set Pair hand type, then set the first card to the pair card
            var retValType = (uint)HandTypes.Pair << TYPE_SHIFT;
            var retValPairCard = (uint)(Tables.TopCardTable[pairCardMask] << FIRST_CARD_SHIFT);

            // Get the other (not pair) cards' bits
            var otherCards = ranks ^ pairCardMask;

            // Get the five top cards, but since we already set the first card bits (the pair card) we shift it right with 4 bits.
            // Then we do a BITWISE AND to make the fifth card bits 0000.
            var retValKickers = (uint)((Tables.TopFiveCardsTable[otherCards] >> 4) & 0xFFFFFFF0);
            
            return retValType | retValPairCard | retValKickers;
        }

        private static uint GetHandRank_TwoPairs(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks, uint pairCardsMask)
        {
            // Get the other (not pair) cards' bits
            var otherCards = ranks ^ pairCardsMask;

            // Set the TwoPairs hand type, then set the two pairs ranks and finally we set the top card from the other (not pair) cards and set it into the third card place.
            var retValType = (uint)HandTypes.TwoPairs << TYPE_SHIFT;
            var retValPairCards = Tables.TopFiveCardsTable[pairCardsMask] & 0x000FF000;
            var retValKicker = (uint)(Tables.TopCardTable[otherCards] << THIRD_CARD_SHIFT);

            return retValType | retValPairCards | retValKicker;
        }

        private static uint GetHandRank_TwoPairs(uint ranks, uint pairCardsMask)
        {
            uint top, second;

            var retval = (((uint)HandTypes.TwoPairs) << TYPE_SHIFT);
            top = Tables.TopCardTable[pairCardsMask];
            retval += (top << FIRST_CARD_SHIFT);
            second = Tables.TopCardTable[pairCardsMask ^ (1 << (int)top)];
            retval += (second << SECOND_CARD_SHIFT);
            retval += (uint)((Tables.TopCardTable[ranks ^ (1U << (int)top) ^ (1 << (int)second)]) << THIRD_CARD_SHIFT);
            return retval;
        }

        private static uint GetHandRank_ThreeOfAKind(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            // Get the three of a kind card, it will contain 1 bit only
            var threeCardsMask = ((clubHand & diamondHand) | (heartHand & spadeHand)) & ((clubHand & heartHand) | (diamondHand & spadeHand));

            // Set ThreeOfAKind hand type, then set the ThreeOfAKind card to the first card place
            var retValType = (uint)HandTypes.ThreeOfAKind << TYPE_SHIFT;
            var retValThreeOfAKindCard = (uint)Tables.TopCardTable[threeCardsMask] << FIRST_CARD_SHIFT;

            // Get the other (not three of a kind) cards' bits
            var otherCards = ranks ^ threeCardsMask;

            // Top card from others
            var topCard = (uint)Tables.TopCardTable[otherCards];

            // Moved to the second card spot
            var retValFirstKickerCard = topCard << SECOND_CARD_SHIFT;

            // Removing topCard's 1 bit from otherCards
            otherCards ^= (1U << (int)topCard);

            // Getting the top card again from others
            topCard = (uint)Tables.TopCardTable[otherCards];

            // Moved to the third card spot
            var retValSecondKickerCard = topCard << THIRD_CARD_SHIFT;

            return retValType | retValThreeOfAKindCard | retValFirstKickerCard | retValSecondKickerCard;
        }

        private static uint GetHandRank_Straight(uint ranks)
        {
            // The top card
            var topCard = (uint)Tables.StraightTable[ranks];

            // Set Straight hand type, then set the top card of the straight to the first card place
            var retValType = (uint)HandTypes.Straight << TYPE_SHIFT;
            var retValTopCard = topCard << FIRST_CARD_SHIFT;

            return retValType | retValTopCard;
        }

        private static uint GetHandRank_Flush(uint flushSuitHand)
        {
            // Set Flush hand type, then get the top 5 cards
            var retValType = (uint)HandTypes.Flush << TYPE_SHIFT;
            var retValTopCards = Tables.TopFiveCardsTable[flushSuitHand];

            return retValType | retValTopCards;
        }

        private static uint GetHandRank_FullHouse(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint pairCardsMask)
        {
            // Get the three of a kind card, it will contain 1 bit only
            var threeCardsMask = ((clubHand & diamondHand) | (heartHand & spadeHand)) & ((clubHand & heartHand) | (diamondHand & spadeHand));

            var retvalType = ((uint)HandTypes.FullHouse) << TYPE_SHIFT;
            var firstCardMask = (uint)Tables.TopCardTable[threeCardsMask];
            var retValFirstCard = firstCardMask << FIRST_CARD_SHIFT;
            var retValTypeAndFirst = retvalType | retValFirstCard;
            var secondCardMask = ((pairCardsMask | threeCardsMask) ^ (1U << (int)firstCardMask));
            var retValSecondCard = (uint)(Tables.TopCardTable[secondCardMask] << SECOND_CARD_SHIFT);

            return retValTypeAndFirst | retValSecondCard;
        }

        private static uint GetHandRank_FourOfAKind(uint fourCardsMask, uint ranks)
        {
            uint cardMask = Tables.TopCardTable[fourCardsMask];
            var retValHandType = (uint)HandTypes.FourOfAKind << TYPE_SHIFT;
            var retValFirstCard = cardMask << FIRST_CARD_SHIFT;
            var retValSecondCard = (uint)(Tables.TopCardTable[ranks ^ (1UL << (int)cardMask)]) << SECOND_CARD_SHIFT;
            return retValHandType | retValFirstCard | retValSecondCard;
        }

        private static uint GetHandRank_StraightFlush(uint flushSuitHand)
        {
            // Set Straight Flush hand type, then get the top 5 cards
            var retValType = (uint)HandTypes.StraightFlush << TYPE_SHIFT;
            var retValTopCards = (uint)Tables.StraightTable[flushSuitHand] << FIRST_CARD_SHIFT;

            return retValType | retValTopCards;
        }
    }
}
