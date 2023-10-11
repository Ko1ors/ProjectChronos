using ProjectChronos.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

namespace ProjectChronos.DB
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<CardPackRewardTemplate> CardPackRewardTemplates { get; set; }

        public DbSet<CardPackTemplate> CardPackTemplates { get; set; }

        public DbSet<CreatedPacks> CreatedPacks { get; set; }

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