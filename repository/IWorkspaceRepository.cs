public interface IWorkspaceRepository
{
    Task<List<Workspace>> GetAllAsync();
    Task<Workspace?> GetByIdAsync(int id);
    void Add(Workspace workspace);
    void Delete(Workspace workspace);
    Task SaveAsync();
}
