using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CardAssignmentsController : ControllerBase
{
    private readonly ICardAssignmentService _service;

    public CardAssignmentsController(ICardAssignmentService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("card/{cardId}")]
    public async Task<IActionResult> GetAssignmentsByCard(int cardId)
    {
        var assignments = await _service.GetAssignmentsByCardAsync(cardId);
        return assignments is null ? NotFound("Card does not exist with id " + cardId) : Ok(assignments);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateCardAssignmentRequest request)
    {
        var assignment = await _service.CreateAssignmentAsync(request);
        return Ok(assignment);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteAssignment(int userId, int cardId)
    {
        return await _service.DeleteAssignmentAsync(userId, cardId)
            ? NoContent()
            : NotFound();
    }
}