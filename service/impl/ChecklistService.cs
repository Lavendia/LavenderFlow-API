public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _repository;
    private readonly ICardRepository _cardRepository;

    public ChecklistService(
        IChecklistRepository repository,
        ICardRepository cardRepository)
    {
        _repository = repository;
        _cardRepository = cardRepository;
    }

    public async Task<IEnumerable<ChecklistResponse>> GetChecklistsAsync()
    {
        var checklists = await _repository.GetAllWithItemsAsync();
        return checklists.Select(c => new ChecklistResponse(c));
    }

    public async Task<ChecklistResponse?> GetChecklistAsync(int id)
    {
        var checklist = await _repository.GetByIdWithItemsAsync(id);
        return checklist == null ? null : new ChecklistResponse(checklist);
    }

    public async Task<ChecklistResponse?> CreateChecklistAsync(CreateChecklistRequest request)
    {
        if (await _cardRepository.GetByIdAsync(request.CardId) is null)
            return null;

        var checklist = new Checklist(request.Name, request.CardId);
        _repository.Add(checklist);
        await _repository.SaveAsync();
        return new ChecklistResponse(checklist);
    }

    public async Task<ChecklistResponse?> UpdateChecklistAsync(int id, UpdateChecklistRequest request)
    {
        var checklist = await _repository.GetByIdAsync(id);
        if (checklist == null)
            return null;

        if (request.Name is not null) checklist.Name = request.Name;
        await _repository.SaveAsync();
        return new ChecklistResponse(checklist);
    }

    public async Task<bool> DeleteChecklistAsync(int id)
    {
        var checklist = await _repository.GetByIdAsync(id);
        if (checklist == null)
            return false;

        _repository.Delete(checklist);
        await _repository.SaveAsync();
        return true;
    }
}
