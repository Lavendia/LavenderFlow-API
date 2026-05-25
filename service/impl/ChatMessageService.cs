using Microsoft.AspNetCore.SignalR;

public class ChatMessageService : IChatMessageService
{
    private readonly IChatMessageRepository _repository;
    private readonly ICardRepository _cardRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public ChatMessageService(
        IChatMessageRepository repository,
        ICardRepository cardRepository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _cardRepository = cardRepository;
        _listItemRepository = listItemRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<ChatMessageResponse>> GetChatMessagesAsync()
    {
        var messages = await _repository.GetAllAsync();
        return messages.Select(m => new ChatMessageResponse(m));
    }

    public async Task<ChatMessageResponse?> GetChatMessageAsync(int id)
    {
        var message = await _repository.GetByIdAsync(id);
        return message == null ? null : new ChatMessageResponse(message);
    }

    public async Task<IEnumerable<ChatMessageResponse>> GetChatMessagesByCardIdAsync(int cardId)
    {
        var messages = await _repository.GetByCardIdAsync(cardId);
        return messages.Select(m => new ChatMessageResponse(m));
    }

    public async Task<ChatMessageResponse?> CreateChatMessageAsync(CreateChatMessageRequest request)
    {
        var card = await _cardRepository.GetByIdAsync(request.CardId);
        if (card is null)
            return null;

        var message = new ChatMessage(request.Text, request.CardId, request.UserId);
        _repository.Add(message);
        await _repository.SaveAsync();
        var response = new ChatMessageResponse(message);

        var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
        if (listItem != null)
        {
            await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChatMessageCreated", response);
        }

        return response;
    }

    public async Task<ChatMessageResponse?> UpdateChatMessageAsync(int id, UpdateChatMessageRequest request)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message == null)
            return null;

        if (request.Text is not null) message.Text = request.Text;
        await _repository.SaveAsync();
        var response = new ChatMessageResponse(message);

        var card = await _cardRepository.GetByIdAsync(message.CardId);
        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChatMessageUpdated", response);
            }
        }

        return response;
    }

    public async Task<bool> DeleteChatMessageAsync(int id)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message == null)
            return false;

        var card = await _cardRepository.GetByIdAsync(message.CardId);
        _repository.Delete(message);
        await _repository.SaveAsync();

        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("ChatMessageDeleted", id);
            }
        }

        return true;
    }
}
