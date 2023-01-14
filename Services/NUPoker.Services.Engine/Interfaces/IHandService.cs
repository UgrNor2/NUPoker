using NUPoker.Services.Engine.Data;

namespace NUPoker.Services.Engine.Interfaces
{
    public interface IHandService
    {
        /// <summary>
        /// Returns hand representation on a 64 bit uint. Card value can be between 0 and 51. 
        /// 52 is valid for turn and river and it means that the specific card is not revealed yet.
        /// </summary>
        /// <param name="myCard1">My first card</param>
        /// <param name="myCard2">My second card</param>
        /// <param name="flopCard1">First flop card</param>
        /// <param name="flopCard2">Second flop card</param>
        /// <param name="flopCard3">Third flop card</param>
        /// <param name="turnCard">Turn card</param>
        /// <param name="riverCard">River card</param>
        /// <returns>Hand value</returns>
        /// <example>If myCard1 is 10, then in the result the 10th bit will be 1</example>
        ulong CreateHand(int myCard1, int myCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52, int riverCard = 52);

        /// <summary>
        /// Returns hand representation on a 64 bit uint.
        /// Cards.Empty is valid for turn and river and it means that the specific card is not revealed yet.
        /// </summary>
        /// <param name="myCard1">My first card</param>
        /// <param name="myCard2">My second card</param>
        /// <param name="flopCard1">First flop card</param>
        /// <param name="flopCard2">Second flop card</param>
        /// <param name="flopCard3">Third flop card</param>
        /// <param name="turnCard">Turn card</param>
        /// <param name="riverCard">River card</param>
        /// <returns>Hand value</returns>
        /// <example>If myCard1 is Queen Spades (10), then in the result the 10th bit will be 1</example>
        ulong CreateHand(Cards myCard1, Cards myCard2, Cards flopCard1, Cards flopCard2, Cards flopCard3, Cards turnCard = Cards.Empty, Cards riverCard = Cards.Empty);

        /// <summary>
        /// Returns the hand rank for a hand representation
        /// </summary>
        /// <param name="hand">Hand (From CreateHand)</param>
        /// <returns>Hand rank</returns>
        uint GetHandRank(ulong hand);

        /// <summary>
        /// Returns the hand rank for a hand representation
        /// </summary>
        /// <param name="myCard1">My first card</param>
        /// <param name="myCard2">My second card</param>
        /// <param name="flopCard1">First flop card</param>
        /// <param name="flopCard2">Second flop card</param>
        /// <param name="flopCard3">Third flop card</param>
        /// <param name="turnCard">Turn card</param>
        /// <param name="riverCard">River card</param>
        /// <returns>Hand rank</returns>
        uint GetHandRank(Cards myCard1, Cards myCard2, Cards flopCard1, Cards flopCard2, Cards flopCard3, Cards turnCard = Cards.Empty, Cards riverCard = Cards.Empty);

        /// <summary>
        /// Returns the hand rank for a hand representation
        /// </summary>
        /// <param name="myCard1">My first card</param>
        /// <param name="myCard2">My second card</param>
        /// <param name="flopCard1">First flop card</param>
        /// <param name="flopCard2">Second flop card</param>
        /// <param name="flopCard3">Third flop card</param>
        /// <param name="turnCard">Turn card</param>
        /// <param name="riverCard">River card</param>
        /// <returns>Hand rank</returns>
        uint GetHandRank(int myCard1, int myCard2, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52, int riverCard = 52);
    }
}
