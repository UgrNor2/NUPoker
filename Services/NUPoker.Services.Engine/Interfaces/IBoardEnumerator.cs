namespace NUPoker.Services.Engine.Interfaces
{
    public interface IBoardEnumerator
    {
        IEnumerable<(int, int, int, int, int)> GetAllPossibleBoards(List<int> deadCards, int flopCard1 = 52, int flopCard2 = 52, int flopCard3 = 52, int turnCard = 52);
    }
}
