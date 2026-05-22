using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ListItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ListItemsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetListItems()
    {
        return Ok(await _context.ListItems.ToListAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetListItem(int id)
    {
        var listItem = await _context.ListItems.FindAsync(id);
        return listItem is null ? NotFound() : Ok(listItem);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateListItem([FromBody] CreateListItemRequest request)
    {
        if (await _context.Boards.FindAsync(request.BoardId) is null)
        {
            return NotFound("Board does not exist with id " + request.BoardId);
        }
        var listItem = new ListItem(request.Name, request.Order, request.BoardId);
        _context.ListItems.Add(listItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetListItem), new { id = listItem.Id }, listItem);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateListItem(int id, [FromBody] UpdateListItemRequest request)
    {
        var listItem = await _context.ListItems.FindAsync(id);
        if (listItem is null) return NotFound();

        if (request.Name is not null) listItem.Name = request.Name;
        if (request.Order is not null) listItem.Order = request.Order.Value;

        await _context.SaveChangesAsync();
        return Ok(listItem);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListItem(int id)
    {
        var listItem = await _context.ListItems.FindAsync(id);
        if (listItem is null) return NotFound();
        _context.ListItems.Remove(listItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}