using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Common.Interfaces.Services;
using ProjectChronos.Common.Models;
using ProjectChronos.Common.Models.Enums;
using ProjectChronos.DB;
using System.Collections.ObjectModel;

namespace ProjectChronos.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IExpressApiService _expressApiService;
        private readonly ICardDeckService _cardDeckService;

        public UserStatsService(ApplicationDbContext dbContext, IExpressApiService expressApiService, ICardDeckService cardDeckService)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
            _cardDeckService = cardDeckService;

        }

        public async Task<CompositeUserStats> GetCompositeUserStatsAsync(IUser user)
        {
            try
            {
                var stats = new CompositeUserStats();

                // Fill user general info
                stats.UserId = user.Id;
                stats.UserAddress = user.UserName;

                var ownedCardsResult = await _expressApiService.GetOwnedNftsAsync(user.UserName);

                if(ownedCardsResult.Success)
                {
                    var ownedCards = ownedCardsResult.Data;
                    // Fill user card stats
                    stats.TotalOwnedCards = ownedCards.Sum(c => int.Parse(c.QuantityOwned));
                    stats.TotalUniqueOwnedCards = ownedCards.Count();

                    var totalOwnedCardsByElement = new UserStatTotalGroup()
                    {
                        GroupName = "Element",
                        Items = new List<UserStatTotalGroupItem>()
                    };

                    foreach (string element in Enum.GetNames(typeof(ElementType)))
                    {
                        totalOwnedCardsByElement.Items.Add(new UserStatTotalGroupItem()
                        {
                            Name = element,
                            Total = ownedCards.Where(c => c.Metadata.Element == element).Sum(c => int.Parse(c.QuantityOwned))
                        });
                    }
                }
      
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;
        }
    }
}
