using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectChronos.Common.Entities
{
    public class CardPackTemplate : ICardPackTemplate
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public CardPackType Type { get; set; }

        public int RewardsPerPack { get; set; }

        public ICollection<ICardPackRewardTemplate> RewardTemplates { get; set; }

        public ICollection<ICreatedPacks> CreatedPacks { get; set; }
    }
}
