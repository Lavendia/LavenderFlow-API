public interface IChecklistItemRepository
{
    Task<ChecklistItem?> GetByIdAsync(int id);
    void Add(ChecklistItem item);
    void Delete(ChecklistItem item);
    Task SaveAsync();
}
