using Microsoft.EntityFrameworkCore;

public class BoardRoleRepository : IBoardRoleRepository
{
    private readonly AppDbContext _context;

    public BoardRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BoardRole>> GetAllAsync()
    {
        return await _context.BoardRoles.ToListAsync();
    }

    public async Task<BoardRole?> GetByIdAsync(int id)
    {
        return await _context.BoardRoles.FindAsync(id);
    }

    public void Add(BoardRole role)
    {
        _context.BoardRoles.Add(role);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
