public interface IChecklistItemRepository
{
    Task<ChecklistItem?> GetByIdAsync(int id);
    Task<IEnumerable<ChecklistItem>> GetByChecklistIdAsync(int checklistId);
    void Add(ChecklistItem item);
    void Delete(ChecklistItem item);
    Task SaveAsync();
}
