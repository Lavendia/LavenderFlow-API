using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BoardUsersController : ControllerBase
{
    private readonly IBoardUserService _service;

    public BoardUsersController(IBoardUserService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetUsersByBoard(int boardId)
    {
        return Ok(await _service.GetUsersByBoardAsync(boardId));
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBoardsByUser(int userId)
    {
        return Ok(await _service.GetBoardsByUserAsync(userId));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddUserToBoard([FromBody] CreateBoardUserRequest request)
    {
        var boardUser = await _service.AddUserToBoardAsync(request);
        return Ok(boardUser);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBoardUser(int userId, int boardId, [FromBody] UpdateBoardUserRequest request)
    {
        var boardUser = await _service.UpdateBoardUserAsync(userId, boardId, request);
        return boardUser is null ? NotFound() : Ok(boardUser);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserFromBoard(int userId, int boardId)
    {
        return await _service.RemoveUserFromBoardAsync(userId, boardId)
            ? NoContent()
            : NotFound();
    }
}