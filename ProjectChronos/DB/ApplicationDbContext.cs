using Azure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using ProjectChronos.Common.Interfaces.Entities;
using ProjectChronos.Entities;
using System.Reflection;

namespace ProjectChronos.DB
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<CardPackRewardTemplate> CardPackRewardTemplates { get; set; }

        public DbSet<CardPackTemplate> CardPackTemplates { get; set; }

        public DbSet<CreatedPacks> CreatedPacks { get; set; }

        public DbSet<DeckCard> DeckCards { get; set; }

        public DbSet<UserDeck> UserDecks { get; set; }

        public DbSet<Opponent> Opponents { get; set; }

        public DbSet<OpponentDeck> OpponentDecks { get; set; }

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardPackTemplate>()
                .HasMany(cpt => cpt.RewardTemplates as ICollection<CardPackRewardTemplate>)
                .WithMany(cprt => cprt.CardPackTemplates as ICollection<CardPackTemplate>);

            modelBuilder.Entity<CreatedPacks>()
                .HasOne(cprt => cprt.CardPackTemplate as CardPackTemplate)
                .WithMany(cpt => cpt.CreatedPacks as ICollection<CreatedPacks>);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserDecks as ICollection<UserDeck>)
                .WithOne(ud => ud.User as User);

            modelBuilder.Entity<UserDeck>()
                .HasMany(ud => ud.DeckCards as ICollection<DeckCard>)
                .WithOne(dc => dc.UserDeck as UserDeck);

            modelBuilder.Entity<Opponent>()
                .HasOne(o => o.OpponentDeck as OpponentDeck)
                .WithOne(od => od.Opponent as Opponent);

            modelBuilder.Entity<Opponent>()
                .HasOne(o => o.User as User);

            modelBuilder.Entity<Opponent>()
                .HasMany(o => o.OpponentUsers as ICollection<User>)
                .WithMany(u => u.Opponents as ICollection<Opponent>)
                .UsingEntity(
            "UserOpponent",
            l => l
                .HasOne(typeof(User))
                .WithMany()
                .HasForeignKey("UserId")
                .HasPrincipalKey(nameof(User.Id))
                .OnDelete(DeleteBehavior.Restrict),
            r => r
                .HasOne(typeof(Opponent))
                .WithMany()
                .HasForeignKey("OpponentId")
                .HasPrincipalKey(nameof(Opponent.Id))
                .OnDelete(DeleteBehavior.Cascade),
            j => j.HasKey("UserId", "OpponentId")
        );
        }

        // For debug purposes
        // This method is used to check if the DbContext is disposed
        public bool IsDisposed()
        {
            bool result = true;
            var typeDbContext = typeof(DbContext);
            var isDisposedTypeField = typeDbContext.GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);

            if (isDisposedTypeField != null)
            {
                result = (bool)isDisposedTypeField.GetValue(this);
            }

            return result;
        }
    }
}