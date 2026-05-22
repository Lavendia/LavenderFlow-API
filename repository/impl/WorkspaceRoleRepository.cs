using Microsoft.EntityFrameworkCore;

public class WorkspaceRoleRepository : IWorkspaceRoleRepository
{
    private readonly AppDbContext _context;

    public WorkspaceRoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkspaceRole>> GetAllAsync()
    {
        return await _context.WorkspaceRoles.ToListAsync();
    }

    public async Task<WorkspaceRole?> GetByIdAsync(int id)
    {
        return await _context.WorkspaceRoles.FindAsync(id);
    }

    public void Add(WorkspaceRole role)
    {
        _context.WorkspaceRoles.Add(role);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
