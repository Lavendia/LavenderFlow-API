using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkspacesController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWorkspaces()
    {
        return Ok((await _context.Workspaces.ToListAsync()).Select(w => new WorkspaceResponse(w)));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspace(int id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        return workspace is null ? NotFound() : Ok(new WorkspaceResponse(workspace));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        var workspace = new Workspace(request.Name, request.Description);
        _context.Workspaces.Add(workspace);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWorkspace), new { id = workspace.Id }, new WorkspaceResponse(workspace));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkspace([FromBody] UpdateWorkspaceRequest request, int id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        if (workspace is null) return NotFound();

        if (request.Name is not null) workspace.Name = request.Name;
        if (request.Description is not null) workspace.Description = request.Description;
        if (request.Public.HasValue) workspace.Public = request.Public.Value;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkspace(int id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        if (workspace is null) return NotFound();

        _context.Workspaces.Remove(workspace);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}