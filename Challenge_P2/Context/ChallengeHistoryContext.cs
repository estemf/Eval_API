using Microsoft.EntityFrameworkCore;

namespace Challenge_P2.Context
{
    public class ChallengeHistoryContext : DbContext
    {
        public DbSet<ChallengeHistory> ChallengeHistories { get; set; }

        // Autres propriétés DbSet pour d’autres entités comme Challenge, User, etc.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallengeHistory>()
                .HasKey(ch => ch.Id);

            modelBuilder.Entity<ChallengeHistory>()
                .HasOne(ch => ch.User)
                .WithMany(u => u.ChallengeHistories)
                .HasForeignKey(ch => ch.UserId);

            modelBuilder.Entity<ChallengeHistory>()
                .HasOne(ch => ch.Challenge)
                .WithMany(c => c.ChallengeHistories)
                .HasForeignKey(ch => ch.ChallengeId);
        }
    }
}
