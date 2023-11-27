using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.Common.Models.ExpressApi;

namespace ProjectChronos.Models
{
    public class MatchCardExtended
    {
        public MatchCardExtended(IMatchDrawnCard card, NftMetadata metadata)
        {
            Card = card;
            Metadata = metadata;

            CurrentHealth = BaseHealth;
        }

        public IMatchDrawnCard Card { get; set; }

        public NftMetadata Metadata { get; set; }

        public int BaseHealth => Metadata.Health;

        public int CurrentHealth { get; set; }

        public int Agility => Metadata.Agility;

        public int Power => Metadata.Power;

        public string Class => Metadata.Class;

        public bool IsMelee => Class == "Melee";

        public bool IsRanged => Class == "Ranged";

        public ElementType Element => Enum.TryParse(Metadata.Element, out ElementType element) ? element : default;

        public bool IsAlive => CurrentHealth > 0;
    }
}
