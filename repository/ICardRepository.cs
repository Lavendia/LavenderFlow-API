public interface ICardRepository
{
    Task<List<Card>> GetAllAsync();
    Task<Card?> GetByIdAsync(int id);
    void Add(Card card);
    void Delete(Card card);
    Task SaveAsync();
}
