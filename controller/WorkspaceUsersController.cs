using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceUsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkspaceUsersController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWorkspaceUsers(int workspaceId)
    {
        var workspace = await _context.Workspaces.FindAsync(workspaceId);
        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        var workspaceUsers = await _context.WorkspaceUsers
            .Where(wu => wu.WorkspaceId == workspaceId)
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .ToListAsync();

        return Ok(workspaceUsers);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspaceUser(int workspaceId, [FromBody] CreateWorkspaceUsersRequest request)
    {
        var workspace = await _context.Workspaces.FindAsync(workspaceId);
        if (workspace == null)
        {
            return NotFound("Workspace not found.");
        }

        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var role = await _context.WorkspaceRoles.FindAsync(request.RoleId);
        if (role == null)
        {
            return NotFound("Role not found.");
        }

        var workspaceUser = new WorkspaceUser(
            userId: request.UserId,
            workspaceId: workspaceId,
            roleId: request.RoleId
        );

        _context.WorkspaceUsers.Add(workspaceUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorkspaceUser), new { id = workspaceUser.Id }, workspaceUser);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceUser(int id)
    {
        var workspaceUser = await _context.WorkspaceUsers
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .FirstOrDefaultAsync(wu => wu.Id == id);

        if (workspaceUser == null)
        {
            return NotFound("Workspace user not found.");
        }

        return Ok(workspaceUser);
    }
}