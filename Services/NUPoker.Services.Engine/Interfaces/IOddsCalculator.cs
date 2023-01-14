using NUPoker.Services.Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.Interfaces
{
    public interface IOddsCalculator
    {
        public double[][] GetOdds(List<(int, int)> playerCards, int flopCard1, int flopCard2, int flopCard3, int turnCard = 52);
    }
}
