using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChecklistsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetChecklists()
    {
        return Ok(await _context.Checklists.Include(c => c.Items).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChecklist(int id)
    {
        var checklist = await _context.Checklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);
        return checklist is null ? NotFound() : Ok(checklist);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistRequest request)
    {
        if (await _context.Cards.FindAsync(request.CardId) is null)
            return NotFound("Card does not exist with id " + request.CardId);

        var checklist = new Checklist(request.Name, request.CardId);
        _context.Checklists.Add(checklist);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChecklist), new { id = checklist.Id }, checklist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklist(int id, [FromBody] UpdateChecklistRequest request)
    {
        var checklist = await _context.Checklists.FindAsync(id);
        if (checklist is null) return NotFound();

        if (request.Name is not null) checklist.Name = request.Name;

        await _context.SaveChangesAsync();
        return Ok(checklist);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklist(int id)
    {
        var checklist = await _context.Checklists.FindAsync(id);
        if (checklist is null) return NotFound();
        _context.Checklists.Remove(checklist);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}