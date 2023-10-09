using ProjectChronos.Common.Models.Enums;

namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface ICardPackTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public CardPackType Type { get; set; }

        public int RewardsPerPack { get; set; }

        public ICollection<ICardPackRewardTemplate> RewardTemplates { get; set; }

        public ICollection<ICreatedPacks> CreatedPacks { get; set; }
    }
}
