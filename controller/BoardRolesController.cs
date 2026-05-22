using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class BoardRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public BoardRolesController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBoardRoles()
    {
        var roles = await _context.BoardRoles.ToListAsync();
        return Ok(roles.Select(r => new BoardRoleResponse(r)));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBoardRole([FromBody] CreateBoardRolesRequest request)
    {
        var role = new BoardRole(request.Name);
        _context.BoardRoles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBoardRoles), new { id = role.Id }, new BoardRoleResponse(role));
    }
}