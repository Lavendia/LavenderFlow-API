using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BoardsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBoards()
    {
        return Ok((await _context.Boards.ToListAsync()).Select(b => new BoardResponse(b)));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBoard(int id)
    {
        var board = await _context.Boards.FindAsync(id);
        return board is null ? NotFound() : Ok(new BoardResponse(board));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] CreateBoardRequest request)
    {
        if (await _context.Workspaces.FindAsync(request.WorkspaceId) is null)
        {
            return NotFound("Worspace does not exist with id " + request.WorkspaceId);
        }
        var board = new Board(request.Name, request.Description, request.WorkspaceId);
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBoard), new { id = board.Id }, new BoardResponse(board));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequest request, int id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board is null) return NotFound();

        if (request.Name is not null) board.Name = request.Name;
        if (request.Description is not null) board.Description = request.Description;

        await _context.SaveChangesAsync();

        return Ok(new BoardResponse(board));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board is null) return NotFound();
        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}