public interface IChecklistRepository
{
    Task<List<Checklist>> GetAllWithItemsAsync();
    Task<Checklist?> GetByIdAsync(int id);
    Task<Checklist?> GetByIdWithItemsAsync(int id);
    void Add(Checklist checklist);
    void Delete(Checklist checklist);
    Task SaveAsync();
}
