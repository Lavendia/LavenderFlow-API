public class CardAssignmentService : ICardAssignmentService
{
    private readonly ICardAssignmentRepository _repository;
    private readonly ICardRepository _cardRepository;
    private readonly IUserRepository _userRepository;

    public CardAssignmentService(
        ICardAssignmentRepository repository,
        ICardRepository cardRepository,
        IUserRepository userRepository)
    {
        _repository = repository;
        _cardRepository = cardRepository;
        _userRepository = userRepository;
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
        if (await _cardRepository.GetByIdAsync(request.CardId) is null)
            throw new KeyNotFoundException("Card does not exist with id " + request.CardId);

        if (await _userRepository.GetByIdAsync(request.UserId) is null)
            throw new KeyNotFoundException("User does not exist with id " + request.UserId);

        var assignment = new CardAssignment(request.UserId, request.CardId);
        _repository.Add(assignment);
        await _repository.SaveAsync();
        return new CardAssignmentResponse(assignment);
    }

    public async Task<bool> DeleteAssignmentAsync(int userId, int cardId)
    {
        var assignment = await _repository.GetByIdsAsync(userId, cardId);
        if (assignment == null)
            return false;

        _repository.Delete(assignment);
        await _repository.SaveAsync();
        return true;
    }
}
