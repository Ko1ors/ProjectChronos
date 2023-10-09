using ProjectChronos.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectChronos.Common.Entities;
using System.Collections.Generic;
using System.Collections;

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
    }
}