namespace NUPoker.Services.Engine.Interfaces
{
    public interface ICardValidator
    {
        void ThrowArgumentExceptionIfCardIsOutOfRange(int card, bool canBeEmpty = false);
    }
}
