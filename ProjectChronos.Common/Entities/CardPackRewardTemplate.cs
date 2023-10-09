using ProjectChronos.Common.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectChronos.Common.Entities
{
    public class CardPackRewardTemplate : ICardPackRewardTemplate
    {
        public int Id { get; set; }

        [Required]
        public string ContractAddress { get; set; }

        public int TokenId { get; set; }

        public int QuantityPerReward { get; set; }

        public int TotalRewards { get; set; }

        public ICollection<ICardPackTemplate> CardPackTemplates { get; set; }
    }
}
