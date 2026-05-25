using Microsoft.EntityFrameworkCore;

public class ChecklistItemRepository : IChecklistItemRepository
{
    private readonly AppDbContext _context;

    public ChecklistItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChecklistItem?> GetByIdAsync(int id)
    {
        return await _context.ChecklistItems.FindAsync(id);
    }

    public void Add(ChecklistItem item)
    {
        _context.ChecklistItems.Add(item);
    }

    public void Delete(ChecklistItem item)
    {
        _context.ChecklistItems.Remove(item);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ChecklistItem>> GetByChecklistIdAsync(int checklistId)
    {
        return await _context.ChecklistItems.Where(item => item.ChecklistId == checklistId).ToListAsync();
    }
}
