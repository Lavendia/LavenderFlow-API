using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChecklistItemsController : ControllerBase
{
    private readonly IChecklistItemService _service;

    public ChecklistItemsController(IChecklistItemService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChecklistItem(int id)
    {
        var item = await _service.GetChecklistItemAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChecklistItem([FromBody] CreateChecklistItemRequest request)
    {
        var item = await _service.CreateChecklistItemAsync(request);
        return item is null
            ? NotFound("Checklist does not exist with id " + request.ChecklistId)
            : CreatedAtAction(nameof(GetChecklistItem), new { id = item.Id }, item);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChecklistItem(int id, [FromBody] UpdateChecklistItemRequest request)
    {
        var item = await _service.UpdateChecklistItemAsync(id, request);
        return item is null ? NotFound() : Ok(item);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChecklistItem(int id)
    {
        return await _service.DeleteChecklistItemAsync(id)
            ? NoContent()
            : NotFound();
    }
}