using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CardAssignmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CardAssignmentsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetAssignmentsByCard(int cardId)
    {
        if (await _context.Cards.FindAsync(cardId) is null)
            return NotFound("Card does not exist with id " + cardId);

        var assignments = await _context.CardAssignments
            .Where(ca => ca.CardId == cardId)
            .Include(ca => ca.User)
            .ToListAsync();

        return Ok(assignments);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateCardAssignmentRequest request)
    {
        if (await _context.Cards.FindAsync(request.CardId) is null)
            return NotFound("Card does not exist with id " + request.CardId);

        if (await _context.Users.FindAsync(request.UserId) is null)
            return NotFound("User does not exist with id " + request.UserId);

        var assignment = new CardAssignment(request.UserId, request.CardId);
        _context.CardAssignments.Add(assignment);
        await _context.SaveChangesAsync();
        return Ok(assignment);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteAssignment(int userId, int cardId)
    {
        var assignment = await _context.CardAssignments
            .FindAsync(userId, cardId);
        if (assignment is null) return NotFound();
        _context.CardAssignments.Remove(assignment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}