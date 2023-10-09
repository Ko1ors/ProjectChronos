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

        private readonly string _contractAddress = "";

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
            if (string.IsNullOrEmpty(Configuration["ContractAddress"]))
            {
                throw new Exception("Contract address not set in configuration");
            }

            _contractAddress = Configuration["ContractAddress"]!;

            // Update contract address on welcome pack template
            _welcomePackTemplate.RewardTemplates.ToList().ForEach(x => x.ContractAddress = _contractAddress);
        }

        public int CreatePacks(int cardPackTemplateId)
        {
            throw new NotImplementedException();
        }

        public int CreatePacks(CardPackType type)
        {
            throw new NotImplementedException();
        }

        public int CreatePacks(ICardPackTemplate packTemplate)
        {
            throw new NotImplementedException();
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
