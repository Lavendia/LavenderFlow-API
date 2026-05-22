using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class WorkspaceRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkspaceRolesController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWorkspaceRoles()
    {
        var roles = await _context.WorkspaceRoles.ToListAsync();
        return Ok(roles);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspaceRole([FromBody] CreateWorkspaceRolesRequest request)
    {
        var role = new WorkspaceRole(request.Name);
        _context.WorkspaceRoles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWorkspaceRoles), new { id = role.Id }, role);
    }
}