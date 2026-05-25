using Microsoft.EntityFrameworkCore;

public class CardAssignmentRepository : ICardAssignmentRepository
{
    private readonly AppDbContext _context;

    public CardAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CardAssignment>> GetByCardIdAsync(int cardId)
    {
        return await _context.CardAssignments
            .Where(ca => ca.CardId == cardId)
            .Include(ca => ca.User)
            .ToListAsync();
    }

    public async Task<CardAssignment?> GetByIdsAsync(int userId, int cardId)
    {
        return await _context.CardAssignments.FindAsync(userId, cardId);
    }

    public void Add(CardAssignment assignment)
    {
        _context.CardAssignments.Add(assignment);
    }

    public void Delete(CardAssignment assignment)
    {
        _context.CardAssignments.Remove(assignment);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
