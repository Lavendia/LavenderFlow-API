public interface ICardAssignmentRepository
{
    Task<List<CardAssignment>> GetByCardIdAsync(int cardId);
    Task<CardAssignment?> GetByIdsAsync(int userId, int cardId);
    void Add(CardAssignment assignment);
    void Delete(CardAssignment assignment);
    Task SaveAsync();
}
