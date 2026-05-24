using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController : ControllerBase
{
    private readonly IChecklistService _service;

    public ChecklistsController(IChecklistService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetChecklists()
    {
        return Ok(await _service.GetChecklistsAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChecklist(int id)
    {
        var checklist = await _service.GetChecklistAsync(id);
        return checklist is null ? NotFound() : Ok(checklist);
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetChecklistsByCardId(int cardId)
    {
        var checklists = await _service.GetChecklistsByCardIdAsync(cardId);
        return checklists is null ? NotFound() : Ok(checklists);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChecklist([FromBody] CreateChecklistRequest request)
    {
        var checklist = await _service.CreateChecklistAsync(request);
        return checklist is null
            ? NotFound("Card does not exist with id " + request.CardId)
            : CreatedAtAction(nameof(GetChecklist), new { id = checklist.Id }, checklist);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklist(int id, [FromBody] UpdateChecklistRequest request)
    {
        var checklist = await _service.UpdateChecklistAsync(id, request);
        return checklist is null ? NotFound() : Ok(checklist);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklist(int id)
    {
        return await _service.DeleteChecklistAsync(id)
            ? NoContent()
            : NotFound();
    }
}