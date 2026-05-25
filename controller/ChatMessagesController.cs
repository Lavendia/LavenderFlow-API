using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController : ControllerBase
{
    private readonly IChatMessageService _service;

    public ChatMessagesController(IChatMessageService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetChatMessages()
    {
        return Ok(await _service.GetChatMessagesAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatMessage(int id)
    {
        var message = await _service.GetChatMessageAsync(id);
        return message is null ? NotFound() : Ok(message);
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetChatMessagesByCardId(int cardId)
    {
        var messages = await _service.GetChatMessagesByCardIdAsync(cardId);
        return Ok(messages);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChatMessage([FromBody] CreateChatMessageRequest request)
    {
        var message = await _service.CreateChatMessageAsync(request);
        return message is null
            ? NotFound("Card does not exist with id " + request.CardId)
            : CreatedAtAction(nameof(GetChatMessage), new { id = message.Id }, message);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChatMessage(int id, [FromBody] UpdateChatMessageRequest request)
    {
        var message = await _service.UpdateChatMessageAsync(id, request);
        return message is null ? NotFound() : Ok(message);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChatMessage(int id)
    {
        return await _service.DeleteChatMessageAsync(id)
            ? NoContent()
            : NotFound();
    }
}
