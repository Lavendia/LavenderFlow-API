using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        return Ok(await _service.GetWorkspacesForUserAsync(userId));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspace(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var workspace = await _service.GetWorkspaceForUserAsync(id, userId);
        return workspace is null ? NotFound() : Ok(workspace);
    }

    [Authorize]
    [HttpGet("{id}/boards")]
    public async Task<IActionResult> GetBoardsByWorkspace(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var boards = await _service.GetBoardsByWorkspaceForUserAsync(id, userId);
        return boards is null ? NotFound() : Ok(boards);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var workspace = await _service.CreateWorkspaceAsync(request, userId);
        return CreatedAtAction(nameof(GetWorkspace), new { id = workspace.Id }, workspace);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkspace([FromBody] UpdateWorkspaceRequest request, int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var updated = await _service.UpdateWorkspaceAsync(id, request, userId);
        return updated ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkspace(int id)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();

        var deleted = await _service.DeleteWorkspaceAsync(id, userId);
        return deleted ? NoContent() : NotFound();
    }

    private bool TryGetUserId(out int userId)
    {
        userId = 0;
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return !string.IsNullOrEmpty(userIdValue) && int.TryParse(userIdValue, out userId);
    }
}
