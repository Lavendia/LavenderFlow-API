using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;

    public AuthService(IAuthRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        var existingUser =
            await _repository.GetByEmailAsync(request.Email);

        if (existingUser != null)
            throw new Exception("Email already exists.");

        var user = new User(
            request.Name,
            request.Email,
            request.Password);

        await _repository.CreateUserAsync(user);

        return GenerateToken(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user =
            await _repository.GetByEmailAsync(request.Email);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Invalid email or password.");
        }

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Name),

            new Claim(
                ClaimTypes.Email,
                user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                Environment.GetEnvironmentVariable("JWT_SECRET")!));

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:
                Environment.GetEnvironmentVariable("JWT_ISSUER"),

            audience:
                Environment.GetEnvironmentVariable("JWT_AUDIENCE"),

            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}