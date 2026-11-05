namespace MajorityDecision.Api.Models;

public class Fighter
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string? Suffix { get; set; }

    public ICollection<Fight> FightsAsFighterA { get; set; } = [];
    public ICollection<Fight> FightsAsFighterB { get; set; } = [];
}
