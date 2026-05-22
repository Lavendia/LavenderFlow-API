using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok((await _context.Users.ToListAsync()).Select(u => new UserResponse(u)));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _context.Users.FindAsync(userId);
        return user is null ? NotFound() : Ok(new UserResponse(user));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user is null ? NotFound() : Ok(new UserResponse(user));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();

        if (request.Name is not null) user.Name = request.Name;
        if (request.Email is not null) user.Email = request.Email;
        if (request.Password is not null) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict("Email already exists.");
        }

        return Ok(new UserResponse(user));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}