using Microsoft.EntityFrameworkCore;

public class WorkspaceUserRepository : IWorkspaceUserRepository
{
    private readonly AppDbContext _context;

    public WorkspaceUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkspaceUser>> GetByWorkspaceIdAsync(int workspaceId)
    {
        return await _context.WorkspaceUsers
            .Where(wu => wu.WorkspaceId == workspaceId)
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .ToListAsync();
    }

    public async Task<WorkspaceUser?> GetByIdAsync(int id)
    {
        return await _context.WorkspaceUsers
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .FirstOrDefaultAsync(wu => wu.Id == id);
    }

    public void Add(WorkspaceUser workspaceUser)
    {
        _context.WorkspaceUsers.Add(workspaceUser);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
