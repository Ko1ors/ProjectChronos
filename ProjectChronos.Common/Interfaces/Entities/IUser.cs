namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IUser
    {
        public ICollection<IUserDeck> UserDecks { get; set; }
    }
}
