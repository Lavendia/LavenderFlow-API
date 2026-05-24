public interface IChecklistService
{
    Task<IEnumerable<ChecklistResponse>> GetChecklistsAsync();
    Task<ChecklistResponse?> GetChecklistAsync(int id);
    Task<IEnumerable<ChecklistResponse>> GetChecklistsByCardIdAsync(int cardId);
    Task<ChecklistResponse?> CreateChecklistAsync(CreateChecklistRequest request);
    Task<ChecklistResponse?> UpdateChecklistAsync(int id, UpdateChecklistRequest request);
    Task<bool> DeleteChecklistAsync(int id);
}
