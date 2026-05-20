using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly AppDbContext _context;

    public WorkspacesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetWorkspaces()
    {
        return Ok(await _context.Workspaces.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspace(int id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        return workspace is null ? NotFound() : Ok(workspace);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        var workspace = new Workspace(request.Name, request.Description);
        _context.Workspaces.Add(workspace);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWorkspace), new { id = workspace.Id }, workspace);
    }

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