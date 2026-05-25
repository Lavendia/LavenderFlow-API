public interface ICardRepository
{
    Task<List<Card>> GetAllAsync();
    Task<Card?> GetByIdAsync(int id);
    Task<List<Card>> GetByListIdAsync(int listId);
    void Add(Card card);
    void Delete(Card card);
    Task SaveAsync();
}
