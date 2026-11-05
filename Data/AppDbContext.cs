using MajorityDecision.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MajorityDecision.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Fighter> Fighters => Set<Fighter>();
    public DbSet<Fight> Fights => Set<Fight>();
    public DbSet<Scorecard> Scorecards => Set<Scorecard>();
    public DbSet<RoundScore> RoundScores => Set<RoundScore>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fighter>(e =>
        {
            e.Property(f => f.FirstName).HasMaxLength(100);
            e.Property(f => f.MiddleName).HasMaxLength(100);
            e.Property(f => f.LastName).HasMaxLength(100);
            e.Property(f => f.Suffix).HasMaxLength(20);
            // Supports the fighter-name autocomplete.
            e.HasIndex(f => new { f.LastName, f.FirstName });
        });

        modelBuilder.Entity<Fight>(e =>
        {
            e.HasOne(f => f.FighterA)
                .WithMany(f => f.FightsAsFighterA)
                .HasForeignKey(f => f.FighterAId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(f => f.FighterB)
                .WithMany(f => f.FightsAsFighterB)
                .HasForeignKey(f => f.FighterBId)
                .OnDelete(DeleteBehavior.Restrict);

            e.Property(f => f.WeightClass).HasConversion<string>().HasMaxLength(30);
            e.Property(f => f.Venue).HasMaxLength(200);
            e.Property(f => f.Referee).HasMaxLength(200);
            e.Property(f => f.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

            e.HasIndex(f => new { f.FighterAId, f.FighterBId, f.MeetingNumber }).IsUnique();
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Fight_DifferentFighters", "[FighterAId] <> [FighterBId]");
                t.HasCheckConstraint("CK_Fight_ScheduledRounds", "[ScheduledRounds] BETWEEN 1 AND 15");
                t.HasCheckConstraint("CK_Fight_MeetingNumber", "[MeetingNumber] >= 1");
            });
        });

        modelBuilder.Entity<Scorecard>(e =>
        {
            e.Property(s => s.JudgeName).HasMaxLength(200);
            e.Property(s => s.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.Property(s => s.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<RoundScore>(e =>
        {
            e.HasIndex(r => new { r.ScorecardId, r.RoundNumber }).IsUnique();
            e.ToTable(t => t.HasCheckConstraint("CK_RoundScore_RoundNumber", "[RoundNumber] >= 1"));
        });
    }
}
