using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ICardService _service;

    public CardsController(ICardService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCards()
    {
        return Ok(await _service.GetCardsAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCard(int id)
    {
        var card = await _service.GetCardAsync(id);
        return card is null ? NotFound() : Ok(card);
    }

    [Authorize]
    [HttpGet("list/{listId}")]
    public async Task<IActionResult> GetCardByListId(int listId)
    {
        var cards = await _service.GetCardsByListIdAsync(listId);
        return cards is null ? NotFound() : Ok(cards);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCard([FromBody] CreateCardRequest request)
    {
        var card = await _service.CreateCardAsync(request);
        return card is null
            ? NotFound("ListItem does not exist with id " + request.ListItemId)
            : CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCard(int id, [FromBody] UpdateCardRequest request)
    {
        var card = await _service.UpdateCardAsync(id, request);
        return card is null ? NotFound() : Ok(card);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        return await _service.DeleteCardAsync(id)
            ? NoContent()
            : NotFound();
    }
}