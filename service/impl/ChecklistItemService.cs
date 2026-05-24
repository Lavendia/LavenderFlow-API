public class ChecklistItemService : IChecklistItemService
{
    private readonly IChecklistItemRepository _repository;
    private readonly IChecklistRepository _checklistRepository;

    public ChecklistItemService(
        IChecklistItemRepository repository,
        IChecklistRepository checklistRepository)
    {
        _repository = repository;
        _checklistRepository = checklistRepository;
    }

    public async Task<ChecklistItemResponse?> GetChecklistItemAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item == null ? null : new ChecklistItemResponse(item);
    }

    public async Task<ChecklistItemResponse?> CreateChecklistItemAsync(CreateChecklistItemRequest request)
    {
        if (await _checklistRepository.GetByIdAsync(request.ChecklistId) is null)
            return null;

        var item = new ChecklistItem(request.Name, false, request.ChecklistId);
        _repository.Add(item);
        await _repository.SaveAsync();
        return new ChecklistItemResponse(item);
    }

    public async Task<ChecklistItemResponse?> UpdateChecklistItemAsync(int id, UpdateChecklistItemRequest request)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return null;

        if (request.Name is not null) item.Name = request.Name;
        if (request.Finished is not null) item.Finished = request.Finished.Value;

        await _repository.SaveAsync();
        return new ChecklistItemResponse(item);
    }

    public async Task<bool> DeleteChecklistItemAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return false;

        _repository.Delete(item);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<ChecklistItemResponse>> GetChecklistItemsByChecklistIdAsync(int checklistId)
    {
        var items = await _repository.GetByChecklistIdAsync(checklistId);
        return items.Select(item => new ChecklistItemResponse(item));
    }
}
