using Microsoft.EntityFrameworkCore;
using StravaClubMonthlyDistanceStandings.Database.DbModels;

namespace StravaClubMonthlyDistanceStandings.Database
{
    public class SqLiteDbContext : DbContext
    {
        public SqLiteDbContext()
        {
        }

        public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AthleteSummary> AthleteSummaries { get; set; }
        public virtual DbSet<MonthlySummary> MonthlySummaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=C:\\MCS.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AthleteSummary>(entity =>
            {
                entity.ToTable("AthleteSummary");

                entity.HasIndex(e => e.AthleteSummaryId, "IX_AthleteSummary_AthleteSummaryId")
                    .IsUnique();

                entity.Property(e => e.AthleteName).IsRequired();

                entity.Property(e => e.AvgPace).IsRequired();

                entity.Property(e => e.DistanceSumInKilometers)
                    .IsRequired()
                    .HasColumnType("NUMERIC");

                entity.Property(e => e.ElevationInMeters)
                    .IsRequired()
                    .HasColumnType("NUMERIC");

                entity.HasOne(d => d.MonthlySummary)
                    .WithMany(p => p.AthleteSummaries)
                    .HasForeignKey(d => d.MonthlySummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MonthlySummary>(entity =>
            {
                entity.HasIndex(e => e.MonthlySummaryId, "IX_MonthlySummaries_MonthlySummaryId")
                    .IsUnique();

                entity.Property(e => e.MonthlySummaryId).ValueGeneratedNever();

                entity.Property(e => e.MonthlySummaryCode).IsRequired();

                entity.Property(e => e.MonthlySummaryTrainingTypes).IsRequired();

                entity.Property(e => e.MontlhySummaryGenerationDate).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
