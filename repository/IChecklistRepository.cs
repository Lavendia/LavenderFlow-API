public interface IChecklistRepository
{
    Task<List<Checklist>> GetAllWithItemsAsync();
    Task<Checklist?> GetByIdAsync(int id);
    Task<Checklist?> GetByIdWithItemsAsync(int id);
    Task<List<Checklist>> GetByCardIdAsync(int cardId);
    void Add(Checklist checklist);
    void Delete(Checklist checklist);
    Task SaveAsync();
}
