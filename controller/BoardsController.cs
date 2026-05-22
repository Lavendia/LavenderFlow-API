using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _service;

    public BoardsController(IBoardService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBoards()
    {
        return Ok(await _service.GetBoardsAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBoard(int id)
    {
        var board = await _service.GetBoardAsync(id);
        return board is null ? NotFound() : Ok(board);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        var board = await _service.CreateBoardAsync(request);
        return board is null
            ? NotFound("Workspace does not exist with id " + request.WorkspaceId)
            : CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequest request, int id)
    {
        var board = await _service.UpdateBoardAsync(id, request);
        return board is null ? NotFound() : Ok(board);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        return await _service.DeleteBoardAsync(id)
            ? NoContent()
            : NotFound();
    }
}