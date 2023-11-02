using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.DB;

namespace ProjectChronos.Services
{
    public class CardPackService : ICardPackService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpressApiService _expressApiService;
        private readonly IConfiguration Configuration;

        private string ContractAddress => Configuration["ContractAddress"];

        private string OwnerAddress => Configuration["OwnerAddress"];

        private int Retries => int.Parse(Configuration["ExpressRequestRetries"]);


        private readonly CardPackTemplate _welcomePackTemplate = new CardPackTemplate()
        {
            Name = "Welcome Pack",
            Description = "A pack of cards to get you started",
            ImageUrl = "",
            Type = CardPackType.WelcomePack,
            RewardsPerPack = 3,
            RewardTemplates = new List<ICardPackRewardTemplate>()
            {
                new CardPackRewardTemplate()
                {
                    ContractAddress = "0x72d1137eaB36EE1C9BAfB12dE74Ed683e5407508",
                    TokenId = 0,
                    QuantityPerReward = 1,
                    TotalRewards = 12
                },
                new CardPackRewardTemplate()
                {
                    ContractAddress = "0x72d1137eaB36EE1C9BAfB12dE74Ed683e5407508",
                    TokenId = 1,
                    QuantityPerReward = 1,
                    TotalRewards = 3
                }
            }
        };


        public CardPackService(ApplicationDbContext dbContext, IExpressApiService expressApiService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
            Configuration = configuration;

            // Check if contract address is set
            if (string.IsNullOrEmpty(ContractAddress))
            {
                throw new Exception("Contract address not set in configuration");
            }
            // Check if owner address is set
            if (string.IsNullOrEmpty(OwnerAddress))
            {
                throw new Exception("Owner address not set in configuration");
            }

            // Update contract address on welcome pack template
            _welcomePackTemplate.RewardTemplates.ToList().ForEach(x => x.ContractAddress = ContractAddress);
        }

        public Task<CreatedPacks> CreatePacksAsync(int cardPackTemplateId)
        {
            var packTemplate = _dbContext.CardPackTemplates
                .Include(cpt => cpt.RewardTemplates)
                .FirstOrDefault(x => x.Id == cardPackTemplateId);

            if(packTemplate == null)
            {
                throw new Exception($"Card pack template not found for id {cardPackTemplateId}");
            }

            return CreatePacksAsync(packTemplate);
        }

        public Task<CreatedPacks> CreatePacksAsync(CardPackType type)
        {
            var packTemplate = _dbContext.CardPackTemplates
                .Include(cpt => cpt.RewardTemplates)
                .FirstOrDefault(x => x.Type == type);

            if(packTemplate == null)
            {
                throw new Exception($"Card pack template not found for type {type}");
            }

            return CreatePacksAsync(packTemplate);
        }

        private record RewardsToCreate
        {
            public ICardPackRewardTemplate RewardTemplate;
            public int Quantity;
        }

        public async Task<CreatedPacks> CreatePacksAsync(ICardPackTemplate packTemplate)
        {
            if (packTemplate == null)
            {
                throw new Exception("Card pack template is null");
            }
            if(packTemplate.RewardTemplates == null || packTemplate.RewardTemplates.Count == 0)
            {
                throw new Exception("Card pack template has no rewards");
            }

            var rewards = packTemplate.RewardTemplates.ToList();
            

            // Check if NFTs exist for each reward
            var ownedNftsResponse = await _expressApiService.GetOwnedNftsAsync(OwnerAddress);
            if(!ownedNftsResponse.Success || ownedNftsResponse.Data is null)
            {
                throw new Exception("Failed to get owned NFTs");
            }

            var ownedNfts = ownedNftsResponse.Data.ToList();
            var rewardsToCreate = new List<RewardsToCreate>();
            foreach (var reward in rewards)
            {
                var ownedNft = ownedNftsResponse.Data.FirstOrDefault(nft => nft.Metadata.Id == reward.TokenId.ToString());
                if(ownedNft == null)
                {
                    rewardsToCreate.Add(new RewardsToCreate()
                    {
                        RewardTemplate = reward,
                        Quantity = reward.TotalRewards
                    });
                }
                else if(int.Parse(ownedNft.QuantityOwned) < reward.TotalRewards)
                {
                    rewardsToCreate.Add(new RewardsToCreate()
                    {
                        RewardTemplate = reward,
                        Quantity = reward.TotalRewards - int.Parse(ownedNft.QuantityOwned)
                    });
                }
            }

            // Create missing NFTs for each reward
            var groupedToCreate = rewardsToCreate.GroupBy(x => x.RewardTemplate.TokenId);

            foreach (var group in groupedToCreate)
            {
                var token = group.Key;
                var quantity = group.Sum(x => x.Quantity);

                var currentRetries = 0;
                var nftCreated = false;

                // Inception loop
                while (currentRetries < Retries && !nftCreated)
                {
                    try
                    {
                        var createNftResponse = await _expressApiService.ClaimNftsAsync(token, quantity);
                        if(!createNftResponse)
                        {
                            continue;
                        }

                        // now refetch owned nfts to see if they were created
                        ownedNftsResponse = await _expressApiService.GetOwnedNftsAsync(OwnerAddress);
                        if (!ownedNftsResponse.Success || ownedNftsResponse.Data is null)
                        {
                            throw new Exception("Failed to get owned NFTs");
                        }
                        ownedNfts = ownedNftsResponse.Data.ToList();

                        if(createNftResponse && ownedNfts.Any(nft => nft.Metadata.Id == token.ToString() && int.Parse(nft.QuantityOwned) >= quantity))
                        {
                            nftCreated = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        currentRetries++;
                    }
                    // Add some delay to avoid spamming the API
                    Thread.Sleep(500 * currentRetries);
                }

                if(!nftCreated)
                {
                    throw new Exception($"Failed to create NFTs for token {token} and quantity {quantity} after multiple retries");
                }
            }

            // Now all NFTs should exist, time to create packs

            var internalId = Guid.NewGuid().ToString();
            var packsCreated = await _expressApiService.CreatePacksAsync(packTemplate, internalId);
            if(!packsCreated)
            {
                throw new Exception("Failed to create packs");
            }

            // Refetch owned packs to see if they were created
            // TODO: both packs and nfts creation can take a while, so we should probably wait a bit before refetching
            var ownedPacksResponse = await _expressApiService.GetOwnedPacksAsync(OwnerAddress);
            if (!ownedPacksResponse.Success || ownedPacksResponse.Data is null)
            {
                throw new Exception("Failed to get owned packs");
            }

            var ownedPacks = ownedPacksResponse.Data.ToList();
            var createdPack = ownedPacks.FirstOrDefault(pack => pack.Metadata.InternalId == internalId);
            if(createdPack == null)
            {
                throw new Exception($"Failed to find created pack with internal id {internalId}");
            }

            // Pack created, now we need to insert it into the database
            var createdPackEntity = new CreatedPacks()
            {
                CardPackTemplateId = packTemplate.Id,
                QuantityRemaining = int.Parse(createdPack.QuantityOwned),
                Quantity = int.Parse(createdPack.QuantityOwned),
                InternalId = internalId,
                CreatedAt = DateTime.UtcNow,
            };

            _dbContext.CreatedPacks.Add(createdPackEntity);
            _dbContext.SaveChanges();

            return createdPackEntity;
        }

        public bool EnsureWelcomePackTemplateExists()
        {
            try
            {
                // Check if welcome pack template exists
                var welcomePackTemplate = _dbContext.CardPackTemplates
                    .Include(cpt => cpt.RewardTemplates)
                    .FirstOrDefault(x => x.Type == CardPackType.WelcomePack);

                if (welcomePackTemplate == null)
                {
                       // Create welcome pack template
                    _dbContext.CardPackTemplates.Add(_welcomePackTemplate);
                    _dbContext.SaveChanges();
                    return true;
                }

                // Check if welcome pack template has correct rewards
                var rewards = welcomePackTemplate.RewardTemplates.ToList();
                var rewardsMatch = rewards.Count == _welcomePackTemplate.RewardTemplates.Count;

                if(rewardsMatch)
                {
                    foreach (var reward in rewards)
                    {
                        var match = _welcomePackTemplate.RewardTemplates.FirstOrDefault(x => x.TokenId == reward.TokenId && x.QuantityPerReward == reward.QuantityPerReward && x.TotalRewards == reward.TotalRewards);
                        if (match == null)
                        {
                            rewardsMatch = false;
                            break;
                        }
                    }
                }

                if(!rewardsMatch)
                {
                    // Update welcome pack template rewards
                    welcomePackTemplate.RewardTemplates = _welcomePackTemplate.RewardTemplates;

                    _dbContext.CardPackTemplates.Update(welcomePackTemplate);
                    _dbContext.SaveChanges();
                }

                // Check if welcome pack template has correct properties
                var propertiesMatch = welcomePackTemplate.Name == _welcomePackTemplate.Name &&
                    welcomePackTemplate.Description == _welcomePackTemplate.Description &&
                    welcomePackTemplate.ImageUrl == _welcomePackTemplate.ImageUrl &&
                    welcomePackTemplate.RewardsPerPack == _welcomePackTemplate.RewardsPerPack;

                if(!propertiesMatch)
                {
                    // Update welcome pack template
                    welcomePackTemplate.Name = _welcomePackTemplate.Name;
                    welcomePackTemplate.Description = _welcomePackTemplate.Description;
                    welcomePackTemplate.ImageUrl = _welcomePackTemplate.ImageUrl;
                    welcomePackTemplate.Type = _welcomePackTemplate.Type;
                    welcomePackTemplate.RewardsPerPack = _welcomePackTemplate.RewardsPerPack;

                    _dbContext.CardPackTemplates.Update(welcomePackTemplate);
                    _dbContext.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {


            }

            return false;
        }

        public int GetPacksRemaining(CardPackType type)
        {
           return _dbContext.CreatedPacks
                .Where(cp => cp.CardPackTemplate.Type == type)
                .Select(cp => cp.QuantityRemaining)
                .Sum();
        }
    }
}
