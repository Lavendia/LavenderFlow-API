using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceUsersController : ControllerBase
{
    private readonly IWorkspaceUserService _service;

    public WorkspaceUsersController(IWorkspaceUserService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceUser(int id)
    {
        var workspaceUser = await _service.GetWorkspaceUserAsync(id);
        return workspaceUser is null ? NotFound("Workspace user not found.") : Ok(workspaceUser);
    }

    [Authorize]
    [HttpGet("workspaces/{workspaceId}")]
    public async Task<IActionResult> GetUsersByWorkspace(int workspaceId)
    {
        var workspaceUsers = await _service.GetUsersByWorkspaceAsync(workspaceId);
        return workspaceUsers is null ? NotFound("Workspace not found.") : Ok(workspaceUsers);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspaceUser(int workspaceId, [FromBody] CreateWorkspaceUsersRequest request)
    {
        var workspaceUser = await _service.CreateWorkspaceUserAsync(workspaceId, request);
        return Ok(workspaceUser);
    }

    [Authorize]
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetWorkspacesByUser(int userId)
    {
        var workspaceUsers = await _service.GetWorkspacesByUserAsync(userId);
        return Ok(workspaceUsers);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWorkspaceUser(int id, [FromBody] UpdateWorkspaceUsersRequest request)
    {
        var updatedWorkspaceUser = await _service.UpdateWorkspaceUserAsync(id, request);
        return updatedWorkspaceUser is null ? NotFound("Workspace user not found.") : Ok(updatedWorkspaceUser);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUserFromWorkspace(int id)
    {
        var result = await _service.RemoveUserFromWorkspaceAsync(id);
        return result ? NoContent() : NotFound("Workspace user not found.");
    }
}