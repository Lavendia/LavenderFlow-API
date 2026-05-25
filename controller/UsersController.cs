using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _userService.GetUsersAsync());
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserAsync(userId);
        return user == null ? NotFound() : Ok(user);
    }

    [Authorize]
    [HttpGet("email")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required.");

        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            return NotFound($"No user found with email '{email}'.");

        return Ok(user);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserAsync(id);

        return user == null
            ? NotFound()
            : Ok(user);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        int id)
    {
        var user = await _userService.UpdateUserAsync(id, request);

        return user == null
            ? NotFound()
            : Ok(user);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _userService.DeleteUserAsync(id);

        return deleted
            ? NoContent()
            : NotFound();
    }
}