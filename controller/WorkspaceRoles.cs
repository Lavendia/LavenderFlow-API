using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class WorkspaceRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkspaceRolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkspaceRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkspaceRole([FromBody] CreateWorkspaceRolesRequest request)
    {
        var role = new WorkspacesRoles(request.Name);
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWorkspaceRoles), new { id = role.Id }, role);
    }
}