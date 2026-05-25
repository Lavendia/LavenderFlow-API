using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly IWorkspaceService _service;

    public WorkspacesController(IWorkspaceService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWorkspaces()
    {
        return Ok(await _service.GetWorkspacesAsync());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspace(int id)
    {
        var workspace = await _service.GetWorkspaceAsync(id);
        return workspace is null ? NotFound() : Ok(workspace);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        var workspace = await _service.CreateWorkspaceAsync(request);
        return CreatedAtAction(nameof(GetWorkspace), new { id = workspace.Id }, workspace);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkspace([FromBody] UpdateWorkspaceRequest request, int id)
    {
        var updated = await _service.UpdateWorkspaceAsync(id, request);
        return updated ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkspace(int id)
    {
        var deleted = await _service.DeleteWorkspaceAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}