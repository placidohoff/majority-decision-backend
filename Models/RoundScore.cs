namespace MajorityDecision.Api.Models;

public class RoundScore
{
    public int Id { get; set; }

    public int ScorecardId { get; set; }
    public Scorecard Scorecard { get; set; } = null!;

    public int RoundNumber { get; set; }

    public int FighterAScore { get; set; }
    public int FighterBScore { get; set; }

    public int FighterAKnockdowns { get; set; }
    public int FighterBKnockdowns { get; set; }

    public int FighterADeductions { get; set; }
    public int FighterBDeductions { get; set; }
}
