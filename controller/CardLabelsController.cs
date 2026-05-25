using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CardLabelsController : ControllerBase
{
    private readonly ICardLabelService _service;

    public CardLabelsController(ICardLabelService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCardLabels()
    {
        return Ok(await _service.GetCardLabelsAsync());
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetLabelsByCardId(int cardId)
    {
        try
        {
            var labels = await _service.GetLabelsByCardIdAsync(cardId);
            return Ok(labels);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("label/{labelId}")]
    public async Task<IActionResult> GetCardsByLabelId(int labelId)
    {
        try
        {
            var cardLabels = await _service.GetCardsByLabelIdAsync(labelId);
            return Ok(cardLabels);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddLabelToCard([FromBody] CreateCardLabelRequest request)
    {
        try
        {
            var cardLabel = await _service.AddLabelToCardAsync(request);
            return cardLabel is null
                ? BadRequest("Failed to add label to card")
                : CreatedAtAction(nameof(GetCardLabels), cardLabel);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("card/{cardId}/label/{labelId}")]
    public async Task<IActionResult> RemoveLabelFromCard(int cardId, int labelId)
    {
        return await _service.RemoveLabelFromCardAsync(cardId, labelId)
            ? NoContent()
            : NotFound();
    }
}
