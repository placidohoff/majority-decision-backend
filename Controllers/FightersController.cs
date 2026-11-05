using System.ComponentModel.DataAnnotations;
using MajorityDecision.Api.Data;
using MajorityDecision.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MajorityDecision.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FightersController(AppDbContext db) : ControllerBase
{
    /// <summary>Search fighters by name (for autocomplete).</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FighterDto>>> Search([FromQuery] string? q, [FromQuery] int limit = 10)
    {
        limit = Math.Clamp(limit, 1, 50);
        var query = db.Fighters.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(f =>
                f.FirstName.StartsWith(term) ||
                f.LastName.StartsWith(term) ||
                (f.FirstName + " " + f.LastName).StartsWith(term));
        }

        return await query
            .OrderBy(f => f.LastName).ThenBy(f => f.FirstName)
            .Take(limit)
            .Select(f => new FighterDto(f.Id, f.FirstName, f.LastName, f.Suffix))
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FighterDto>> Get(int id)
    {
        var f = await db.Fighters.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        return f is null ? NotFound() : new FighterDto(f.Id, f.FirstName, f.LastName, f.Suffix);
    }

    [HttpPost]
    public async Task<ActionResult<FighterDto>> Create(CreateFighterRequest request)
    {
        var fighter = new Fighter
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Suffix = string.IsNullOrWhiteSpace(request.Suffix) ? null : request.Suffix.Trim(),
        };
        db.Fighters.Add(fighter);
        await db.SaveChangesAsync();

        var dto = new FighterDto(fighter.Id, fighter.FirstName, fighter.LastName, fighter.Suffix);
        return CreatedAtAction(nameof(Get), new { id = fighter.Id }, dto);
    }
}

public record FighterDto(int Id, string FirstName, string LastName, string? Suffix);

public record CreateFighterRequest(
    [Required, MaxLength(100)] string FirstName,
    [Required, MaxLength(100)] string LastName,
    [MaxLength(20)] string? Suffix);
