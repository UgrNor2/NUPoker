using NUPoker.Services.Engine.Data;
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
            uint spadeHand = (uint)(hand & 0x1FFFUL); //Only spade cards in 13 bit
            uint heartHand = (uint)(hand >> 13 & 0x1FFFUL); //Only heart cards in 13 bit
            uint clubHand = (uint)(hand >> 26 & 0x1FFFUL); //Only club cards in 13 bit
            uint diamondHand = (uint)(hand >> 39 & 0x1FFFUL); //Only diamond cards in 13 bit

            int numberOfCards = Tables.NumberOfBitsTable[spadeHand] + Tables.NumberOfBitsTable[heartHand] + Tables.NumberOfBitsTable[clubHand] + Tables.NumberOfBitsTable[diamondHand];

            uint ranks = spadeHand | heartHand | clubHand | diamondHand;

            uint numberOfDifferentRanks = Tables.NumberOfBitsTable[ranks];
            uint numberOfDuplications = ((uint)(numberOfCards - numberOfDifferentRanks));

            var handTypeModel = RecognizeHandType(numberOfDifferentRanks, numberOfDuplications, spadeHand, heartHand, clubHand, diamondHand, ranks);

            switch(handTypeModel.HandType)
            {
                case HandTypes.HighCard: return GetHandRank_HighCard(ranks);
                case HandTypes.Pair: return GetHandRank_Pair(spadeHand, heartHand, clubHand, diamondHand, ranks);
                case HandTypes.TwoPairs: return GetHandRank_TwoPairs(spadeHand, heartHand, clubHand, diamondHand, ranks);
                case HandTypes.ThreeOfAKind: return GetHandRank_ThreeOfAKind(spadeHand, heartHand, clubHand, diamondHand, ranks);
                case HandTypes.Straight: return GetHandRank_Straight(ranks);
                case HandTypes.Flush: return GetHandRank_Flush(((FlushHandTypeRecognitionModel)handTypeModel).FlushSuitHand);
            }

            throw new NotImplementedException();
        }

        private static HandTypeRecognitionModel RecognizeHandType(uint numberOfDifferentRanks, uint numberOfDuplications, uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            if(numberOfDifferentRanks >= 5)
            {
                if (Tables.NumberOfBitsTable[spadeHand] >= 5)
                {
                    if (Tables.StraightTable[spadeHand] != 0)
                        return new HandTypeRecognitionModel { HandType = HandTypes.StraightFlush };
                    else
                        return new FlushHandTypeRecognitionModel { HandType = HandTypes.Flush, FlushSuitHand = spadeHand };
                }

                if (Tables.NumberOfBitsTable[heartHand] >= 5)
                {
                    if (Tables.StraightTable[heartHand] != 0)
                        return new HandTypeRecognitionModel { HandType = HandTypes.StraightFlush };
                    else
                        return new FlushHandTypeRecognitionModel { HandType = HandTypes.Flush, FlushSuitHand = heartHand };
                }

                if (Tables.NumberOfBitsTable[clubHand] >= 5)
                {
                    if (Tables.StraightTable[clubHand] != 0)
                        return new HandTypeRecognitionModel { HandType = HandTypes.StraightFlush };
                    else
                        return new FlushHandTypeRecognitionModel { HandType = HandTypes.Flush, FlushSuitHand = clubHand };
                }

                if (Tables.NumberOfBitsTable[diamondHand] >= 5)
                {
                    if (Tables.StraightTable[diamondHand] != 0)
                        return new HandTypeRecognitionModel { HandType = HandTypes.StraightFlush };
                    else
                        return new FlushHandTypeRecognitionModel { HandType = HandTypes.Flush, FlushSuitHand = diamondHand };
                }

                if (Tables.StraightTable[ranks] != 0)
                {
                    return new HandTypeRecognitionModel { HandType = HandTypes.Straight };
                }
            }

            switch (numberOfDuplications)
            {
                case 0: return new HandTypeRecognitionModel { HandType = HandTypes.HighCard }; ;
                case 1: return new HandTypeRecognitionModel { HandType = HandTypes.Pair }; ;
                case 2:
                    {
                        var pair_cards_mask = ranks ^ (spadeHand ^ heartHand ^ clubHand ^ diamondHand);
                        return pair_cards_mask != 0 ? new HandTypeRecognitionModel { HandType = HandTypes.TwoPairs } : new HandTypeRecognitionModel { HandType = HandTypes.ThreeOfAKind };
                    }                 
            }

            throw new NotImplementedException();
        }

        private static uint GetHandRank_HighCard(uint ranks)
        {
            return (((uint)HandTypes.HighCard) << TYPE_SHIFT) + Tables.TopFiveCardsTable[ranks];
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
            
            return retValType + retValPairCard + retValKickers;
        }

        private uint GetHandRank_TwoPairs(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            // Get the pair cards, it will contain 2 bits only
            var pairCardsMask = ranks ^ (spadeHand ^ heartHand ^ clubHand ^ diamondHand);

            // Get the other (not pair) cards' bits
            var otherCards = ranks ^ pairCardsMask;

            // Set the TwoPairs hand type, then set the two pairs ranks and finally we set the top card from the other (not pair) cards and set it into the third card place.
            var retValType = (uint)HandTypes.TwoPairs << TYPE_SHIFT;
            var retValPairCards = Tables.TopFiveCardsTable[pairCardsMask] & 0x000FF000;
            var retValKicker = (uint)(Tables.TopCardTable[otherCards] << THIRD_CARD_SHIFT);

            return retValType + retValPairCards + retValKicker;
        }

        private uint GetHandRank_ThreeOfAKind(uint spadeHand, uint heartHand, uint clubHand, uint diamondHand, uint ranks)
        {
            // Get the three of a kind card, it will contain 1 bit only
            var threeCardMask = ((clubHand & diamondHand) | (heartHand & spadeHand)) & ((clubHand & heartHand) | (diamondHand & spadeHand));

            // Set ThreeOfAKind hand type, then set the ThreeOfAKind card to the first card place
            var retValType = (uint)HandTypes.ThreeOfAKind << TYPE_SHIFT;
            var retValThreeOfAKindCard = (uint)Tables.TopCardTable[threeCardMask] << FIRST_CARD_SHIFT;

            // Get the other (not three of a kind) cards' bits
            var otherCards = ranks ^ threeCardMask;

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

            return retValType + retValThreeOfAKindCard + retValFirstKickerCard + retValSecondKickerCard;
        }

        private uint GetHandRank_Straight(uint ranks)
        {
            // The top card
            var topCard = (uint)Tables.StraightTable[ranks];

            // Set Straight hand type, then set the top card of the straight to the first card place
            var retValType = (uint)HandTypes.Straight << TYPE_SHIFT;
            var retValTopCard = topCard << FIRST_CARD_SHIFT;

            return retValType + retValTopCard;
        }

        private uint GetHandRank_Flush(uint flushSuitHand)
        {
            // Set Flush hand type, then get the top 5 cards
            var retValType = (uint)HandTypes.Flush << TYPE_SHIFT;
            var retValTopCards = Tables.TopFiveCardsTable[flushSuitHand];

            return retValType + retValTopCards;
        }

        private class HandTypeRecognitionModel
        {
            public HandTypes HandType { get; init; }
        }

        private class FlushHandTypeRecognitionModel : HandTypeRecognitionModel
        {
            public uint FlushSuitHand { get; init; }
        }

    }
}
