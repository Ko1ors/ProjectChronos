namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface IUser
    {
        public string Id { get; set; }

        public ICollection<IUserDeck> UserDecks { get; set; }
    }
}
