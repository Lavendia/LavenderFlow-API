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

    public async Task<WorkspaceUser?> GetByIdAsync(int workspaceUserId)
    {
        return await _context.WorkspaceUsers
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .FirstOrDefaultAsync(wu => wu.Id == workspaceUserId);
    }

    public void Add(WorkspaceUser workspaceUser)
    {
        _context.WorkspaceUsers.Add(workspaceUser);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<WorkspaceUser>> GetByUserIdAsync(int userId)
    {
        return await _context.WorkspaceUsers
            .Where(wu => wu.UserId == userId)
            .Include(wu => wu.User)
            .Include(wu => wu.Role)
            .Include(wu => wu.Workspace)
            .ToListAsync();
    }

    public async Task<bool> UserBelongsToWorkspaceAsync(int userId, int workspaceId)
    {
        return await _context.WorkspaceUsers.AnyAsync(wu =>
            wu.UserId == userId && wu.WorkspaceId == workspaceId);
    }

    public async Task<bool> ExistsAsync(int workspaceUserId)
    {
        return await _context.WorkspaceUsers.AnyAsync(wu => wu.Id == workspaceUserId);
    }

    public void Delete(WorkspaceUser workspaceUser)
    {
        _context.WorkspaceUsers.Remove(workspaceUser);
    }
}
