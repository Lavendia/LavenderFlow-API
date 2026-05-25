using Microsoft.EntityFrameworkCore;

public class LabelRepository : ILabelRepository
{
    private readonly AppDbContext _context;

    public LabelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Label>> GetAllAsync()
    {
        return await _context.Labels.ToListAsync();
    }

    public async Task<Label?> GetByIdAsync(int id)
    {
        return await _context.Labels.FindAsync(id);
    }

    public void Add(Label label)
    {
        _context.Labels.Add(label);
    }

    public void Delete(Label label)
    {
        _context.Labels.Remove(label);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
