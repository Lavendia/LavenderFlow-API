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
    [HttpGet]
    public async Task<IActionResult> GetWorkspaceUsers(int workspaceId)
    {
        var workspaceUsers = await _service.GetWorkspaceUsersAsync(workspaceId);
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceUser(int id)
    {
        var workspaceUser = await _service.GetWorkspaceUserAsync(id);
        return workspaceUser is null ? NotFound("Workspace user not found.") : Ok(workspaceUser);
    }
}