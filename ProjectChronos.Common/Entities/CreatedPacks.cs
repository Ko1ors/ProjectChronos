using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Entities
{
    public class CreatedPacks : ICreatedPacks
    {
        public int Id { get; set; }

        public int CardPackTemplateId { get; set; }

        public int Quantity { get; set; }

        public int QuantityRemaining { get; set; }

        public string InternalId { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICardPackTemplate CardPackTemplate { get; set; }


    }
}
