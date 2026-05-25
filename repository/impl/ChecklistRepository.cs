using Microsoft.EntityFrameworkCore;

public class ChecklistRepository : IChecklistRepository
{
    private readonly AppDbContext _context;

    public ChecklistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Checklist>> GetAllWithItemsAsync()
    {
        return await _context.Checklists
            .Include(c => c.Items)
            .ToListAsync();
    }

    public async Task<Checklist?> GetByIdAsync(int id)
    {
        return await _context.Checklists.FindAsync(id);
    }

    public async Task<Checklist?> GetByIdWithItemsAsync(int id)
    {
        return await _context.Checklists
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public void Add(Checklist checklist)
    {
        _context.Checklists.Add(checklist);
    }

    public void Delete(Checklist checklist)
    {
        _context.Checklists.Remove(checklist);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<Checklist>> GetByCardIdAsync(int cardId)
    {
        return await _context.Checklists.Where(c => c.CardId == cardId).ToListAsync();
    }
}
