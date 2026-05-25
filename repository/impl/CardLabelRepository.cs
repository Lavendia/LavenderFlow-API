using Microsoft.EntityFrameworkCore;

public class CardLabelRepository : ICardLabelRepository
{
    private readonly AppDbContext _context;

    public CardLabelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CardLabel>> GetAllAsync()
    {
        return await _context.CardLabels.ToListAsync();
    }

    public async Task<CardLabel?> GetByIdsAsync(int cardId, int labelId)
    {
        return await _context.CardLabels
            .FirstOrDefaultAsync(cl => cl.CardId == cardId && cl.LabelId == labelId);
    }

    public async Task<List<CardLabel>> GetByCardIdAsync(int cardId)
    {
        return await _context.CardLabels.Where(cl => cl.CardId == cardId).ToListAsync();
    }

    public async Task<List<CardLabel>> GetByLabelIdAsync(int labelId)
    {
        return await _context.CardLabels.Where(cl => cl.LabelId == labelId).ToListAsync();
    }

    public async Task<bool> ExistsAsync(int cardId, int labelId)
    {
        return await _context.CardLabels.AnyAsync(cl => cl.CardId == cardId && cl.LabelId == labelId);
    }

    public void Add(CardLabel cardLabel)
    {
        _context.CardLabels.Add(cardLabel);
    }

    public void Delete(CardLabel cardLabel)
    {
        _context.CardLabels.Remove(cardLabel);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
