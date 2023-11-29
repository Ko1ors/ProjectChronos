namespace ProjectChronos.Common.Models
{
    public class CompositeUserStats
    {
        // User General Info
        public string UserId { get; set; }

        public string UserAddress { get; set; }


        // Cards Stats
        public int TotalOwnedCards { get; set; }

        public int TotalUniqueOwnedCards { get; set; }

        public UserStatTotalGroup TotalOwnedCardsByElement { get; set; }

        public UserStatTotalGroup TotalOwnedCardsByRarity { get; set; }

        public UserStatTotalGroup TotalOwnedCardsByType { get; set; }

        // Match Stats
        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Draws { get; set; }

        public int TotalMatches { get; set; }
    }

    public class UserStatTotalGroup
    {
        public string GroupName { get; set; }

        public List<UserStatTotalGroupItem> Items { get; set; }
    }


    public class UserStatTotalGroupItem
    {
        public string Name { get; set; }
        
        public int Value { get; set; }
    }
}
