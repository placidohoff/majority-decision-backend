namespace MajorityDecision.Api.Models;

public class Scorecard
{
    public int Id { get; set; }

    public int FightId { get; set; }
    public Fight Fight { get; set; } = null!;

    public bool IsOfficial { get; set; }

    /// <summary>Name of the judge when <see cref="IsOfficial"/> is true.</summary>
    public string? JudgeName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RoundScore> RoundScores { get; set; } = [];
}
