using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BoardRolesController : ControllerBase
{
    private readonly IBoardRoleService _service;

    public BoardRolesController(IBoardRoleService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBoardRoles()
    {
        return Ok(await _service.GetBoardRolesAsync());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateBoardRole([FromBody] CreateBoardRolesRequest request)
    {
        var role = await _service.CreateBoardRoleAsync(request);
        return CreatedAtAction(nameof(GetBoardRoles), new { id = role.Id }, role);
    }
}