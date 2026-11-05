namespace MajorityDecision.Api.Models;

public class Fight
{
    public int Id { get; set; }

    public int FighterAId { get; set; }
    public Fighter FighterA { get; set; } = null!;

    public int FighterBId { get; set; }
    public Fighter FighterB { get; set; } = null!;

    public int ScheduledRounds { get; set; }
    public WeightClass? WeightClass { get; set; }

    /// <summary>1 for the first meeting between these fighters, 2 for a rematch, etc.</summary>
    public int MeetingNumber { get; set; } = 1;

    // Optional metadata (can be added after the fight is created).
    public DateOnly? Date { get; set; }
    public string? Venue { get; set; }
    public string? Referee { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Scorecard> Scorecards { get; set; } = [];
}
