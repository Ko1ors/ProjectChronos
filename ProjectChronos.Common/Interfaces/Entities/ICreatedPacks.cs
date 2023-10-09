namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface ICreatedPacks
    {
        public int Id { get; set; }

        public int CardPackTemplateId { get; set; }

        public int Quantity { get; set; }

        public int QuantityRemaining { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICardPackTemplate CardPackTemplate { get; set; }
    }
}
