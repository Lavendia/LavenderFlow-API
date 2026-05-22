using Microsoft.EntityFrameworkCore;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Workspace>> GetAllAsync()
    {
        return await _context.Workspaces.ToListAsync();
    }

    public async Task<Workspace?> GetByIdAsync(int id)
    {
        return await _context.Workspaces.FindAsync(id);
    }

    public void Add(Workspace workspace)
    {
        _context.Workspaces.Add(workspace);
    }

    public void Delete(Workspace workspace)
    {
        _context.Workspaces.Remove(workspace);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
