using Microsoft.AspNetCore.SignalR;

public class ChecklistService : IChecklistService
{
    private readonly IChecklistRepository _repository;
    private readonly ICardRepository _cardRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public ChecklistService(
        IChecklistRepository repository,
        ICardRepository cardRepository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _cardRepository = cardRepository;
        _listItemRepository = listItemRepository;
        _hub = hub;
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
        var card = await _cardRepository.GetByIdAsync(request.CardId);
        if (card is null)
            return null;

        var checklist = new Checklist(request.Name, request.CardId);
        _repository.Add(checklist);
        await _repository.SaveAsync();
        var response = new ChecklistResponse(checklist);

        var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
        if (listItem != null)
        {
            await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistCreated", response);
        }

        return response;
    }

    public async Task<ChecklistResponse?> UpdateChecklistAsync(int id, UpdateChecklistRequest request)
    {
        var checklist = await _repository.GetByIdAsync(id);
        if (checklist == null)
            return null;

        if (request.Name is not null) checklist.Name = request.Name;
        await _repository.SaveAsync();
        var response = new ChecklistResponse(checklist);

        var card = await _cardRepository.GetByIdAsync(checklist.CardId);
        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistUpdated", response);
            }
        }

        return response;
    }

    public async Task<bool> DeleteChecklistAsync(int id)
    {
        var checklist = await _repository.GetByIdAsync(id);
        if (checklist == null)
            return false;

        var card = await _cardRepository.GetByIdAsync(checklist.CardId);
        _repository.Delete(checklist);
        await _repository.SaveAsync();

        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChecklistDeleted", id);
            }
        }

        return true;
    }

    public async Task<IEnumerable<ChecklistResponse>> GetChecklistsByCardIdAsync(int cardId)
    {
        var checklists = await _repository.GetByCardIdAsync(cardId);
        return checklists.Select(c => new ChecklistResponse(c));
    }
}
