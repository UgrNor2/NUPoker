namespace NUPoker.Services.Engine.Data.OddsCalculator
{
    public class PlayerOddsResult
    {
        public PlayerOddsResult()
        {
            NumberOfHandOccurences = new int[9];
            NumberOfWins = new int[9];
            NumberOfTies= new int[9];
        }

        public int[] NumberOfHandOccurences { get; init; }

        public int[] NumberOfWins { get; init; }

        public int[] NumberOfTies { get; init; }

        public double[] GetOccurancePercentages() => NumberOfHandOccurences.Select(x => (double)x / TotalHandOccurances() * 100).ToArray();

        public double GetWinPercentage() => ((double)TotalNumberOfWins() / TotalNumberOfOccurences()) * 100;

        public double GetTiePercentage() => ((double)TotalNumberOfTies() / TotalNumberOfOccurences()) * 100;

        private int TotalNumberOfOccurences() => NumberOfHandOccurences.Sum();

        private int TotalNumberOfWins() => NumberOfWins.Sum();

        private int TotalNumberOfTies() => NumberOfTies.Sum();

        private int TotalHandOccurances() => NumberOfHandOccurences.Sum();
    }
}