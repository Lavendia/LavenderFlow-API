using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ListItemsController : ControllerBase
{
    private readonly IListItemService _service;

    public ListItemsController(IListItemService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetListItems()
    {
        return Ok(await _service.GetListItemsAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetListItem(int id)
    {
        var listItem = await _service.GetListItemAsync(id);
        return listItem is null ? NotFound() : Ok(listItem);
    }

    [Authorize]
    [HttpGet("board/{id}")]
    public async Task<IActionResult> GetListItemsByBoard(int id)
    {
        var listItems = await _service.GetListItemsByBoardAsync(id);
        return Ok(listItems);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateListItem([FromBody] CreateListItemRequest request)
    {
        var listItem = await _service.CreateListItemAsync(request);
        return listItem is null
            ? NotFound("Board does not exist with id " + request.BoardId)
            : CreatedAtAction(nameof(GetListItem), new { id = listItem.Id }, listItem);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateListItem(int id, [FromBody] UpdateListItemRequest request)
    {
        var listItem = await _service.UpdateListItemAsync(id, request);
        return listItem is null ? NotFound() : Ok(listItem);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteListItem(int id)
    {
        return await _service.DeleteListItemAsync(id)
            ? NoContent()
            : NotFound();
    }
}