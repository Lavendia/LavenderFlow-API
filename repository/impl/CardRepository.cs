using Microsoft.EntityFrameworkCore;

public class CardRepository : ICardRepository
{
    private readonly AppDbContext _context;

    public CardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> GetAllAsync()
    {
        return await _context.Cards.ToListAsync();
    }

    public async Task<Card?> GetByIdAsync(int id)
    {
        return await _context.Cards.FindAsync(id);
    }

    public void Add(Card card)
    {
        _context.Cards.Add(card);
    }

    public void Delete(Card card)
    {
        _context.Cards.Remove(card);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
