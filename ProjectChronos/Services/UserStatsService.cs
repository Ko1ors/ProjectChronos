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
        private readonly IGameSystemService _gameSystemService;

        public UserStatsService(ApplicationDbContext dbContext, IExpressApiService expressApiService, ICardDeckService cardDeckService, IGameSystemService gameSystemService)
        {
            _dbContext = dbContext;
            _expressApiService = expressApiService;
            _cardDeckService = cardDeckService;
            _gameSystemService = gameSystemService;
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


                    // Get total owned cards by type
                    var totalOwnedCardsByElement = new UserStatTotalGroup()
                    {
                        GroupName = "Element",
                        Items = new List<UserStatTotalGroupItem>()
                    };

                    var groupedByElement = ownedCards.GroupBy(c => c.Metadata.Element);
                    foreach (var group in groupedByElement)
                    {
                        totalOwnedCardsByElement.Items.Add(new UserStatTotalGroupItem()
                        {
                            Name = group.Key,
                            Value = group.Sum(c => int.Parse(c.QuantityOwned))
                        });
                    }
                    stats.TotalOwnedCardsByElement = totalOwnedCardsByElement;


                    // Get total owned cards by rarity
                    var totalOwnedCardsByRarity = new UserStatTotalGroup()
                    {
                        GroupName = "Rarity",
                        Items = new List<UserStatTotalGroupItem>()
                    };

                    var groupedByRarity = ownedCards.GroupBy(c => c.Metadata.Rarity);
                    foreach (var group in groupedByRarity)
                    {
                        totalOwnedCardsByRarity.Items.Add(new UserStatTotalGroupItem()
                        {
                            Name = group.Key,
                            Value = group.Sum(c => int.Parse(c.QuantityOwned))
                        });
                    }
                    stats.TotalOwnedCardsByRarity = totalOwnedCardsByRarity;


                    // Get total owned cards by class
                    var totalOwnedCardsByClass = new UserStatTotalGroup()
                    {
                        GroupName = "Class",
                        Items = new List<UserStatTotalGroupItem>()
                    };

                    var groupedByType = ownedCards.GroupBy(c => c.Metadata.Class);
                    foreach (var group in groupedByType)
                    {
                        totalOwnedCardsByClass.Items.Add(new UserStatTotalGroupItem()
                        {
                            Name = group.Key,
                            Value = group.Sum(c => int.Parse(c.QuantityOwned))
                        });
                    }
                    stats.TotalOwnedCardsByClass = totalOwnedCardsByClass;
                }

                // Fill user match stats
                var userMatches = _gameSystemService.GetAllUserMatches(user);

                stats.Wins = userMatches.Count(m => m.Result == MatchResultType.Win);
                stats.Losses = userMatches.Count(m => m.Result == MatchResultType.Loss);
                stats.Draws = userMatches.Count(m => m.Result == MatchResultType.Draw);

                stats.TotalMatches = userMatches.Count();

                return stats;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return default;
        }
    }
}
