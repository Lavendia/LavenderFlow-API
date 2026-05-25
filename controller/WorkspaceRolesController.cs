using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class WorkspaceRolesController : ControllerBase
{
    private readonly IWorkspaceRoleService _service;

    public WorkspaceRolesController(IWorkspaceRoleService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetWorkspaceRoles()
    {
        return Ok(await _service.GetWorkspaceRolesAsync());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWorkspaceRole([FromBody] CreateWorkspaceRolesRequest request)
    {
        var role = await _service.CreateWorkspaceRoleAsync(request);
        return CreatedAtAction(nameof(GetWorkspaceRoles), new { id = role.Id }, role);
    }
}