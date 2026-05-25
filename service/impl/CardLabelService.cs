using Microsoft.AspNetCore.SignalR;

public class CardLabelService : ICardLabelService
{
    private readonly ICardLabelRepository _repository;
    private readonly ICardRepository _cardRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public CardLabelService(
        ICardLabelRepository repository,
        ICardRepository cardRepository,
        ILabelRepository labelRepository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _cardRepository = cardRepository;
        _labelRepository = labelRepository;
        _listItemRepository = listItemRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<CardLabelResponse>> GetCardLabelsAsync()
    {
        var cardLabels = await _repository.GetAllAsync();
        return cardLabels.Select(cl => new CardLabelResponse(cl));
    }

    public async Task<IEnumerable<LabelResponse>> GetLabelsByCardIdAsync(int cardId)
    {
        if (await _cardRepository.GetByIdAsync(cardId) is null)
            throw new KeyNotFoundException("Card does not exist with id " + cardId);

        var cardLabels = await _repository.GetByCardIdAsync(cardId);
        var labelIds = cardLabels.Select(cl => cl.LabelId);
        var labels = new List<Label>();

        foreach (var labelId in labelIds)
        {
            var label = await _labelRepository.GetByIdAsync(labelId);
            if (label != null) labels.Add(label);
        }

        return labels.Select(l => new LabelResponse(l));
    }

    public async Task<IEnumerable<CardLabelResponse>> GetCardsByLabelIdAsync(int labelId)
    {
        if (await _labelRepository.GetByIdAsync(labelId) is null)
            throw new KeyNotFoundException("Label does not exist with id " + labelId);

        var cardLabels = await _repository.GetByLabelIdAsync(labelId);
        return cardLabels.Select(cl => new CardLabelResponse(cl));
    }

    public async Task<CardLabelResponse?> AddLabelToCardAsync(CreateCardLabelRequest request)
    {
        var card = await _cardRepository.GetByIdAsync(request.CardId);
        if (card is null)
            throw new KeyNotFoundException("Card does not exist with id " + request.CardId);

        if (await _labelRepository.GetByIdAsync(request.LabelId) is null)
            throw new KeyNotFoundException("Label does not exist with id " + request.LabelId);

        if (await _repository.ExistsAsync(request.CardId, request.LabelId))
            throw new InvalidOperationException("Label is already assigned to this card.");

        var cardLabel = new CardLabel(request.CardId, request.LabelId);
        _repository.Add(cardLabel);
        await _repository.SaveAsync();
        var response = new CardLabelResponse(cardLabel);

        var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
        if (listItem != null)
        {
            await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("LabelAddedToCard", response);
        }

        return response;
    }

    public async Task<bool> RemoveLabelFromCardAsync(int cardId, int labelId)
    {
        var cardLabel = await _repository.GetByIdsAsync(cardId, labelId);
        if (cardLabel == null)
            return false;

        var card = await _cardRepository.GetByIdAsync(cardId);
        _repository.Delete(cardLabel);
        await _repository.SaveAsync();

        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("LabelRemovedFromCard", new { cardId, labelId });
            }
        }

        return true;
    }
}
