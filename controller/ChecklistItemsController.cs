using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ChecklistItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChecklistItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChecklistItem(int id)
    {
        var item = await _context.ChecklistItems.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChecklistItem([FromBody] CreateChecklistItemRequest request)
    {
        if (await _context.Checklists.FindAsync(request.ChecklistId) is null)
            return NotFound("Checklist does not exist with id " + request.ChecklistId);

        var item = new ChecklistItem(request.Name, false, request.ChecklistId);
        _context.ChecklistItems.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetChecklistItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklistItem(int id, [FromBody] UpdateChecklistItemRequest request)
    {
        var item = await _context.ChecklistItems.FindAsync(id);
        if (item is null) return NotFound();

        if (request.Name is not null) item.Name = request.Name;
        if (request.Finished is not null) item.Finished = request.Finished.Value;

        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklistItem(int id)
    {
        var item = await _context.ChecklistItems.FindAsync(id);
        if (item is null) return NotFound();
        _context.ChecklistItems.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}