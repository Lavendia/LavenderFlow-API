using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class BoardUsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public BoardUsersController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetUsersByBoard(int boardId)
    {
        if (await _context.Boards.FindAsync(boardId) is null)
            return NotFound("Board does not exist with id " + boardId);

        var boardUsers = await _context.BoardUsers
            .Where(bu => bu.BoardId == boardId)
            .Include(bu => bu.User)
            .Include(bu => bu.BoardRole)
            .ToListAsync();

        return Ok(boardUsers);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBoardsByUser(int userId)
    {
        if (await _context.Users.FindAsync(userId) is null)
            return NotFound("User does not exist with id " + userId);

        var boardUsers = await _context.BoardUsers
            .Where(bu => bu.UserId == userId)
            .Include(bu => bu.Board)
            .Include(bu => bu.BoardRole)
            .ToListAsync();

        return Ok(boardUsers);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddUserToBoard([FromBody] CreateBoardUserRequest request)
    {
        if (await _context.Boards.FindAsync(request.BoardId) is null)
            return NotFound("Board does not exist with id " + request.BoardId);

        if (await _context.Users.FindAsync(request.UserId) is null)
            return NotFound("User does not exist with id " + request.UserId);

        if (await _context.BoardRoles.FindAsync(request.BoardRoleId) is null)
            return NotFound("BoardRole does not exist with id " + request.BoardRoleId);

        var exists = await _context.BoardUsers
            .AnyAsync(bu => bu.UserId == request.UserId && bu.BoardId == request.BoardId);
        if (exists) return Conflict("User is already a member of this board.");

        var boardUser = new BoardUser(request.UserId, request.BoardId, request.BoardRoleId);
        _context.BoardUsers.Add(boardUser);
        await _context.SaveChangesAsync();
        return Ok(boardUser);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBoardUser(int userId, int boardId, [FromBody] UpdateBoardUserRequest request)
    {
        var boardUser = await _context.BoardUsers.FindAsync(userId, boardId);
        if (boardUser is null) return NotFound();

        if (await _context.BoardRoles.FindAsync(request.BoardRoleId) is null)
            return NotFound("BoardRole does not exist with id " + request.BoardRoleId);

        boardUser.BoardRoleId = request.BoardRoleId;
        await _context.SaveChangesAsync();
        return Ok(boardUser);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveUserFromBoard(int userId, int boardId)
    {
        var boardUser = await _context.BoardUsers.FindAsync(userId, boardId);
        if (boardUser is null) return NotFound();
        _context.BoardUsers.Remove(boardUser);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}