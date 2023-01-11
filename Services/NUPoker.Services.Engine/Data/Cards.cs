using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUPoker.Services.Engine.Data
{
    public enum Cards
    {
        TwoSpades = (int)Suits.Spade * 13 + (int)Ranks.Two,
        ThreeSpades = (int)Suits.Spade * 13 + (int)Ranks.Three,
        FourSpades = (int)Suits.Spade * 13 + (int)Ranks.Four,
        FiveSpades = (int)Suits.Spade * 13 + (int)Ranks.Five,
        SixSpades = (int)Suits.Spade * 13 + (int)Ranks.Six,
        SevenSpades = (int)Suits.Spade * 13 + (int)Ranks.Seven,
        EightSpades = (int)Suits.Spade * 13 + (int)Ranks.Eight,
        NineSpades = (int)Suits.Spade * 13 + (int)Ranks.Nine,
        TenSpades = (int)Suits.Spade * 13 + (int)Ranks.Ten,
        JackSpades = (int)Suits.Spade * 13 + (int)Ranks.Jack,
        QueenSpades = (int)Suits.Spade * 13 + (int)Ranks.Queen,
        KingSpades = (int)Suits.Spade * 13 + (int)Ranks.King,
        AceSpades = (int)Suits.Spade * 13 + (int)Ranks.Ace,
        TwoHearts = (int)Suits.Heart * 13 + (int)Ranks.Two,
        ThreeHearts = (int)Suits.Heart * 13 + (int)Ranks.Three,
        FourHearts = (int)Suits.Heart * 13 + (int)Ranks.Four,
        FiveHearts = (int)Suits.Heart * 13 + (int)Ranks.Five,
        SixHearts = (int)Suits.Heart * 13 + (int)Ranks.Six,
        SevenHearts = (int)Suits.Heart * 13 + (int)Ranks.Seven,
        EightHearts = (int)Suits.Heart * 13 + (int)Ranks.Eight,
        NineHearts = (int)Suits.Heart * 13 + (int)Ranks.Nine,
        TenHearts = (int)Suits.Heart * 13 + (int)Ranks.Ten,
        JackHearts = (int)Suits.Heart * 13 + (int)Ranks.Jack,
        QueenHearts = (int)Suits.Heart * 13 + (int)Ranks.Queen,
        KingHearts = (int)Suits.Heart * 13 + (int)Ranks.King,
        AceHearts = (int)Suits.Heart * 13 + (int)Ranks.Ace,
        TwoClubs = (int)Suits.Club * 13 + (int)Ranks.Two,
        ThreeClubs = (int)Suits.Club * 13 + (int)Ranks.Three,
        FourClubs = (int)Suits.Club * 13 + (int)Ranks.Four,
        FiveClubs = (int)Suits.Club * 13 + (int)Ranks.Five,
        SixClubs = (int)Suits.Club * 13 + (int)Ranks.Six,
        SevenClubs = (int)Suits.Club * 13 + (int)Ranks.Seven,
        EightClubs = (int)Suits.Club * 13 + (int)Ranks.Eight,
        NineClubs = (int)Suits.Club * 13 + (int)Ranks.Nine,
        TenClubs = (int)Suits.Club * 13 + (int)Ranks.Ten,
        JackClubs = (int)Suits.Club * 13 + (int)Ranks.Jack,
        QueenClubs = (int)Suits.Club * 13 + (int)Ranks.Queen,
        KingClubs = (int)Suits.Club * 13 + (int)Ranks.King,
        AceClubs = (int)Suits.Club * 13 + (int)Ranks.Ace,
        TwoDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Two,
        ThreeDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Three,
        FourDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Four,
        FiveDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Five,
        SixDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Six,
        SevenDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Seven,
        EightDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Eight,
        NineDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Nine,
        TenDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Ten,
        JackDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Jack,
        QueenDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Queen,
        KingDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.King,
        AceDiamonds = (int)Suits.Diamond * 13 + (int)Ranks.Ace,
        Empty = 52
    }
}
