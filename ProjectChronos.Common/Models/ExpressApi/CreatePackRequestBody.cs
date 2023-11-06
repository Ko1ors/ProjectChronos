using Newtonsoft.Json;
using ProjectChronos.Common.Interfaces.Entities;

namespace ProjectChronos.Common.Models.ExpressApi
{
    public class CreatePackRequestBody
    {

        public CreatePackRequestBody()
        {

        }

        public CreatePackRequestBody(ICardPackTemplate packTemplate, string internalId = "")
        {
            Name = packTemplate.Name;
            Description = packTemplate.Description;
            ImageUrl = packTemplate.ImageUrl;
            InternalId = internalId;
            RewardsPerPack = packTemplate.RewardsPerPack;
            Rewards = packTemplate.RewardTemplates.Select(x => new CreatePackRequestBodyReward
            {
                ContractAddress = x.ContractAddress,
                TokenId = x.TokenId,
                QuantityPerReward = x.QuantityPerReward,
                TotalRewards = x.TotalRewards
            });

        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("image")]
        public string? ImageUrl { get; set; }

        [JsonProperty("internalId")]
        public string InternalId { get; set; }

        [JsonProperty("rewardsPerPack")]
        public int RewardsPerPack { get; set; }

        [JsonProperty("rewards")]
        public IEnumerable<CreatePackRequestBodyReward> Rewards { get; set; }
    }

    public class CreatePackRequestBodyReward
    {
        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("tokenId")]
        public int TokenId { get; set; }

        [JsonProperty("quantityPerReward")]
        public int QuantityPerReward { get; set; }

        [JsonProperty("totalRewards")]
        public int TotalRewards { get; set; }
    }
}
