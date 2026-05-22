using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CardsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCards()
    {
        return Ok((await _context.Cards.ToListAsync()).Select(c => new CardResponse(c)));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCard(int id)
    {
        var card = await _context.Cards.FindAsync(id);
        return card is null ? NotFound() : Ok(new CardResponse(card));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        if (await _context.ListItems.FindAsync(request.ListItemId) is null)
        {
            return NotFound("ListItem does not exist with id " + request.ListItemId);
        }
        var card = new Card(request.Name, request.Order, request.Description, false, request.Deadline, request.ListItemId);
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCard), new { id = card.Id }, new CardResponse(card));
    }


    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCard(int id, [FromBody] UpdateCardRequest request)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card is null) return NotFound();

        if (request.ListItemId is not null)
        {
            if (await _context.ListItems.FindAsync(request.ListItemId.Value) is null)
                return NotFound("ListItem does not exist with id " + request.ListItemId);
            card.ListItemId = request.ListItemId.Value;
        }

        if (request.Name is not null) card.Name = request.Name;
        if (request.Order is not null) card.Order = request.Order.Value;
        if (request.Description is not null) card.Description = request.Description;
        if (request.Archived is not null) card.Archived = request.Archived.Value;
        if (request.Deadline is not null) card.Deadline = request.Deadline.Value;
        if (request.ListItemId is not null) card.ListItemId = request.ListItemId.Value;

        await _context.SaveChangesAsync();
        return Ok(new CardResponse(card));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var card = await _context.Cards.FindAsync(id);
        if (card is null) return NotFound();
        _context.Cards.Remove(card);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}