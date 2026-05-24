public class CardService : ICardService
{
    private readonly ICardRepository _repository;
    private readonly IListItemRepository _listItemRepository;

    public CardService(
        ICardRepository repository,
        IListItemRepository listItemRepository)
    {
        _repository = repository;
        _listItemRepository = listItemRepository;
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
        if (await _listItemRepository.GetByIdAsync(request.ListItemId) is null)
            return null;

        var card = new Card(request.Name, request.Order, request.Description, false, request.Deadline, request.ListItemId);
        _repository.Add(card);
        await _repository.SaveAsync();
        return new CardResponse(card);
    }

    public async Task<CardResponse?> UpdateCardAsync(int id, UpdateCardRequest request)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null)
            return null;

        if (request.ListItemId is not null)
        {
            if (await _listItemRepository.GetByIdAsync(request.ListItemId.Value) is null)
                throw new KeyNotFoundException("ListItem does not exist with id " + request.ListItemId.Value);
            card.ListItemId = request.ListItemId.Value;
        }

        if (request.Name is not null) card.Name = request.Name;
        if (request.Order is not null) card.Order = request.Order.Value;
        if (request.Description is not null) card.Description = request.Description;
        if (request.Archived is not null) card.Archived = request.Archived.Value;
        if (request.Deadline is not null) card.Deadline = request.Deadline.Value;

        await _repository.SaveAsync();
        return new CardResponse(card);
    }

    public async Task<bool> DeleteCardAsync(int id)
    {
        var card = await _repository.GetByIdAsync(id);
        if (card == null)
            return false;

        _repository.Delete(card);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<IEnumerable<CardResponse>> GetCardsByListIdAsync(int listId)
    {
        var cards = await _repository.GetByListIdAsync(listId);
        return cards.Select(c => new CardResponse(c));
    }
}
