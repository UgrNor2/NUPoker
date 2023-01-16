using NUPoker.Services.Engine.Data.OddsCalculator;

namespace NUPoker.Services.Engine.Interfaces
{
    public interface IOddsCalculator
    {
        public OddsCalculatorResult GetOdds(List<(int, int)> playerCards, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52);
    }
}
