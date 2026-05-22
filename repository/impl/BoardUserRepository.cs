using Microsoft.EntityFrameworkCore;

public class BoardUserRepository : IBoardUserRepository
{
    private readonly AppDbContext _context;

    public BoardUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BoardUser>> GetByBoardIdAsync(int boardId)
    {
        return await _context.BoardUsers
            .Where(bu => bu.BoardId == boardId)
            .Include(bu => bu.User)
            .Include(bu => bu.BoardRole)
            .ToListAsync();
    }

    public async Task<List<BoardUser>> GetByUserIdAsync(int userId)
    {
        return await _context.BoardUsers
            .Where(bu => bu.UserId == userId)
            .Include(bu => bu.Board)
            .Include(bu => bu.BoardRole)
            .ToListAsync();
    }

    public async Task<BoardUser?> GetByIdsAsync(int userId, int boardId)
    {
        return await _context.BoardUsers.FindAsync(userId, boardId);
    }

    public async Task<bool> ExistsAsync(int userId, int boardId)
    {
        return await _context.BoardUsers
            .AnyAsync(bu => bu.UserId == userId && bu.BoardId == boardId);
    }

    public void Add(BoardUser boardUser)
    {
        _context.BoardUsers.Add(boardUser);
    }

    public void Delete(BoardUser boardUser)
    {
        _context.BoardUsers.Remove(boardUser);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
