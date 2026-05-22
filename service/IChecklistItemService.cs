public interface IChecklistItemService
{
    Task<ChecklistItemResponse?> GetChecklistItemAsync(int id);
    Task<ChecklistItemResponse?> CreateChecklistItemAsync(CreateChecklistItemRequest request);
    Task<ChecklistItemResponse?> UpdateChecklistItemAsync(int id, UpdateChecklistItemRequest request);
    Task<bool> DeleteChecklistItemAsync(int id);
}
