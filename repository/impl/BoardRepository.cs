using Microsoft.EntityFrameworkCore;

public class BoardRepository : IBoardRepository
{
    private readonly AppDbContext _context;

    public BoardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Board>> GetAllAsync()
    {
        return await _context.Boards.ToListAsync();
    }

    public async Task<Board?> GetByIdAsync(int id)
    {
        return await _context.Boards.FindAsync(id);
    }

    public void Add(Board board)
    {
        _context.Boards.Add(board);
    }

    public void Delete(Board board)
    {
        _context.Boards.Remove(board);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
