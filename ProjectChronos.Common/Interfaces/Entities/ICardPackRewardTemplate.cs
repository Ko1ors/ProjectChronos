namespace ProjectChronos.Common.Interfaces.Entities
{
    public interface ICardPackRewardTemplate
    {
        public int Id { get; set; }

        public string ContractAddress { get; set; }

        public int TokenId { get; set; }

        public int QuantityPerReward { get; set; }

        public int TotalRewards { get; set; }

        public ICollection<ICardPackTemplate> CardPackTemplates { get; set; }
    }
}
