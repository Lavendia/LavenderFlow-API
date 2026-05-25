using Microsoft.EntityFrameworkCore;

public class ListItemRepository : IListItemRepository
{
    private readonly AppDbContext _context;

    public ListItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ListItem>> GetAllAsync()
    {
        return await _context.ListItems.ToListAsync();
    }

    public async Task<ListItem?> GetByIdAsync(int id)
    {
        return await _context.ListItems.FindAsync(id);
    }

    public void Add(ListItem listItem)
    {
        _context.ListItems.Add(listItem);
    }

    public void Delete(ListItem listItem)
    {
        _context.ListItems.Remove(listItem);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<ListItem>> GetByBoardIdAsync(int boardId)
    {
        return await _context.ListItems.Where(li => li.BoardId == boardId).ToListAsync();
    }
}
