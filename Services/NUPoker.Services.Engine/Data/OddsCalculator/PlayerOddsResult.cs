namespace NUPoker.Services.Engine.Data.OddsCalculator
{
    public class PlayerOddsResult
    {
        private double[]? _occurancePercentages = null;
        private double? _winPercentage = null;
        private double? _tiePercentage = null;
        private int? _totalNumberOfOccurences = null;
        private int? _totalNumberOfWins = null;
        private int? _totalNumberOfTies = null;

        public PlayerOddsResult()
        {
            NumberOfHandOccurences = new int[9];
            NumberOfWins = new int[9];
            NumberOfTies= new int[9];
        }

        public int[] NumberOfHandOccurences { get; init; }

        public int[] NumberOfWins { get; init; }

        public int[] NumberOfTies { get; init; }

        public double[] OccurancePercentages
        {
            get
            {
                if(_occurancePercentages == null)
                {
                    _occurancePercentages = NumberOfHandOccurences.Select(x => (double)x / TotalNumberOfOccurences * 100).ToArray();
                }

                return _occurancePercentages;
            }
        }

        public double WinPercentage
        {
            get
            {
                if (_winPercentage == null)
                {
                    _winPercentage = ((double)TotalNumberOfWins / TotalNumberOfOccurences) * 100;
                }

                return _winPercentage.Value;
            }
        }

        public double TiePercentage
        {
            get
            {
                if (_tiePercentage == null)
                {
                    _tiePercentage = ((double)TotalNumberOfTies / TotalNumberOfOccurences) * 100;
                }

                return _tiePercentage.Value;
            }
        }

        public int TotalNumberOfOccurences
        {
            get
            {
                if(_totalNumberOfOccurences == null)
                {
                    _totalNumberOfOccurences = NumberOfHandOccurences.Sum();
                }

                return _totalNumberOfOccurences.Value;
            }
        }

        public int TotalNumberOfWins
        {
            get
            {
                if (_totalNumberOfWins == null)
                {
                    _totalNumberOfWins = NumberOfWins.Sum();
                }

                return _totalNumberOfWins.Value;
            }
        }

        public int TotalNumberOfTies
        {
            get
            {
                if (_totalNumberOfTies == null)
                {
                    _totalNumberOfTies = NumberOfTies.Sum();
                }

                return _totalNumberOfTies.Value;
            }
        }

    }
}