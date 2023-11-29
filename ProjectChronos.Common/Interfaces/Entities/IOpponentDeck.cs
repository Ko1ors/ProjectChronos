namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IOpponentDeck : IUserDeck
    {
        public IOpponent Opponent { get; set; }
    }
}
