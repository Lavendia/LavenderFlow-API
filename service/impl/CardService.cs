using Microsoft.AspNetCore.SignalR;

public class CardService : ICardService
{
    private readonly ICardRepository _repository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public CardService(
        ICardRepository repository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _listItemRepository = listItemRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<CardResponse>> GetCardsAsync()
    {
        var cards = await _repository.GetAllAsync();
        return cards.Select(c => new CardResponse(c));
    }

    public async Task<CardResponse?> GetCardAsync(int id)
    {
        var card = await _repository.GetByIdAsync(id);
        return card == null ? null : new CardResponse(card);
    }

    public async Task<CardResponse?> CreateCardAsync(CreateCardRequest request)
    {
        var listItem = await _listItemRepository.GetByIdAsync(request.ListItemId);

        if (listItem == null)
            return null;

        var card = new Card(request.Name, request.Order, request.Description, false, request.Deadline, request.ListItemId);
        _repository.Add(card);
        await _repository.SaveAsync();
        var response = new CardResponse(card);

        await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("CardCreated", response);

        return response;
    }

    public async Task<CardResponse?> UpdateCardAsync(int id, UpdateCardRequest request)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null)
            return null;

        ListItem? listItem = null;

        if (request.ListItemId is not null)
        {
            listItem = await _listItemRepository.GetByIdAsync(request.ListItemId.Value);
            if (listItem is null)
                throw new KeyNotFoundException("ListItem does not exist with id " + request.ListItemId.Value);
            card.ListItemId = request.ListItemId.Value;
        }

        if (request.Name is not null) card.Name = request.Name;
        if (request.Order is not null) card.Order = request.Order.Value;
        if (request.Description is not null) card.Description = request.Description;
        if (request.Archived is not null) card.Archived = request.Archived.Value;
        card.Deadline = request.Deadline;
        await _repository.SaveAsync();

        var response = new CardResponse(card);
        listItem ??= await _listItemRepository.GetByIdAsync(card.ListItemId);
        await _hub.Clients.Group(listItem!.BoardId.ToString()).SendAsync("CardUpdated", response);
        return response;
    }

    public async Task<bool> DeleteCardAsync(int id)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null)
            return false;

        var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);

        _repository.Delete(card);
        await _repository.SaveAsync();

        await _hub.Clients.Group(listItem!.BoardId.ToString()).SendAsync("CardDeleted", id);

        return true;
    }

    public async Task<IEnumerable<CardResponse>> GetCardsByListIdAsync(int listId)
    {
        var cards = await _repository.GetByListIdAsync(listId);
        return cards.Select(c => new CardResponse(c));
    }
}
