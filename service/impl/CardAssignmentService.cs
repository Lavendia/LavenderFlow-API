using Microsoft.AspNetCore.SignalR;

public class CardAssignmentService : ICardAssignmentService
{
    private readonly ICardAssignmentRepository _repository;
    private readonly ICardRepository _cardRepository;
    private readonly IUserRepository _userRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IHubContext<LavenderFlowHub> _hub;

    public CardAssignmentService(
        ICardAssignmentRepository repository,
        ICardRepository cardRepository,
        IUserRepository userRepository,
        IListItemRepository listItemRepository,
        IHubContext<LavenderFlowHub> hub)
    {
        _repository = repository;
        _cardRepository = cardRepository;
        _userRepository = userRepository;
        _listItemRepository = listItemRepository;
        _hub = hub;
    }

    public async Task<IEnumerable<CardAssignmentResponse>?> GetAssignmentsByCardAsync(int cardId)
    {
        if (await _cardRepository.GetByIdAsync(cardId) is null)
            return null;

        var assignments = await _repository.GetByCardIdAsync(cardId);
        return assignments.Select(a => new CardAssignmentResponse(a));
    }

    public async Task<CardAssignmentResponse> CreateAssignmentAsync(CreateCardAssignmentRequest request)
    {
        var card = await _cardRepository.GetByIdAsync(request.CardId);
        if (card is null)
            throw new KeyNotFoundException("Card does not exist with id " + request.CardId);

        if (await _userRepository.GetByIdAsync(request.UserId) is null)
            throw new KeyNotFoundException("User does not exist with id " + request.UserId);

        var assignment = new CardAssignment(request.UserId, request.CardId);
        _repository.Add(assignment);
        await _repository.SaveAsync();
        var response = new CardAssignmentResponse(assignment);

        var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
        if (listItem != null)
        {
            await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("AssignmentCreated", response);
        }

        return response;
    }

    public async Task<bool> DeleteAssignmentAsync(int userId, int cardId)
    {
        var assignment = await _repository.GetByIdsAsync(userId, cardId);
        if (assignment == null)
            return false;

        var card = await _cardRepository.GetByIdAsync(cardId);
        _repository.Delete(assignment);
        await _repository.SaveAsync();

        if (card != null)
        {
            var listItem = await _listItemRepository.GetByIdAsync(card.ListItemId);
            if (listItem != null)
            {
                await _hub.Clients.Group(listItem.BoardId.ToString()).SendAsync("AssignmentDeleted", new { userId, cardId });
            }
        }

        return true;
    }
}
