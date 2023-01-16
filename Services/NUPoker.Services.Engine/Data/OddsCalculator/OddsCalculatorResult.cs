using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.Data.OddsCalculator
{
    public class OddsCalculatorResult
    {
        public OddsCalculatorResult(int numberOfPlayers, int numberOfCases)
        {
            NumberOfPlayers = numberOfPlayers;
            NumberOfCases = numberOfCases;
            PlayerOddsResults = new List<PlayerOddsResult>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerOddsResults.Add(new PlayerOddsResult());
            }
        }

        public int NumberOfPlayers { get; init; }

        public int NumberOfCases { get; init; }

        public List<PlayerOddsResult> PlayerOddsResults { get; init; }
    }
}
