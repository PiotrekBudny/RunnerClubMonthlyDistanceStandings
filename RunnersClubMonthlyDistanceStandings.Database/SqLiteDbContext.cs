using Microsoft.EntityFrameworkCore;
using RunnersClubMonthlyDistanceStandings.Database.DbModels;

namespace RunnersClubMonthlyDistanceStandings.Database
{
    public class SqLiteDbContext : DbContext
    {
        private readonly string _databaseConnectionString;
        
        public SqLiteDbContext(string databaseConnectionString) : base()
        {
            _databaseConnectionString = databaseConnectionString;
        }
        
        public virtual DbSet<AthleteSummaryDbModel> AthleteSummaries { get; set; }
        public virtual DbSet<MonthlySummaryDbModel> MonthlySummaries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_databaseConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AthleteSummaryDbModel>(entity =>
            {
                entity.ToTable("AthleteSummary");

                entity.HasIndex(e => e.AthleteSummaryId, "IX_AthleteSummary_AthleteSummaryId")
                    .IsUnique();

                entity.HasKey(e => e.AthleteSummaryId);

                entity.Property(e => e.AthleteSummaryId).ValueGeneratedOnAdd();

                entity.Property(e => e.AthleteName).IsRequired();

                entity.Property(e => e.AvgPace).IsRequired();

                entity.Property(e => e.DistanceSumInKilometers)
                    .IsRequired()
                    .HasColumnType("REAL");

                entity.Property(e => e.ElevationInMeters)
                    .IsRequired()
                    .HasColumnType("REAL");

                entity.HasOne(d => d.MonthlySummary)
                    .WithMany(p => p.AthleteSummaries)
                    .HasForeignKey(d => d.MonthlySummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<MonthlySummaryDbModel>(entity =>
            {
                entity.HasIndex(e => e.MonthlySummaryId, "IX_MonthlySummaries_MonthlySummaryId")
                    .IsUnique();

                entity.Property(e => e.MonthlySummaryId).ValueGeneratedOnAdd();

                entity.HasKey(e => e.MonthlySummaryId);

                entity.Property(e => e.SummaryCode).IsRequired();

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.TrainingTypes).IsRequired();
            });
        }
    }
}
