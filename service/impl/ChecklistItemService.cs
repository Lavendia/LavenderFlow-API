using Microsoft.AspNetCore.SignalR;

public class ChecklistItemService : IChecklistItemService
{
    private readonly IChecklistItemRepository _repository;
    private readonly IChecklistRepository _checklistRepository;
    private readonly ICardRepository _cardRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public ChecklistItemService(
        IChecklistItemRepository repository,
        IChecklistRepository checklistRepository,
        ICardRepository cardRepository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _checklistRepository = checklistRepository;
        _cardRepository = cardRepository;
        _listItemRepository = listItemRepository;
        _hub = hub;
    }

    public async Task<ChecklistItemResponse?> GetChecklistItemAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item == null ? null : new ChecklistItemResponse(item);
    }

    public async Task<ChecklistItemResponse?> CreateChecklistItemAsync(CreateChecklistItemRequest request)
    {
        var checklist = await _checklistRepository.GetByIdAsync(request.ChecklistId);
        if (checklist is null)
            return null;

        var item = new ChecklistItem(request.Name, false, request.ChecklistId);
        _repository.Add(item);
        await _repository.SaveAsync();
        var response = new ChecklistItemResponse(item);

        var card = await _cardRepository.GetByIdAsync(checklist.CardId);
        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistItemCreated", response);
            }
        }

        return response;
    }

    public async Task<ChecklistItemResponse?> UpdateChecklistItemAsync(int id, UpdateChecklistItemRequest request)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return null;

        if (request.Name is not null) item.Name = request.Name;
        if (request.Finished is not null) item.Finished = request.Finished.Value;

        await _repository.SaveAsync();
        var response = new ChecklistItemResponse(item);

        var checklist = await _checklistRepository.GetByIdAsync(item.ChecklistId);
        if (checklist != null)
        {
            var card = await _cardRepository.GetByIdAsync(checklist.CardId);
            if (card != null)
            {
                var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
                if (listItem != null)
                {
                    await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistItemUpdated", response);
                }
            }
        }

        return response;
    }

    public async Task<bool> DeleteChecklistItemAsync(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null)
            return false;

        var checklist = await _checklistRepository.GetByIdAsync(item.ChecklistId);
        _repository.Delete(item);
        await _repository.SaveAsync();

        if (checklist != null)
        {
            var card = await _cardRepository.GetByIdAsync(checklist.CardId);
            if (card != null)
            {
                var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
                if (listItem != null)
                {
                    await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistItemDeleted", id);
                }
            }
        }

        return true;
    }

    public async Task<IEnumerable<ChecklistItemResponse>> GetChecklistItemsByChecklistIdAsync(int checklistId)
    {
        var items = await _repository.GetByChecklistIdAsync(checklistId);
        return items.Select(item => new ChecklistItemResponse(item));
    }
}
